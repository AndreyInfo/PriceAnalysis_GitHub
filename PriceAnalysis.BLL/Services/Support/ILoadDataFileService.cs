using BlazorInputFile;
using PriceAnalysis.BLL.Models;
using Radzen;

namespace PriceAnalysis.BLL.Services.Support;

public interface ILoadDataFileService
{
    NotificationMessage SaveFileForLoadSpecification(IFileListEntry file);
    string DownloadFileRequestKP(List<SpecificationDto> specifications, int supplierId);

}
