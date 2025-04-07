using AutoMapper;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using PriceAnalysis.BLL.Models;
using PriceAnalysis.BLL.Services.Support;
using PriceAnalysis.DAL;
using PriceAnalysis.DAL.Models;
using PriceAnalysis.DAL.Models.Reference;
using PriceAnalysis.DAL.Models.Support;
using PriceAnalysis.DAL.Repository;
using PriceAnalysis.DAL.Repository.FileStorage;
using PriceAnalysis.DAL.Repository.Support;
using Radzen;
using FileInfo = System.IO.FileInfo;

namespace PriceAnalysis.BLL.Services;

public class SpecificationService : ISpecificationService
{
    private readonly ISpecificationRepository _specificationRepository;
    private readonly IChapterForSpecificationRepository _chapterForSpecificationRepository;
    private readonly IMapper _mapper;
    private readonly IAlertService _alertService;
    public IConfiguration _configuration { get; }
    public readonly IDocFilesRepository _docFilesRepository;
    public readonly IReferencesRepository _referencesRepository;
    public readonly IFileStorageRepository _fileStorageRepository;
    private readonly DBContext _context;
    private readonly ISectionRepository _sectionRepository;

    public SpecificationService(
          ISpecificationRepository specificationRepository
        , IChapterForSpecificationRepository chapterForSpecificationRepository
        , IMapper mapper
        , IAlertService alertService
        , IConfiguration configuration
        , IDocFilesRepository docFilesRepository
        , IReferencesRepository referencesRepository
        , IFileStorageRepository fileStorageRepository
        , DBContext context
        , ISectionRepository sectionRepository)
    {
        _specificationRepository = specificationRepository;
        _mapper = mapper;
        _alertService = alertService;
        _configuration = configuration;
        _docFilesRepository = docFilesRepository;
        _referencesRepository = referencesRepository;
        _fileStorageRepository = fileStorageRepository;
        _chapterForSpecificationRepository = chapterForSpecificationRepository;
        _context = context;
        _sectionRepository = sectionRepository;
    }

    public SpecificationDto GetById(int id)
    {
        var entity = _specificationRepository.GetById(id);

        return _mapper.Map<SpecificationDto>(entity);
    }

    public List<SpecificationDto> GetList()
    {
        var list = _specificationRepository.GetList();

        return _mapper.Map<List<SpecificationDto>>(list);
    }

    public NotificationMessage CreateItem(SpecificationDto model)
    {
        try
        {
            _specificationRepository.CreateItem(_mapper.Map<SpecificationEntity>(model));

            return _alertService.Get(3);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public NotificationMessage DeleteItem(int id)
    {
        try
        {
            _specificationRepository.DeleteItem(id);

            return _alertService.Get(4);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public (NotificationMessage, string) DownloadFileSpecifications(int sectionId)
    {
        try
        {
            var section = _sectionRepository.GetById(sectionId);
            var docFile = _docFilesRepository.GetItem((int)section.SpecificationDocFileId);

            _fileStorageRepository.CopyDocFileInTempDir(docFile);

            string path = @"temp/" + "_" + docFile.GUID.ToString() + @"/" + docFile.FileName;

            return (_alertService.GetSuccess(), path);
        }
        catch (Exception e)
        {
            return (_alertService.GetError("Ошибка при получении файла! " + e.Message), "");
        }
    }

    public (NotificationMessage, List<SpecificationEntity>) CreateDataSpecification(string guid, int sectionId, string fileName)
    {
        string filePath = @"wwwroot/temp/" + guid;
        FileInfo existingFile = new FileInfo(filePath);
        List<SpecificationEntity> specifications = new List<SpecificationEntity>();
        List<ChapterForSpecificationEntity> chapters = new List<ChapterForSpecificationEntity>();
        ChapterForSpecificationEntity chapter = null;
        var unitOfMeasurementList = _referencesRepository.GetUnitOfMeasurementList();

        //Сохраняем файл спецификации
        try
        {
            string newFilePath = _configuration.GetSection("AppSettings:DocFilesPath").Value + guid;
            File.Copy(filePath, newFilePath, true);

            DocFilesDto model = new DocFilesDto();
            model.GUID = Guid.Parse(guid);
            model.CreateDate = DateTime.UtcNow;
            model.CreatorId = 1;
            model.FileName = fileName;

            var en = _mapper.Map<DocFilesEntity>(model);
            int docFilesId = _docFilesRepository.CreateItem(en);

            var sectionEn = _sectionRepository.GetById(sectionId);
            sectionEn.SpecificationDocFileId = docFilesId;
            _sectionRepository.UpdateItem(sectionEn);
        }
        catch (Exception)
        {
            //TODO Добавить запись об ошибке в лог
            return (_alertService.Get(53), null);
        }

        //Готовим данные для сохранения
        try
        {
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.End.Row;

                for (int row = 2; row <= rowCount; row++)
                {
                    //Проверка на заголовок
                    if (worksheet.Cells[row, 1].Value == null)
                    {
                        chapter = new ChapterForSpecificationEntity()
                        { 
                            Id = Guid.NewGuid(),
                            Name = worksheet.Cells[row, 2].Value.ToString()
                        };
                        chapters.Add(chapter);
                        continue;
                    }

                    //Ед.изм. и кол-во
                    if (worksheet.Cells[row, 6].Value != null && worksheet.Cells[row, 7].Value != null)
                    {
                        UnitOfMeasurementEntity unit = unitOfMeasurementList.Where(x => x.Name.ToLower()
                        == worksheet.Cells[row, 6].Value.ToString().Trim().ToLower()).FirstOrDefault();
                        if (unit == null)
                        {
                            string a = worksheet.Cells[row, 6].Value.ToString().Trim().ToLower();
                            foreach (var el in unitOfMeasurementList)
                            {
                                if (string.IsNullOrEmpty(el.Aliases))
                                {
                                    continue;
                                }
                                string[] ar = el.Aliases.Split(new char[] { '#' });
                                var itemAr = ar.Where(x => x.Trim().ToLower() == a.Trim().ToLower()).FirstOrDefault();
                                if (itemAr != null)
                                {
                                    unit = el;
                                    break;
                                }
                            }
                        }

                        double count;
                        bool isIntCount = double.TryParse(worksheet.Cells[row, 7].Value.ToString(), out count);
                        if (unit == null || !isIntCount)
                        {
                            return (_alertService.GetError("Ошибка при обработке единиц измерения. Строка " + row), null);
                        }

                        if (unit != null)
                        {
                            Random random = new Random();
                            int rndInt = random.Next(10000, 99999);

                            specifications.Add(new SpecificationEntity
                            {
                                SectionId = sectionId,
                                SID = sectionId + "-" + rndInt.ToString(),
                                Name = worksheet.Cells[row, 2].Value.ToString(),
                                Type = worksheet.Cells[row, 3].Value == null ? string.Empty : worksheet.Cells[row, 3].Value.ToString(),
                                Code = worksheet.Cells[row, 4].Value == null ? string.Empty : worksheet.Cells[row, 4].Value.ToString(),
                                Manufacturer = worksheet.Cells[row, 5].Value.ToString(),
                                UnitOfMeasurementId = unit.Id,
                                Count = double.Parse(worksheet.Cells[row, 7].Value.ToString()),
                                ChapterId = chapter == null ? null : chapter.Id
                            });
                        }
                    }
                    else
                    {
                        return (_alertService.GetError("Ошибка при обработке единиц измерения. Строка " + row), null);
                    }
                }
            }

        }
        catch (Exception)
        {
            //TODO Добавить запись об ошибке в лог
            return (_alertService.Get(53), null);
        }

        //Готовим данные для удаления, если они есть
        List<ChapterForSpecificationEntity> chaptersOld = null;
        List<SpecificationEntity> specificationsOld = null;

        try
        {
            specificationsOld = _specificationRepository.GetList().Where(x => x.SectionId == sectionId).ToList();

            if (specificationsOld.Count != 0)
            {
                var specificationsOldListID = from n in specificationsOld
                                              select n.ChapterId;

                specificationsOldListID = specificationsOldListID.Distinct();

                var chaptersOldPrev = from n in _chapterForSpecificationRepository.GetList()
                                      join t in specificationsOldListID on n.Id equals t
                                      select n;

                chaptersOld = chaptersOldPrev.ToList();
            }
        }
        catch (Exception)
        {
            //TODO Добавить запись об ошибке в лог
            return (_alertService.Get(53), null);
        }

        //Удаляем старые данные и добавляем новые в одной транзакции
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                if (specificationsOld.Count != 0)
                {
                    _specificationRepository.DeleteListBySectionId(sectionId);
                    _chapterForSpecificationRepository.DeleteList(chaptersOld);
                }

                _chapterForSpecificationRepository.CreateList(chapters);
                _specificationRepository.CreateList(specifications);

                transaction.Commit();
                return (_alertService.Get(52), specifications);
            }
            catch (Exception)
            {
                transaction.Rollback();
                //TODO Добавить запись об ошибке в лог
                return (_alertService.Get(53), null);
            }
        }
    }
}
