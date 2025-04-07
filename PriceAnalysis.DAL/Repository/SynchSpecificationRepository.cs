using Microsoft.EntityFrameworkCore;
using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public class SynchSpecificationRepository : ISynchSpecificationRepository
{
    private readonly DBContext _context;

    public SynchSpecificationRepository(DBContext context)
    {
        _context = context;
    }

    public SynchSpecificationEntity GetById(int Id)
    {
        return _context.synchSpecificationEntity.Find(Id);
    }

    public List<SynchSpecificationEntity> GetList()
    {
        return _context.synchSpecificationEntity.Include(x => x.UnitOfMeasurement).ToList();
    }

    public void CreateItem(SynchSpecificationEntity entity)
    {
        _context.synchSpecificationEntity.Add(entity);
        _context.SaveChanges();
    }

    public void CreateList(List<SynchSpecificationEntity> list)
    {
        foreach (var item in list) 
        {
            _context.synchSpecificationEntity.Add(item);
            _context.SaveChanges();
        }
    }

    public void UpdateItem(SynchSpecificationEntity newEntity)
    {
        var entity = _context.synchSpecificationEntity.Find(newEntity.Id);

        entity.SectionId = newEntity.SectionId;
        entity.CodeProduct = newEntity.CodeProduct;
        entity.TypeProduct = newEntity.TypeProduct;
        entity.Name = newEntity.Name;
        entity.Manufacturer = newEntity.Manufacturer;
        entity.UnitOfMeasurementId = newEntity.UnitOfMeasurementId;

        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void DeleteItem(int Id)
    {
        _context.Remove(_context.synchSpecificationEntity.Find(Id));
        _context.SaveChanges();
    }

    public void DeleteList(List<SynchSpecificationEntity> list)
    {
        foreach (var item in list)
        {
            _context.Remove(_context.synchSpecificationEntity.Find(item.Id));
            _context.SaveChanges();
        }
    }
}
