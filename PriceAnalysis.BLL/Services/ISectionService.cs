using BlazorInputFile;
using PriceAnalysis.BLL.Models;
using Radzen;

namespace PriceAnalysis.BLL.Services;

public interface ISectionService
{
    SectionDto GetById(int Id);
    List<SectionDto> GetList();
    NotificationMessage CreateItem(SectionDto model);
    NotificationMessage UpdateItem(SectionDto model);
    NotificationMessage DeleteItem(int Id);
    Task<NotificationMessage> UploadTablePriceAnalysis(IFileListEntry file, int sectionId);
    Task<NotificationMessage> UploadFilesKP(IFileListEntry[] files, int sectionId);
    NotificationMessage DeleteTablePriceAnalysis(int sectionId);
    NotificationMessage DeleteFilesKP(int sectionId);
    (NotificationMessage, string) DownloadFilePriceAnalysis(int sectionId);
    (NotificationMessage, string) DownloadFilesKP(int sectionId);
    //SynchSpecificationAndKPPrepDto GetSynchroSpecificationAndKPDto(int SectionId);
}
