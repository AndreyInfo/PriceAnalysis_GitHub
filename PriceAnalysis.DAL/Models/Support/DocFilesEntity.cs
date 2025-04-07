using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceAnalysis.DAL.Models.Support;

[Table("DocFiles")]
public class DocFilesEntity
{
    [Key]
    public int Id { get; set; }
    public string FileName { get; set; }

    public Guid GUID { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreatorId { get; set; }

    [NotMapped]
    public Stream StreamData { get; set; }

    [NotMapped]
    public string CreateDateStr { get { return this.CreateDate.ToString("dd.MM.yyyy hh:mm:ss"); } set { } }

    [NotMapped]
    public string CreatorName { get { return "Петров А."; } set { } }
}
