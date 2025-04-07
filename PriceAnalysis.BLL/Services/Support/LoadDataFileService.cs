using AutoMapper;
using BlazorInputFile;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using PriceAnalysis.BLL.Models;
using PriceAnalysis.DAL.Models.Support;
using PriceAnalysis.DAL.Repository.FileStorage;
using PriceAnalysis.DAL.Repository.Support;
using Radzen;
using FileInfo = System.IO.FileInfo;

namespace PriceAnalysis.BLL.Services.Support;

public class LoadDataFileService : ILoadDataFileService
{
    private readonly IAlertService _alertService;
    private readonly IFileStorageRepository _fileStorageRepository;
    public IConfiguration _configuration { get; }
    public readonly IDocFilesRepository _docFilesRepository;
    private readonly IMapper _mapper;

    public LoadDataFileService(
          IAlertService alertService
        , IFileStorageRepository fileStorageRepository
        , IConfiguration configuration
        , IDocFilesRepository docFilesRepository
        , IMapper mapper
        )
    {
        _alertService = alertService;
        _fileStorageRepository = fileStorageRepository;
        _configuration = configuration;
        _docFilesRepository = docFilesRepository;
        _mapper = mapper;
    }

    #region Specification

    public NotificationMessage SaveFileForLoadSpecification(IFileListEntry file)
    {
        try
        {
            DocFilesEntity entityFile = new DocFilesEntity();
            entityFile.GUID = Guid.NewGuid();
            entityFile.StreamData = file.Data;

            _fileStorageRepository.SaveFileInTempDir(entityFile);

            return _alertService.GetSuccess(entityFile.GUID.ToString());
        }
        catch (Exception)
        {
            return _alertService.GetError("");
        }
    }

    #endregion

    private string GetFileType(string fileType)
    {
        switch (fileType)
        {
            case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet": return "xlsx";
            case "text/xml": return "xml";
            default: return "";
        }
    }

    public string DownloadFileRequestKP(List<SpecificationDto> specifications, int supplierId)
    {
        try
        {
            string pathForTemplateExcel = @"wwwroot/TemplatesExcel/TemplateRequestKP" + @".xlsx";

            FileInfo template = new FileInfo(pathForTemplateExcel);

            using (ExcelPackage eP = new ExcelPackage(template, true))
            {
                eP.Workbook.Properties.Title = "Запрос КП";
                ExcelWorksheet sheet = eP.Workbook.Worksheets[0];

                sheet.Cells[1, 1].Value = "Запрос Коммерческого предложения " + "|" 
                    + " Дата формирования файла: " + DateTime.Now + " | Автор: " + "Иванов И.";
                sheet.Cells[2, 1].Value = "Поставщик " + supplierId;

                int row = 5;
                int str = 0;

                foreach (var item in specifications)
                {
                    if (!item.IsSelectedItem)
                    {
                        continue;
                    }
                    str++;
                    sheet.Cells[row, 1].Value = str;

                    sheet.Cells[row, 2].Value = item.Name;
                    sheet.Cells[row, 3].Value = item.Type;
                    sheet.Cells[row, 4].Value = item.Code;
                    sheet.Cells[row, 5].Value = item.Manufacturer;
                    sheet.Cells[row, 6].Value = item.UnitOfMeasurement.Name;
                    sheet.Cells[row, 7].Value = item.Count;

                    row++;
                }

                var pathReport = @"wwwroot/temp/" + "requestKP" + "-" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + @".xlsx";

                var bin = eP.GetAsByteArray();
                System.IO.File.WriteAllBytes(pathReport, bin);

                return @"temp/" + "requestKP" + "-" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + @".xlsx";
            }

        }
        catch (Exception)
        {
            //TODO Добавить запись об ошибке в лог
            return null;
        }
    }
}
