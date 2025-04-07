using PriceAnalysis.DAL.Models.Support;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceAnalysis.DAL.Models;

[Table("KPFiles")]
public class KPFilesEntity
{
    public int Id { get; set; }
    public string? Notice { get; set; }
    public int SupplierId { get; set; }
    public int SectionId { get; set; }
    public int? DocFilesIdExcel { get; set; }
    public int? DocFilesIdPdf { get; set; }

    [ForeignKey("SupplierId")]
    public virtual SupplierEntity Supplier { get; set; }

    [ForeignKey("SectionId")]
    public virtual SectionEntity Section { get; set; }

    [ForeignKey("DocFilesIdExcel")]
    public virtual DocFilesEntity DocFilesExcel { get; set; }

    [ForeignKey("DocFilesIdPdf")]
    public virtual DocFilesEntity DocFilesPdf { get; set; }
}
