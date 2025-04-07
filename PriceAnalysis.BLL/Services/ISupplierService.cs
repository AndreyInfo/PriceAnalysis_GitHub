using PriceAnalysis.BLL.Models;
using Radzen;

namespace PriceAnalysis.BLL.Services;

public interface ISupplierService
{
    SupplierDto GetById(int Id);
    List<SupplierDto> GetList();
    NotificationMessage CreateItem(SupplierDto model);
    NotificationMessage UpdateItem(SupplierDto model);
    NotificationMessage DeleteItem(int Id);
    List<SupplierDto> GetListSearchSupplier(string val);
}
