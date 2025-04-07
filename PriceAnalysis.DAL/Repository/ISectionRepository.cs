using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public interface ISectionRepository
{
    List<SectionEntity> GetList();
    SectionEntity GetById(int Id);
    void CreateItem(SectionEntity entity);
    void UpdateItem(SectionEntity entity);
    void DeleteItem(int Id);
}
