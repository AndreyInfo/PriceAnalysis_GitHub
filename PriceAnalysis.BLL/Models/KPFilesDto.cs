using BlazorInputFile;
using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.BLL.Models;

public class KPFilesDto
{
    public int Id { get; set; }
    public string? Notice { get; set; }
    public int SupplierId { get; set; }
    public int SectionId { get; set; }
    public int? DocFilesIdExcel { get; set; }
    public int? DocFilesIdPdf { get; set; }

    public IFileListEntry File { get; set; }

    public SupplierEntity Supplier { get; set; }
}
