using Microsoft.EntityFrameworkCore;
using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public class SynchPriceRepository : ISynchPriceRepository
{
    private readonly DBContext _context;

    public SynchPriceRepository(DBContext context)
    {
        _context = context;
    }

    public SynchPriceEntity GetById(int Id)
    {
        return _context.synchPriceEntity.Find(Id);
    }

    public List<SynchPriceEntity> GetList()
    {
        return _context.synchPriceEntity.Include(x => x.Supplier).ToList();
    }

    public void CreateItem(SynchPriceEntity entity)
    {
        _context.synchPriceEntity.Add(entity);
        _context.SaveChanges();
    }

    public void CreateList(List<SynchPriceEntity> list)
    {
        foreach (var item in list) 
        {
            _context.synchPriceEntity.Add(item);
            _context.SaveChanges();
        }
    }

    public void UpdateItem(SynchPriceEntity newEntity)
    {
        var entity = _context.synchPriceEntity.Find(newEntity.Id);

        entity.SynchSpecificationId = newEntity.SynchSpecificationId;
        entity.SupplierId = newEntity.SupplierId;
        entity.Price = newEntity.Price;

        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void DeleteItem(int Id)
    {
        _context.Remove(_context.synchPriceEntity.Find(Id));
        _context.SaveChanges();
    }


}
