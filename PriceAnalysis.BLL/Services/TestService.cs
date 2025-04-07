using PriceAnalysis.DAL.Models;
using PriceAnalysis.BLL.Transfers;
using PriceAnalysis.BLL.Models;
using PriceAnalysis.DAL.Repository;
using AutoMapper;

namespace PriceAnalysis.BLL.Services;

public class TestService : ITestService
{
    private readonly ITestRepository _testRepository;
    private readonly IMapper _mapper;

    public TestService(
          ITestRepository testRepository
        , IMapper mapper)
    {
        _testRepository = testRepository;
        _mapper = mapper;
    }

    public List<TestDto> GetList()
    {
        var list = _testRepository.GetList();

        //list = _mapper.Map<TestDto>(list);

        //return list.TransferToListDto();
        return _mapper.Map<List<TestDto>>(list);
    }
}
