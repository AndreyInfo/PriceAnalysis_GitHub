using Microsoft.EntityFrameworkCore;
using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public class SectionRepository : ISectionRepository
{
    private readonly DBContext _context;

    public SectionRepository(DBContext context)
    {
        _context = context;
    }

    public SectionEntity GetById(int Id)
    {
        return _context.sectionEntity.Find(Id);
    }

    public List<SectionEntity> GetList()
    {
        return _context.sectionEntity.ToList();
    }

    public void CreateItem(SectionEntity entity)
    {
        var t =_context.sectionEntity.Add(entity);
        _context.SaveChanges();
    }

    public void UpdateItem(SectionEntity newEntity)
    {
        var entity = _context.sectionEntity.Find(newEntity.Id);

        entity.Name = newEntity.Name;
        entity.SpecificationDocFileId = newEntity.SpecificationDocFileId;
        entity.PriceAnalysisDocFileId = newEntity.PriceAnalysisDocFileId;
        entity.KPDocFilesId = newEntity.KPDocFilesId;

        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void DeleteItem(int Id)
    {
        _context.Remove(_context.sectionEntity.Find(Id));
        _context.SaveChanges();
    }
}
