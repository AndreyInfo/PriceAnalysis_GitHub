
namespace PriceAnalysis.BLL.Models;

public class DocFilesDto
{
    public int Id { get; set; }
    public string FileName { get; set; }

    public Guid GUID { get; set; }

    public DateTime CreateDate { get; set; }

    public int PermissiveDocId { get; set; }

    public int CreatorId { get; set; }

    public string Description { get; set; }

    public Stream StreamData { get; set; }
}
