using PriceAnalysis.DAL.Models.Support;

namespace PriceAnalysis.DAL.Repository.Support;

public interface IDocFilesRepository
{
    DocFilesEntity GetItem(int id);
    List<DocFilesEntity> GetList();
    int CreateItem(DocFilesEntity model);
    bool DeleteItem(int id);
}
