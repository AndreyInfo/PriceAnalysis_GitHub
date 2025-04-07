using Microsoft.EntityFrameworkCore;
using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public class KPFilesRepository : IKPFilesRepository
{
    private readonly DBContext _context;

    public KPFilesRepository(DBContext context)
    {
        _context = context;
    }

    public KPFilesEntity GetById(int Id)
    {
        return _context.kpFilesEntity.Find(Id);
    }

    public List<KPFilesEntity> GetList()
    {
        return _context.kpFilesEntity.ToList();
    }

    public void CreateItem(KPFilesEntity entity)
    {
        _context.kpFilesEntity.Add(entity);
        _context.SaveChanges();
    }

    public void UpdateItem(KPFilesEntity newEntity)
    {
        var entity = _context.kpFilesEntity.Find(newEntity.Id);

        entity.Notice = newEntity.Notice;
        entity.SupplierId = newEntity.SupplierId;
        entity.SectionId = newEntity.SectionId;
        entity.DocFilesIdExcel = newEntity.DocFilesIdExcel;
        entity.DocFilesIdPdf = newEntity.DocFilesIdPdf;

        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void DeleteItem(int Id)
    {
        _context.Remove(_context.kpFilesEntity.Find(Id));
        _context.SaveChanges();
    }
}
