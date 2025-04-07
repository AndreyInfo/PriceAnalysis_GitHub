using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public class ChapterForSpecificationRepository : IChapterForSpecificationRepository
{
    private readonly DBContext _context;

    public ChapterForSpecificationRepository(DBContext context)
    {
        _context = context;
    }

    public List<ChapterForSpecificationEntity> GetList()
    {
        return _context.chapterForSpecificationEntity.ToList();
    }

    public void CreateItem(ChapterForSpecificationEntity entity)
    {
        _context.chapterForSpecificationEntity.Add(entity);
        _context.SaveChanges();
    }

    public void CreateList(List<ChapterForSpecificationEntity> list)
    {
        foreach (var item in list)
        {
            _context.chapterForSpecificationEntity.Add(item);
            _context.SaveChanges();
        }
    }

    public void DeleteItem(int Id)
    {
        _context.Remove(_context.chapterForSpecificationEntity.Find(Id));
        _context.SaveChanges();
    }

    public void DeleteList(List<ChapterForSpecificationEntity> list)
    {
        foreach(var item in list)
        {
            _context.Remove(item);
            _context.SaveChanges();
        }
    }
}
