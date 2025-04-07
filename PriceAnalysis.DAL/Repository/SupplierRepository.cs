using Microsoft.EntityFrameworkCore;
using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public class SupplierRepository : ISupplierRepository
{
    private readonly DBContext _context;

    public SupplierRepository(DBContext context)
    {
        _context = context;
    }

    public SupplierEntity GetById(int Id)
    {
        return _context.supplierEntity.Find(Id);
    }

    public List<SupplierEntity> GetList()
    {
        return _context.supplierEntity.ToList();
    }

    public void CreateItem(SupplierEntity entity)
    {
        _context.supplierEntity.Add(entity);
        _context.SaveChanges();
    }

    public void UpdateItem(SupplierEntity newEntity)
    {
        var entity = _context.supplierEntity.Find(newEntity.Id);

        entity.Name = newEntity.Name;
        entity.Inn = newEntity.Inn;
        entity.Kpp = newEntity.Kpp;
        entity.WebSite = newEntity.WebSite;

        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void DeleteItem(int Id)
    {
        _context.Remove(_context.supplierEntity.Find(Id));
        _context.SaveChanges();
    }
}
