using Microsoft.EntityFrameworkCore;
using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public class SynchSpecificationAndKPPrepRepository : ISynchSpecificationAndKPPrepRepository
{
    private readonly DBContext _context;

    public SynchSpecificationAndKPPrepRepository(DBContext context)
    {
        _context = context;
    }

    public SynchSpecificationAndKPPrepEntity GetById(int Id)
    {
        return _context.synchSpecificationAndKPPrepEntity.Find(Id);
    }

    public List<SynchSpecificationAndKPPrepEntity> GetList()
    {
        return _context.synchSpecificationAndKPPrepEntity.ToList();
    }

    public void CreateItem(SynchSpecificationAndKPPrepEntity entity)
    {
        _context.synchSpecificationAndKPPrepEntity.Add(entity);
        _context.SaveChanges();
    }

    public void UpdateItem(SynchSpecificationAndKPPrepEntity newEntity)
    {
        var entity = _context.synchSpecificationAndKPPrepEntity.Find(newEntity.Id);

        entity.SectionId = newEntity.SectionId;
        entity.IsSynch = newEntity.IsSynch;

        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void DeleteItem(int Id)
    {
        _context.Remove(_context.synchSpecificationAndKPPrepEntity.Find(Id));
        _context.SaveChanges();
    }
}
