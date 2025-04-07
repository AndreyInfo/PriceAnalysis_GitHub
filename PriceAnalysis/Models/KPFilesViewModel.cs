using BlazorInputFile;

namespace PriceAnalysis.Models;

public class KPFilesViewModel
{
    public int Id { get; set; }
    public string? Notice { get; set; }
    public int SupplierId { get; set; }
    public int SectionId { get; set; }
    public int? DocFilesIdExcel { get; set; }
    public int? DocFilesIdPdf { get; set; }
    
    public IFileListEntry File { get; set; }

    public string? SupplierName { get; set; }
    public int? Number { get; set; }
}
