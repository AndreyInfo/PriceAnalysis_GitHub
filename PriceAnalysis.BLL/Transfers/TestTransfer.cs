using PriceAnalysis.BLL.Models;
using PriceAnalysis.DAL.Models;

namespace PriceAnalysis.BLL.Transfers;

public static class TestTransfer
{
    public static TestEntity TransferToEntity(this TestDto dto)
    {
        TestEntity entity = new TestEntity()
        {
            id = dto.Id,
            name = dto.Name,
            age = dto.Age
        };
        return entity;
    }

    public static TestDto TransferToDto(this TestEntity entity)
    {
        TestDto dto = new TestDto()
        {
            Id = entity.id,
            Name = entity.name,
            Age = entity.age
        };

        return dto;
    }

    public static List<TestDto> TransferToListDto(this List<TestEntity> entities)
    {
        List<TestDto> dtos = entities.Select(x => x.TransferToDto()).ToList();
        return dtos;
    }

    public static List<TestEntity> TransferToListEntity(this List<TestDto> dtos)
    {
        List<TestEntity> entities = dtos.Select(x => x.TransferToEntity()).ToList();
        return entities;
    }
}
