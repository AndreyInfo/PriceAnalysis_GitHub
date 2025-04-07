using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public interface IKPFilesRepository
{
    List<KPFilesEntity> GetList();
    KPFilesEntity GetById(int Id);
    void CreateItem(KPFilesEntity entity);
    void UpdateItem(KPFilesEntity entity);
    void DeleteItem(int Id);
}
