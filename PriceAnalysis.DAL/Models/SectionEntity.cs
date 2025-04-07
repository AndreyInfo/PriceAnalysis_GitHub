using System.ComponentModel.DataAnnotations.Schema;

namespace PriceAnalysis.DAL.Models;

[Table("Sections")]
public class SectionEntity
{
    public int Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; }

    public int? SpecificationDocFileId { get; set; }
    public int? PriceAnalysisDocFileId { get; set; }
    public string? KPDocFilesId { get; set; }

    //[ForeignKey("ProjectId")]
    //public virtual ProjectEntity Project { get; set; }
}