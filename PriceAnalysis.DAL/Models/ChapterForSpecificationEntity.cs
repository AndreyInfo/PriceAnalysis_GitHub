using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceAnalysis.DAL.Models;

[Table("ChapterForSpecification")]
public class ChapterForSpecificationEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
}
