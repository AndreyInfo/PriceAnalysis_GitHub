using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceAnalysis.DAL.Models;

[Table("SynchPrice")]
public class SynchPriceEntity
{
    [Key]
    public int Id { get; set; }

    public int SynchSpecificationId { get; set; }
    public int SupplierId { get; set; }
    public float? Price { get; set; }

    [ForeignKey("SynchSpecificationId")]
    public virtual SynchSpecificationEntity SynchSpecification { get; set; }

    [ForeignKey("SupplierId")]
    public virtual SupplierEntity Supplier { get; set; }
}