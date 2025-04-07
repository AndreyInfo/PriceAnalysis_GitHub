using System.ComponentModel.DataAnnotations.Schema;

namespace PriceAnalysis.DAL.Models;

[Table("test")]
public class TestEntity
{
    public int id { get; set; }
    public string name { get; set; }
    public int age { get; set; }
}
