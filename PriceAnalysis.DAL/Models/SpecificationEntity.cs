using PriceAnalysis.DAL.Models.Reference;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceAnalysis.DAL.Models;

[Table("Specification")]
public class SpecificationEntity
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
    public int SectionId { get; set; }
    public string SID { get; set; }

    public string? Type { get; set; }
    public string? Code { get; set; }
    public string? Manufacturer { get; set; }

    public int UnitOfMeasurementId { get; set; }
    public double Count { get; set; }
    public Guid? ChapterId { get; set; }

    [ForeignKey("SectionId")]
    public virtual SectionEntity Section { get; set; }

    [ForeignKey("UnitOfMeasurementId")]
    public virtual UnitOfMeasurementEntity UnitOfMeasurement { get; set; }

    [ForeignKey("ChapterId")]
    public virtual ChapterForSpecificationEntity? Chapter { get; set; }
}