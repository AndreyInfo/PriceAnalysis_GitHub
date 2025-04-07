namespace PriceAnalysis.BLL.Models;

public class SynchSpecificationAndKPResultDto
{
    public int SynchSpecificationId { get; set; }
    public int SectionId { get; set; }
    public string CodeProduct { get; set; }
    public string TypeProduct { get; set; }
    public string Name { get; set; }
    public string Manufacturer { get; set; }
    public int UnitOfMeasurementId { get; set; }
    public string UnitOfMeasurementName { get; set; }

    public List<SynchPriceDto> SynchPriceList { get; set; }
}

public class SynchPriceDto
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public string SupplierName { get; set; }
    public float? Price { get; set; }
}
