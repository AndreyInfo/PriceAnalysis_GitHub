using Newtonsoft.Json;
using PriceAnalysis.DAL.Models.Support;

namespace PriceAnalysis.DAL.Repository.Support;

public class AlertRepository : IAlertRepository
{
    public AlertEntity GetById(int Id)
    {
        var item = JsonConvert.DeserializeObject<List<AlertEntity>>(File.ReadAllText("wwwroot/alerts.json")).Where(x => x.Id == Id).FirstOrDefault();

        return item;
    }
}
