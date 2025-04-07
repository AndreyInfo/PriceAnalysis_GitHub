using AutoMapper;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using PriceAnalysis.BLL.Models;
using PriceAnalysis.BLL.Services.Support;
using PriceAnalysis.DAL;
using PriceAnalysis.DAL.Models;
using PriceAnalysis.DAL.Models.Reference;
using PriceAnalysis.DAL.Repository;
using PriceAnalysis.DAL.Repository.Support;
using Radzen;
using FileInfo = System.IO.FileInfo;

namespace PriceAnalysis.BLL.Services;

public class SynchSpecificationAndKPService : ISynchSpecificationAndKPService
{
    private readonly ISynchPriceRepository _synchPriceRepository;
    private readonly ISynchSpecificationRepository _synchSpecificationRepository;
    private readonly IMapper _mapper;
    private readonly IKPFilesRepository _kpFilesRepository;
    private readonly IAlertService _alertService;
    private readonly ISynchSpecificationAndKPPrepRepository _synchSpecificationAndKPPrepRepository;
    private readonly IDocFilesRepository _docFilesRepository;
    public IConfiguration _configuration { get; }
    private readonly ISectionRepository _sectionRepository;
    public readonly IReferencesRepository _referencesRepository;
    private readonly DBContext _context;

    public SynchSpecificationAndKPService(
          ISynchPriceRepository synchPriceRepository
        , ISynchSpecificationRepository synchSpecificationRepository
        , IMapper mapper
        , IKPFilesRepository kPFilesRepository
        , IAlertService alertService
        , ISynchSpecificationAndKPPrepRepository synchSpecificationAndKPPrepRepository
        , IDocFilesRepository docFilesRepository
        , IConfiguration configuration
        , ISectionRepository sectionRepository
        , IReferencesRepository referencesRepository
        , DBContext context
        )
    {
        _synchPriceRepository = synchPriceRepository;
        _synchSpecificationRepository = synchSpecificationRepository;
        _mapper = mapper;
        _kpFilesRepository = kPFilesRepository;
        _alertService = alertService;
        _synchSpecificationAndKPPrepRepository = synchSpecificationAndKPPrepRepository;
        _docFilesRepository = docFilesRepository;
        _configuration = configuration;
        _sectionRepository = sectionRepository;
        _referencesRepository = referencesRepository;
        _context = context;
    }

    public NotificationMessage CreateItemSynchSpecificationAndKPPrep(int sectionId)
    {
        try
        {
            SynchSpecificationAndKPPrepEntity entity = new SynchSpecificationAndKPPrepEntity();

            entity.SectionId = sectionId;
            entity.IsSynch = false;
            _synchSpecificationAndKPPrepRepository.CreateItem(entity);

            return _alertService.Get(3);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public List<SynchSpecificationAndKPResultDto> GetSynchSpecificationAndKP(int sectionId)
    {
        var synchSpecificationList = _synchSpecificationRepository.GetList()
            .Where(x => x.SectionId == sectionId).ToList();

        var synchSpecificationListID = from n in synchSpecificationList
                                       select n.Id;

        var synchPrices = from n in _synchPriceRepository.GetList()
                          join t in synchSpecificationListID on n.SynchSpecificationId equals t
                          select n;

        List<(int, string)> suppliers = new List<(int, string)>();
        foreach (var pr in synchPrices)
        {
            if (!suppliers.Contains((pr.SupplierId, pr.Supplier.Name)))
            {
                suppliers.Add((pr.SupplierId, pr.Supplier.Name));
            }
        }

        List<SynchSpecificationAndKPResultDto> list = new List<SynchSpecificationAndKPResultDto>();

        foreach (var item in synchSpecificationList)
        {
            var prices = synchPrices.Where(x => x.SynchSpecificationId == item.Id);

            List<SynchPriceDto> synchPricesDto = new List<SynchPriceDto>();
            for (int i = 0; i < suppliers.Count; i++)
            {
                float price = 0;
                int id = 0;
                var synchPrice = prices.FirstOrDefault(x => x.SupplierId == suppliers[i].Item1);
                if (synchPrice is not null)
                {
                    price = (float)synchPrice.Price;
                    id = synchPrice.Id;
                }

                synchPricesDto.Add(new SynchPriceDto()
                {
                    Id = id,
                    Price = price,
                    SupplierId = suppliers[i].Item1,
                    SupplierName = suppliers[i].Item2
                });
            }

            list.Add(new SynchSpecificationAndKPResultDto()
            {
                SynchSpecificationId = item.Id,
                SectionId = item.SectionId,
                CodeProduct = item.CodeProduct,
                TypeProduct = item.TypeProduct,
                Name = item.Name,
                Manufacturer = item.Manufacturer,
                UnitOfMeasurementId = item.UnitOfMeasurementId,
                UnitOfMeasurementName = item.UnitOfMeasurement.Name,
                SynchPriceList = synchPricesDto
            });
        }
        return list;
    }

    public List<SynchSpecificationAndKPPrepDto> GetListSynchSpecificationAndKPPrep(Guid projectId)
    {
        List<SynchSpecificationAndKPPrepDto> list = new List<SynchSpecificationAndKPPrepDto>();
        var sections = _sectionRepository.GetList().Where(x => x.ProjectId == projectId);

        var sectionsId = from n in sections
                         select n.Id;

        var sectionsSynchPrep = from n in _synchSpecificationAndKPPrepRepository.GetList()
                                join t in sectionsId on n.SectionId equals t
                                select n;
        try
        {
            foreach (var item in sectionsSynchPrep)
            {
                var kpFiles = _kpFilesRepository.GetList().Where(x => x.SectionId == item.SectionId);
                List<ExistFilesForKPDto> existFiles = new List<ExistFilesForKPDto>();
                int i = 1;
                foreach (var item2 in kpFiles)
                {
                    existFiles.Add(new ExistFilesForKPDto()
                    {
                        Name = "КП" + i,
                        ExistExcel = item2.DocFilesIdExcel == null ? false : true,
                        ExistPDF = item2.DocFilesIdPdf == null ? false : true
                    });
                    i++;
                }
                var section = _sectionRepository.GetById(item.SectionId);

                SynchSpecificationAndKPPrepDto model = new SynchSpecificationAndKPPrepDto();
                model.SectionId = section.Id;
                model.SectionName = section.Name;
                model.CountStringInSpecification = GetCountStringInSpecification(item.SectionId);
                model.ExistSpecification = true;
                model.IsSynch = item.IsSynch;
                if (model.CountStringInSpecification == 0)
                {
                    model.ExistSpecification = false;
                }

                model.ListExistFilesForKP = existFiles;
                list.Add(model);
            }
            return list;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private int GetCountStringInSpecification(int sectionId)
    {
        var specificationDocFileId = _sectionRepository.GetById(sectionId).SpecificationDocFileId;

        if (specificationDocFileId is null)
        {
            return 0;
        }

        string guid = _docFilesRepository.GetItem((int)specificationDocFileId).GUID.ToString();
        string filePath = _configuration.GetSection("AppSettings:DocFilesPath").Value + guid;
        System.IO.FileInfo existingFile = new System.IO.FileInfo(filePath);

        int rowCount;
        using (ExcelPackage package = new ExcelPackage(existingFile))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            rowCount = worksheet.Dimension.End.Row;
        }

        return rowCount - 1;
    }

    public NotificationMessage DeleteItemSynchSpecificationAndKPPrep(int sectionId)
    {
        try
        {
            var entity = _synchSpecificationAndKPPrepRepository.GetList().FirstOrDefault(x => x.SectionId == sectionId);
            _synchSpecificationAndKPPrepRepository.DeleteItem(entity.Id);

            return _alertService.Get(4);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public NotificationMessage Synchronization(int sectionId)
    {
        //Шаг 1. Получаем файл специф. Данные записываем в SynchSpecification
        var dataSynchSpecification = CreateDataSynchSpecification(sectionId);
        if (dataSynchSpecification.Item1.Severity != NotificationSeverity.Success)
        {
            return dataSynchSpecification.Item1;
        }

        //Шаг 2. Анализируем файлы КП по очереди
        //Записываем данные в SynchPrice на основе List<SynchSpecificationEntity>
        var kpFiles = _kpFilesRepository.GetList().Where(x => x.SectionId == sectionId);
        var synchSpecification = dataSynchSpecification.Item2;

        List<SynchPriceEntity> priceList = new List<SynchPriceEntity>();
        foreach (var item in kpFiles)
        {
            if (item.DocFilesIdExcel is null)
            {
                continue;
            }
            string guid = _docFilesRepository.GetItem((int)item.DocFilesIdExcel).GUID.ToString();
            string filePath = _configuration.GetSection("AppSettings:DocFilesPath").Value + guid;
            FileInfo existingFile = new FileInfo(filePath);

            try
            {
                using (ExcelPackage package = new ExcelPackage(existingFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.End.Row;

                    //Поиск совпадений по коду продукта. 1-й проход
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (worksheet.Cells[row, 4].Value is not null)
                        {
                            if (synchSpecification
                                .Any(x => x.CodeProduct == worksheet.Cells[row, 4].Value.ToString()))
                            {
                                var priceItem = synchSpecification
                                    .FirstOrDefault(x => x.CodeProduct == worksheet.Cells[row, 4].Value.ToString());

                                if (priceList.Where(x => x.SupplierId == item.SupplierId)
                                    .Any(z => z.SynchSpecificationId == priceItem.Id))
                                {
                                    continue;
                                }

                                priceList.Add(new SynchPriceEntity
                                {
                                    SynchSpecificationId = priceItem.Id,
                                    Price = float.Parse(worksheet.Cells[row, 8].Value.ToString()),
                                    SupplierId = item.SupplierId
                                });
                            }
                        }
                    }

                    //Поиск совпадений по типу/марке. 2-й проход
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (worksheet.Cells[row, 3].Value is not null)
                        {
                            if (synchSpecification
                                .Any(x => x.TypeProduct == worksheet.Cells[row, 3].Value.ToString()))
                            {
                                var priceItem = synchSpecification
                                    .FirstOrDefault(x => x.TypeProduct == worksheet.Cells[row, 3].Value.ToString());

                                if (priceList.Where(x => x.SupplierId == item.SupplierId)
                                    .Any(z => z.SynchSpecificationId == priceItem.Id))
                                {
                                    continue;
                                }

                                priceList.Add(new SynchPriceEntity
                                {
                                    SynchSpecificationId = priceItem.Id,
                                    Price = float.Parse(worksheet.Cells[row, 8].Value.ToString()),
                                    SupplierId = item.SupplierId
                                });
                            }
                        }
                    }

                    //Поиск совпадений по наименованию. 3-й проход
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (worksheet.Cells[row, 2].Value is not null)
                        {
                            if (synchSpecification
                                .Any(x => x.Name == worksheet.Cells[row, 2].Value.ToString()))
                            {
                                var priceItem = synchSpecification
                                    .FirstOrDefault(x => x.Name == worksheet.Cells[row, 2].Value.ToString());

                                if (priceList.Where(x => x.SupplierId == item.SupplierId)
                                    .Any(z => z.SynchSpecificationId == priceItem.Id))
                                {
                                    continue;
                                }

                                priceList.Add(new SynchPriceEntity
                                {
                                    SynchSpecificationId = priceItem.Id,
                                    Price = float.Parse(worksheet.Cells[row, 8].Value.ToString()),
                                    SupplierId = item.SupplierId
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return _alertService.GetError("Ошибка при синхронизации!" + e.Message);
            }
        }
        _synchPriceRepository.CreateList(priceList);

        //Шаг 3.В таблице SynchSpecificationAndKPPrep правим флаг "IsSynch"
        //TODO Реализовать удаление dataSynchSpecification (откат). Транзакция здесь не поможет.
        //Если транзакция то pk переделать на гуид, для генерации здесь, а не в бд.
        var synchSpecificationAndKPPrep = _synchSpecificationAndKPPrepRepository.GetList().FirstOrDefault(x => x.SectionId == sectionId);
        synchSpecificationAndKPPrep.IsSynch = true;
        _synchSpecificationAndKPPrepRepository.UpdateItem(synchSpecificationAndKPPrep);

        return _alertService.Get(50);
    }

    public NotificationMessage ReSynchronization(int sectionId)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                var synchSpecificationAndKPPrep = _synchSpecificationAndKPPrepRepository.GetList().FirstOrDefault(x => x.SectionId == sectionId);
                synchSpecificationAndKPPrep.IsSynch = false;
                _synchSpecificationAndKPPrepRepository.UpdateItem(synchSpecificationAndKPPrep);

                var listSynchSpecification = _synchSpecificationRepository.GetList()
                            .Where(x => x.SectionId == sectionId).ToList();

                foreach (var item in listSynchSpecification)
                {
                    var synchPrice = _synchPriceRepository.GetList().FirstOrDefault(x => x.SynchSpecificationId == item.Id);

                    if (synchPrice is not null)
                    {
                        _synchPriceRepository.DeleteItem(synchPrice.Id);
                    }
                }

                _synchSpecificationRepository.DeleteList(listSynchSpecification);

                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return _alertService.GetError("Во время транзакции произошла ошибка!" + e.Message);
            }
        }

        var res = Synchronization(sectionId);

        if (res.Severity != NotificationSeverity.Success)
        {
            return _alertService.Get(51);
        }

        return _alertService.Get(50);
    }

    private (NotificationMessage, List<SynchSpecificationEntity>) CreateDataSynchSpecification(int sectionId)
    {
        var specificationDocFileId = _sectionRepository.GetById(sectionId).SpecificationDocFileId;

        string guid = _docFilesRepository.GetItem((int)specificationDocFileId).GUID.ToString();
        string filePath = _configuration.GetSection("AppSettings:DocFilesPath").Value + guid;
        FileInfo existingFile = new FileInfo(filePath);
        var unitOfMeasurementList = _referencesRepository.GetUnitOfMeasurementList();

        List<SynchSpecificationEntity> list = new List<SynchSpecificationEntity>();

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
                            list.Add(new SynchSpecificationEntity
                            {
                                SectionId = sectionId,
                                CodeProduct = worksheet.Cells[row, 4].Value == null ? string.Empty : worksheet.Cells[row, 4].Value.ToString(),
                                TypeProduct = worksheet.Cells[row, 3].Value == null ? string.Empty : worksheet.Cells[row, 3].Value.ToString(),
                                Name = worksheet.Cells[row, 2].Value.ToString(),
                                Manufacturer = worksheet.Cells[row, 5].Value.ToString(),
                                UnitOfMeasurementId = unit.Id
                            });
                        }
                    }
                    else
                    {
                        return (_alertService.GetError("Ошибка при обработке единиц измерения. Строка " + row), null);
                    }
                }
            }

            _synchSpecificationRepository.CreateList(list);

            return (_alertService.Get(50), list);
        }
        catch (Exception e)
        {
            return (_alertService.GetError("Ошибка при обработке файла синхронизации! " + e.Message), null);
        }
    }

}
