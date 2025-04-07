using PriceAnalysis.DAL.Models;
using PriceAnalysis.DAL.Models.Reference;

namespace PriceAnalysis.BLL.Models;

public class SpecificationDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SectionId { get; set; }
    public string SID { get; set; }
    public string Type { get; set; }
    public string Code { get; set; }
    public string Manufacturer { get; set; }
    public int UnitOfMeasurementId { get; set; }
    public double Count { get; set; }
    public Guid? ChapterId { get; set; }
    public bool IsSelectedItem { get; set; } = false;

    public SectionEntity Section { get; set; }
    public ChapterForSpecificationEntity? Chapter { get; set; }
    public UnitOfMeasurementEntity UnitOfMeasurement { get; set; }
}