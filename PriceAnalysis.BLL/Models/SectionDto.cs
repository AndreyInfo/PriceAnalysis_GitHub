
namespace PriceAnalysis.BLL.Models;

public class SectionDto
{
    public int Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; }

    public int? SpecificationDocFileId { get; set; }
    public int? PriceAnalysisDocFileId { get; set; }
    public string? KPDocFilesId { get; set; }
}
