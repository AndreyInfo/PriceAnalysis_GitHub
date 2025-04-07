using PriceAnalysis.DAL.Models.Reference;

namespace PriceAnalysis.DAL.Repository;

public class ReferencesRepository : IReferencesRepository
{
    private readonly DBContext _context;

    public ReferencesRepository(DBContext context)
    {
        _context = context;
    }

    public UnitOfMeasurementEntity GetUnitOfMeasurementById(int id)
    {
        return _context.unitOfMeasurementEntity.Find(id);
    }

    public List<UnitOfMeasurementEntity> GetUnitOfMeasurementList()
    {
        return _context.unitOfMeasurementEntity.ToList();
    }
}
