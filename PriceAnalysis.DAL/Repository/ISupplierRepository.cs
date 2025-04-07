using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public interface ISupplierRepository
{
    List<SupplierEntity> GetList();
    SupplierEntity GetById(int Id);
    void CreateItem(SupplierEntity entity);
    void UpdateItem(SupplierEntity entity);
    void DeleteItem(int Id);
}
