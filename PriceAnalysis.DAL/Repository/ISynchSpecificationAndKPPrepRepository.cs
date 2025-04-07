using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public interface ISynchSpecificationAndKPPrepRepository
{
    List<SynchSpecificationAndKPPrepEntity> GetList();
    SynchSpecificationAndKPPrepEntity GetById(int Id);
    void CreateItem(SynchSpecificationAndKPPrepEntity entity);
    void UpdateItem(SynchSpecificationAndKPPrepEntity entity);
    void DeleteItem(int Id);
}
