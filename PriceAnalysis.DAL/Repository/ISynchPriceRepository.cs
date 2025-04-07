using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public interface ISynchPriceRepository
{
    List<SynchPriceEntity> GetList();
    SynchPriceEntity GetById(int Id);
    void CreateItem(SynchPriceEntity entity);
    void CreateList(List<SynchPriceEntity> entity);
    void UpdateItem(SynchPriceEntity entity);
    void DeleteItem(int Id);
}
