using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.DAL.Repository;

public interface ITestRepository
{
    List<TestEntity> GetList();
}
