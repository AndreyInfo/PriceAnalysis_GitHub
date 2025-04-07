using PriceAnalysis.DAL.Models.Reference;

namespace PriceAnalysis.DAL.Repository;

public interface IReferencesRepository
{
    List<UnitOfMeasurementEntity> GetUnitOfMeasurementList();
    UnitOfMeasurementEntity GetUnitOfMeasurementById(int id);
}
