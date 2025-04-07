
namespace PriceAnalysis.BLL.Models;

public class SynchSpecificationAndKPPrepDto
{
    public int SectionId { get; set; }
    public string SectionName { get; set; }
    public bool ExistSpecification { get; set; }
    public int? CountStringInSpecification { get; set; }
    public bool IsSynch { get; set; }
    public List<ExistFilesForKPDto> ListExistFilesForKP { get; set; }
}

public class ExistFilesForKPDto
{
    public string Name { get; set; }
    public bool ExistPDF { get; set; }
    public bool ExistExcel { get; set; }
}
