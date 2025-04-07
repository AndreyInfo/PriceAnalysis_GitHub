using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public interface ISynchSpecificationRepository
{
    List<SynchSpecificationEntity> GetList();
    SynchSpecificationEntity GetById(int Id);
    void CreateItem(SynchSpecificationEntity entity);
    void CreateList(List<SynchSpecificationEntity> list);
    void UpdateItem(SynchSpecificationEntity entity);
    void DeleteItem(int Id);
    void DeleteList(List<SynchSpecificationEntity> list);
}
