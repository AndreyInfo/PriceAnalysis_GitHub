using AutoMapper;
using PriceAnalysis.BLL.Models;
using PriceAnalysis.DAL.Repository.Support;
using Radzen;
using Radzen.Blazor.Rendering;

namespace PriceAnalysis.BLL.Services.Support;

public class AlertService : IAlertService
{
    private readonly IAlertRepository _alertRepository;
    private readonly IMapper _mapper;

    public AlertService(
          IAlertRepository alertRepository
        , IMapper mapper)
    {
        _alertRepository = alertRepository;
        _mapper = mapper;
    }

    public NotificationMessage Get(int Id)
    {
        var item = _mapper.Map<NotificationMessage>(_alertRepository.GetById(Id));
        return item;
    }

    public NotificationMessage GetError(string detail, string summary = "Внутренняя ошибка!")
    {
        NotificationMessage item = new NotificationMessage() { Severity = NotificationSeverity.Error, Duration = 15000, Summary = summary, Detail = detail };
        return item;
    }

    public NotificationMessage GetSuccess(string detail = "", string summary = "Успех!")
    {
        NotificationMessage item = new NotificationMessage() { Severity = NotificationSeverity.Success, Duration = 3000, Summary = summary, Detail = detail };
        return item;
    }
}
