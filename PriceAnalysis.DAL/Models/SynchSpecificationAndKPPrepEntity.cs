using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceAnalysis.DAL.Models;

[Table("SynchSpecificationAndKPPrep")]
public class SynchSpecificationAndKPPrepEntity
{
    [Key]
    public int Id { get; set; }

    public int SectionId { get; set; }
    public bool IsSynch { get; set; }

    [ForeignKey("SectionId")]
    public virtual SectionEntity Section { get; set; }
}