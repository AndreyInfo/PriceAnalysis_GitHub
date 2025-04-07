using System.ComponentModel.DataAnnotations;

namespace PriceAnalysis.Models;

public class SupplierViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Обязательное поле")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Строка от 2 до 200 символов")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Обязательное поле")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "10 цифр")]
    public string Inn { get; set; }

    [Required(ErrorMessage = "Обязательное поле")]
    [StringLength(9, MinimumLength = 9, ErrorMessage = "9 цифр")]
    public string Kpp { get; set; }

    public string? WebSite { get; set; }

    public string BeforeName { get; set; }
    public string AfterName { get; set; }
    public string SearchStr { get; set; }
}
