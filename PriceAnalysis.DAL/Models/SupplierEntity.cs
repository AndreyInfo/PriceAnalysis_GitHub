using System.ComponentModel.DataAnnotations.Schema;

namespace PriceAnalysis.DAL.Models;

[Table("Suppliers")]
public class SupplierEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Inn { get; set; }
    public string Kpp { get; set; }
    public string? WebSite { get; set; }
}