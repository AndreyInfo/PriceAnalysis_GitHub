using Microsoft.EntityFrameworkCore;
using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public class SpecificationRepository : ISpecificationRepository
{
    private readonly DBContext _context;

    public SpecificationRepository(DBContext context)
    {
        _context = context;
    }

    public List<SpecificationEntity> GetList()
    {
        return _context.specificationEntity.Include(x => x.UnitOfMeasurement).ToList();
    }

    public SpecificationEntity GetById(int id)
    {
        return _context.specificationEntity.Find(id);
    }

    public void CreateItem(SpecificationEntity entity)
    {
        _context.specificationEntity.Add(entity);
        _context.SaveChanges();
    }

    public void CreateList(List<SpecificationEntity> list)
    {
            try
            {
                foreach (var item in list)
                {
                    _context.specificationEntity.Add(item);
                    _context.SaveChanges();
                }
            }
            catch (Exception )
            {
                //TODO Добавить запись об ошибке в лог
            }
    }

    public void DeleteItem(int id)
    {
        _context.Remove(_context.specificationEntity.Find(id));
        _context.SaveChanges();
    }

    public void DeleteListBySectionId(int sectionId)
    {
        var list = _context.specificationEntity.Where(x => x.SectionId == sectionId).ToList();

        foreach (var item in list)
        {
            _context.Remove(item);
            _context.SaveChanges();
        }
    }
}
