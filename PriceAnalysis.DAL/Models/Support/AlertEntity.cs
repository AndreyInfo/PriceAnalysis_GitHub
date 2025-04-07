
namespace PriceAnalysis.DAL.Models.Support;

public class AlertEntity
{
    public int Id { get; set; }
    public int Severity { get; set; }
    public string Summary { get; set; }
    public string Detail { get; set; }
    public int Duration { get; set; }
}
