using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public interface ISpecificationRepository
{
    List<SpecificationEntity> GetList();
    SpecificationEntity GetById(int id);
    void CreateItem(SpecificationEntity entity);
    void CreateList(List<SpecificationEntity> list);
    void DeleteItem(int id);
    void DeleteListBySectionId(int sectionId);
}
