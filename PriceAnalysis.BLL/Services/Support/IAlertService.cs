using Radzen;

namespace PriceAnalysis.BLL.Services.Support;

public interface IAlertService
{
    NotificationMessage Get(int Id);
    NotificationMessage GetError(string detail, string summary = "Внутренняя ошибка!");
    NotificationMessage GetSuccess(string detail = "", string summary = "");
}
