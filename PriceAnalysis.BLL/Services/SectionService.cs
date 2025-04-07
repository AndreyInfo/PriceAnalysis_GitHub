using AutoMapper;
using BlazorInputFile;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PriceAnalysis.BLL.Models;
using PriceAnalysis.BLL.Services.Support;
using PriceAnalysis.DAL.Models;
using PriceAnalysis.DAL.Models.Support;
using PriceAnalysis.DAL.Repository;
using PriceAnalysis.DAL.Repository.FileStorage;
using PriceAnalysis.DAL.Repository.Support;
using Radzen;

namespace PriceAnalysis.BLL.Services;

public class SectionService : ISectionService
{
    private readonly ISectionRepository _sectionRepository;
    private readonly IMapper _mapper;
    private readonly IAlertService _alertService;
    private readonly IDocFilesRepository _docFilesRepository;
    public IConfiguration _configuration { get; }
    public readonly IFileStorageRepository _fileStorageRepository;
    public readonly IKPFilesRepository _kpFilesRepository;

    public SectionService(
          ISectionRepository sectionRepository
        , IMapper mapper
        , IAlertService alertService
        , IDocFilesRepository docFilesRepository
        , IConfiguration configuration
        , IFileStorageRepository fileStorageRepository
        , IKPFilesRepository kpFilesRepository
        )
    {
        _sectionRepository = sectionRepository;
        _mapper = mapper;
        _alertService = alertService;
        _docFilesRepository = docFilesRepository;
        _configuration = configuration;
        _fileStorageRepository = fileStorageRepository;
        _kpFilesRepository = kpFilesRepository;
    }

    public SectionDto GetById(int Id)
    {
        var entity = _sectionRepository.GetById(Id);

        return _mapper.Map<SectionDto>(entity);
    }

    public List<SectionDto> GetList()
    {
        var list = _sectionRepository.GetList();

        return _mapper.Map<List<SectionDto>>(list);
    }

    public NotificationMessage CreateItem(SectionDto model)
    {
        try
        {
            _sectionRepository.CreateItem(_mapper.Map<SectionEntity>(model));

            return _alertService.Get(3);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public NotificationMessage DeleteItem(int sectionId)
    {
        try
        {
            var specificationDocFileId = _sectionRepository.GetById(sectionId).SpecificationDocFileId;

            if (specificationDocFileId != null) 
            {
                var docFile = _docFilesRepository.GetItem((int)specificationDocFileId);
                string filePath = _configuration.GetSection("AppSettings:DocFilesPath").Value + docFile.GUID.ToString();

                //Удаляем запись в DocFiles
                _docFilesRepository.DeleteItem((int)specificationDocFileId);

                //Удаляем файл
                File.Delete(filePath);
            }

            //Удаляем запись в Sections
            _sectionRepository.DeleteItem(sectionId);

            return _alertService.Get(4);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public NotificationMessage UpdateItem(SectionDto model)
    {
        try
        {
            _sectionRepository.UpdateItem(_mapper.Map<SectionEntity>(model));
            return _alertService.Get(2);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public async Task<NotificationMessage> UploadTablePriceAnalysis(IFileListEntry file, int sectionId)
    {
        var check = CheckUploadFile(file);
        if (check.Severity != NotificationSeverity.Success) 
        {
            return check;
        }

        try
        {
            DocFilesEntity entityFile = new DocFilesEntity();
            entityFile.StreamData = file.Data;
            entityFile.FileName = file.Name;

            int docFileId = await _fileStorageRepository.SaveFileInDocFiles(entityFile);

            var section = _sectionRepository.GetById(sectionId);
            section.PriceAnalysisDocFileId = docFileId;
            _sectionRepository.UpdateItem(section);

            return _alertService.Get(14);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public async Task<NotificationMessage> UploadFilesKP(IFileListEntry[] files, int sectionId)
    {
        List<int> docFileIdList = new List<int>();
        var section = _sectionRepository.GetById(sectionId);

        foreach (var file in files)
        {
            var check = CheckUploadFile(file);
            if (check.Severity != NotificationSeverity.Success)
            {
                return check;
            }
        }

        try
        {
            foreach (IFileListEntry file in files) 
            {
                DocFilesEntity entityFile = new DocFilesEntity();
                entityFile.StreamData = file.Data;
                entityFile.FileName = file.Name;

                int docFileId = await _fileStorageRepository.SaveFileInDocFiles(entityFile);
                docFileIdList.Add(docFileId);
            }
            
            var docFileIdLisJson = JsonConvert.SerializeObject(docFileIdList, Formatting.Indented);

            section.KPDocFilesId = docFileIdLisJson;
            _sectionRepository.UpdateItem(section);

            return _alertService.Get(14);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public NotificationMessage DeleteTablePriceAnalysis(int sectionId)
    {
        try
        {
            var section = _sectionRepository.GetById(sectionId);
            var docFile = _docFilesRepository.GetItem((int)section.PriceAnalysisDocFileId);

            _fileStorageRepository.DeleteFileInDocFiles(docFile);

            section.PriceAnalysisDocFileId = null;
            _sectionRepository.UpdateItem(section);

            return _alertService.Get(15);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public NotificationMessage DeleteFilesKP(int sectionId)
    {
        try
        {
            var section = _sectionRepository.GetById(sectionId);

            List<int> docFileIdList = JsonConvert.DeserializeObject<List<int>>(section.KPDocFilesId);

            foreach (var item in docFileIdList)
            {
                var docFile = _docFilesRepository.GetItem(item);
                _fileStorageRepository.DeleteFileInDocFiles(docFile);
            }

            section.KPDocFilesId = null;
            _sectionRepository.UpdateItem(section);

            return _alertService.Get(15);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public (NotificationMessage, string) DownloadFilePriceAnalysis(int sectionId)
    {
        try
        {
            var section = _sectionRepository.GetById(sectionId);
            var docFile = _docFilesRepository.GetItem((int)section.PriceAnalysisDocFileId);

            _fileStorageRepository.CopyDocFileInTempDir(docFile);

            string path = @"temp/" + docFile.GUID.ToString() + @"/" + docFile.FileName;

            return (_alertService.GetSuccess(), path);
        }
        catch (Exception e)
        {
            return (_alertService.GetError("Ошибка при получении файла! " + e.Message), "");
        }
    }

    public (NotificationMessage, string) DownloadFilesKP(int sectionId)
    {
        try
        {
            var section = _sectionRepository.GetById(sectionId);
            List<int> docFileIdList = JsonConvert.DeserializeObject<List<int>>(section.KPDocFilesId);

            string subpath = _fileStorageRepository.CopyDocFilesInTempDirAndCreateZip(docFileIdList);

            string path = @"temp/" + subpath + ".zip";

            return (_alertService.GetSuccess(), path);
        }
        catch (Exception e)
        {
            return (_alertService.GetError("Ошибка при получении файла! " + e.Message), "");
        }
    }

    private NotificationMessage CheckUploadFile(IFileListEntry file)
    {
        if (file.Size >= 10000000)
        {
            return _alertService.GetError("Превышен размер загружаемого файла!");
        }

        if (file.Type != "application/pdf" 
            && file.Type != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            && file.Type != "application/vnd.ms-excel")
        {
            return _alertService.GetError("Для загрузки допускаются только pdf или excel файлы!");
        }

        return _alertService.GetSuccess();
    }
    
}
