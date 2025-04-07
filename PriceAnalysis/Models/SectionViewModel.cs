namespace PriceAnalysis.Models;

public class SectionViewModel
{
    public int Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; }
    public string CreateListCiphersClass { get; set; }

    public int? SpecificationDocFileId { get; set; }
    public int? PriceAnalysisDocFileId { get; set; }
    public string? KPDocFilesId { get; set; }
    public int? CountKPDocFiles { get; set; }
}
