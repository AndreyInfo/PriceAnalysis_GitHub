using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public class TestRepository : ITestRepository
{
    private readonly DBContext _context;

    public TestRepository(DBContext context)
    {
        _context = context;
    }

    public List<TestEntity> GetList()
    {
        return _context.testEntity.ToList();
    }
}
