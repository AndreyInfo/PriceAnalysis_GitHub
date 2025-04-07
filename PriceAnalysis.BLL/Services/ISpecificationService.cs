using PriceAnalysis.BLL.Models;
using PriceAnalysis.DAL.Models;
using Radzen;

namespace PriceAnalysis.BLL.Services;

public interface ISpecificationService
{
    SpecificationDto GetById(int id);
    List<SpecificationDto> GetList();
    NotificationMessage CreateItem(SpecificationDto model);
    NotificationMessage DeleteItem(int id);

    //GOTO Удалить!
    //(NotificationMessage, string) GetTableFromFile(int sectionId);

    (NotificationMessage, string) DownloadFileSpecifications(int sectionId);
    (NotificationMessage, List<SpecificationEntity>) CreateDataSpecification(string guid, int SectionId, string fileName);
}