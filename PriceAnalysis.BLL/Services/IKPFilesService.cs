using BlazorInputFile;
using PriceAnalysis.BLL.Models;
using PriceAnalysis.DAL.Models;
using Radzen;

namespace PriceAnalysis.BLL.Services;

public interface IKPFilesService
{
    KPFilesDto GetById(int Id);
    List<KPFilesDto> GetList();
    NotificationMessage CreateItem(KPFilesDto model);
    NotificationMessage UpdateItem(KPFilesDto model);
    NotificationMessage DeleteItem(int Id);
    Task<NotificationMessage> UploadKPFile(KPFilesDto model);
    NotificationMessage DeleteFileKP(int kpFileId);
    (NotificationMessage, string) DownloadFilesKP(int kpFileId);
}
