using PriceAnalysis.DAL.Models.Reference;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceAnalysis.DAL.Models;

[Table("SynchSpecification")]
public class SynchSpecificationEntity
{
    [Key]
    public int Id { get; set; }

    public int SectionId { get; set; }
    public string? CodeProduct { get; set; }
    public string? TypeProduct { get; set; }
    public string Name { get; set; }
    public string Manufacturer { get; set; }
    public int UnitOfMeasurementId { get; set; }

    [ForeignKey("SectionId")]
    public virtual SectionEntity Section { get; set; }

    [ForeignKey("UnitOfMeasurementId")]
    public virtual UnitOfMeasurementEntity UnitOfMeasurement { get; set; }
}