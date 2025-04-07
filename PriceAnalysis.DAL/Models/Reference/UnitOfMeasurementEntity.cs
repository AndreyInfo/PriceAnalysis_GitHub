using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceAnalysis.DAL.Models.Reference;

[Table("UnitOfMeasurement")]
public class UnitOfMeasurementEntity
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string? FullName { get; set; }
    public string? Aliases { get; set; }
    public int? Code { get; set; }
    public int? Accordance { get; set; }

    public int GroupUnitOfMeasurementId { get; set; }

    //TODO Пока группы не добавил
    //[ForeignKey("GroupUnitOfMeasurementId")]
    //public virtual GroupUnitOfMeasurementEntity GroupUnitOfMeasurement { get; set; }
}
