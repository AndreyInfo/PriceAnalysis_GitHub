using PriceAnalysis.DAL.Models.Support;

namespace PriceAnalysis.DAL.Repository.Support;

public interface IAlertRepository
{
    AlertEntity GetById(int Id);
}
