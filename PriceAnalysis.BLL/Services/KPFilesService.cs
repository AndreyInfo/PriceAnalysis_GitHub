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

public class KPFilesService : IKPFilesService
{
    private readonly IKPFilesRepository _kpFilesRepository;
    private readonly IMapper _mapper;
    private readonly IAlertService _alertService;
    private readonly IDocFilesRepository _docFilesRepository;
    public IConfiguration _configuration { get; }
    public readonly IFileStorageRepository _fileStorageRepository;

    public KPFilesService(
          IKPFilesRepository kpFilesRepository
        , IMapper mapper
        , IAlertService alertService
        , IDocFilesRepository docFilesRepository
        , IConfiguration configuration
        , IFileStorageRepository fileStorageRepository
        )
    {
        _kpFilesRepository = kpFilesRepository;
        _mapper = mapper;
        _alertService = alertService;
        _docFilesRepository = docFilesRepository;
        _configuration = configuration;
        _fileStorageRepository = fileStorageRepository;
    }

    public KPFilesDto GetById(int Id)
    {
        var entity = _kpFilesRepository.GetById(Id);

        return _mapper.Map<KPFilesDto>(entity);
    }

    public List<KPFilesDto> GetList()
    {
        var list = _kpFilesRepository.GetList();

        return _mapper.Map<List<KPFilesDto>>(list);
    }

    public NotificationMessage CreateItem(KPFilesDto model)
    {
        try
        {
            _kpFilesRepository.CreateItem(_mapper.Map<KPFilesEntity>(model));

            return _alertService.Get(3);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public NotificationMessage UpdateItem(KPFilesDto model)
    {
        try
        {
            _kpFilesRepository.UpdateItem(_mapper.Map<KPFilesEntity>(model));
            return _alertService.Get(2);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public NotificationMessage DeleteItem(int Id)
    {
        try
        {
            _kpFilesRepository.DeleteItem(Id);

            return _alertService.Get(4);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public async Task<NotificationMessage> UploadKPFile(KPFilesDto model)
    {
        var check = CheckUploadModel(model);
        if (check.Severity != NotificationSeverity.Success)
        {
            return check;
        }

        try
        {
            var kpFile = _kpFilesRepository.GetList().Where(x => x.SectionId == model.SectionId)
                .Where(x => x.SupplierId == model.SupplierId).FirstOrDefault();

            string typeFile = CheckTypeFile(model.File);

            DocFilesEntity entityFile = new DocFilesEntity();
            entityFile.StreamData = model.File.Data;
            entityFile.FileName = model.File.Name;
            int docFileId = await _fileStorageRepository.SaveFileInDocFiles(entityFile);

            KPFilesEntity newKPFile = new KPFilesEntity();
            newKPFile.SectionId = model.SectionId;
            newKPFile.Notice = model.Notice;
            newKPFile.SupplierId = model.SupplierId;
            switch (typeFile)
            {
                case "pdf":
                    newKPFile.DocFilesIdPdf = docFileId;
                    break;
                case "excel":
                    newKPFile.DocFilesIdExcel = docFileId;
                    break;
                default:
                    break;
            }

            if (kpFile is null)
            {
                _kpFilesRepository.CreateItem(newKPFile);
            }
            else if (typeFile == "pdf")
            {
                if (kpFile.DocFilesIdPdf is not null) 
                {
                    var docFileOld = _docFilesRepository.GetItem((int)kpFile.DocFilesIdPdf);
                    _fileStorageRepository.DeleteFileInDocFiles(docFileOld);
                }

                kpFile.DocFilesIdPdf = docFileId;
                kpFile.Notice = model.Notice;
                _kpFilesRepository.UpdateItem(kpFile);
            }
            else if (typeFile == "excel")
            {
                if (kpFile.DocFilesIdExcel is not null)
                {
                    var docFileOld = _docFilesRepository.GetItem((int)kpFile.DocFilesIdExcel);
                    _fileStorageRepository.DeleteFileInDocFiles(docFileOld);
                }

                kpFile.DocFilesIdExcel = docFileId;
                kpFile.Notice = model.Notice;
                _kpFilesRepository.UpdateItem(kpFile);
            }

            return _alertService.Get(14);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public NotificationMessage DeleteFileKP(int kpFileId)
    {
        try
        {
            var kpFile = _kpFilesRepository.GetById(kpFileId);

            if (kpFile.DocFilesIdPdf is not null)
            {
                var file = _docFilesRepository.GetItem((int)kpFile.DocFilesIdPdf);
                _fileStorageRepository.DeleteFileInDocFiles(file);
            }

            if (kpFile.DocFilesIdExcel is not null)
            {
                var file = _docFilesRepository.GetItem((int)kpFile.DocFilesIdExcel);
                _fileStorageRepository.DeleteFileInDocFiles(file);
            }

            _kpFilesRepository.DeleteItem(kpFileId);

            return _alertService.Get(15);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public (NotificationMessage, string) DownloadFilesKP(int kpFileId)
    {
        try
        {
            var kpFile = _kpFilesRepository.GetById(kpFileId);

            List<int> docFileIdList = new List<int>();

            if (kpFile.DocFilesIdPdf is not null)
            {
                docFileIdList.Add((int)kpFile.DocFilesIdPdf);
            }

            if (kpFile.DocFilesIdExcel is not null)
            {
                docFileIdList.Add((int)kpFile.DocFilesIdExcel);
            }

            string subpath = _fileStorageRepository.CopyDocFilesInTempDirAndCreateZip(docFileIdList);

            string path = @"temp/" + subpath + ".zip";

            return (_alertService.GetSuccess(), path);
        }
        catch (Exception e)
        {
            return (_alertService.GetError("Ошибка при получении файла! " + e.Message), "");
        }
    }

    private NotificationMessage CheckUploadModel(KPFilesDto model)
    {
        if (model.SupplierId == 0)
        {
            return _alertService.GetError("Не выбран поставщик!");
        }

        if (model.File is null)
        {
            return _alertService.GetError("Не выбран файл для загрузки!");
        }

        if (model.File.Size >= 10000000)
        {
            return _alertService.GetError("Превышен размер загружаемого файла!");
        }

        if (CheckTypeFile(model.File) != "excel" && CheckTypeFile(model.File) != "pdf")
        {
            return _alertService.GetError("Для загрузки допускаются только pdf или excel файлы!");
        }

        return _alertService.GetSuccess();
    }

    private string CheckTypeFile(IFileListEntry file)
    {
        switch (file.Type)
        {
            case "application/vnd.ms-excel": return "excel";
            case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet": return "excel";
            case "application/pdf": return "pdf";

            default: return "";
        }
    }
}
