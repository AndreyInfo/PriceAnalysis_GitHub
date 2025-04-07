using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public interface IChapterForSpecificationRepository
{
    List<ChapterForSpecificationEntity> GetList();
    void CreateItem(ChapterForSpecificationEntity entity);
    void CreateList(List<ChapterForSpecificationEntity> entity);
    void DeleteItem(int Id);
    void DeleteList(List<ChapterForSpecificationEntity> list);
}
