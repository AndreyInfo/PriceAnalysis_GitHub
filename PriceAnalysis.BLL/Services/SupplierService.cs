using AutoMapper;
using PriceAnalysis.BLL.Models;
using PriceAnalysis.BLL.Services.Support;
using PriceAnalysis.DAL.Models;
using PriceAnalysis.DAL.Repository;
using Radzen;

namespace PriceAnalysis.BLL.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;
    private readonly IAlertService _alertService;

    public SupplierService(
          ISupplierRepository supplierRepository
        , IMapper mapper
        , IAlertService alertService
        )
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
        _alertService = alertService;
    }

    public SupplierDto GetById(int Id)
    {
        var entity = _supplierRepository.GetById(Id);

        return _mapper.Map<SupplierDto>(entity);
    }

    public List<SupplierDto> GetList()
    {
        var list = _supplierRepository.GetList();

        return _mapper.Map<List<SupplierDto>>(list);
    }

    public NotificationMessage CreateItem(SupplierDto model)
    {
        var check = CheckSupplierBeforeCreate(model);
        if (check.Severity != NotificationSeverity.Success)
        {
            return check;
        }

        try
        {
            _supplierRepository.CreateItem(_mapper.Map<SupplierEntity>(model));

            return _alertService.Get(3);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public NotificationMessage DeleteItem(int Id)
    {
        try
        {
            _supplierRepository.DeleteItem(Id);

            return _alertService.Get(4);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    public NotificationMessage UpdateItem(SupplierDto model)
    {
        try
        {
            _supplierRepository.UpdateItem(_mapper.Map<SupplierEntity>(model));
            return _alertService.Get(2);
        }
        catch (Exception e)
        {
            return _alertService.GetError(e.Message);
        }
    }

    private NotificationMessage CheckSupplierBeforeCreate(SupplierDto model)
    {
        var sappliers = _supplierRepository.GetList();

        if(sappliers.FirstOrDefault(x => x.Name.Equals(model.Name)) is not null)
        {
            return _alertService.GetError("Поставщик с таким наименованием уже существует!");
        }

        if (sappliers.FirstOrDefault(x => x.Inn.Equals(model.Inn)) is not null)
        {
            return _alertService.GetError("Поставщик с таким ИНН уже существует!");
        }

        if (sappliers.FirstOrDefault(x => x.Kpp.Equals(model.Kpp)) is not null)
        {
            return _alertService.GetError("Поставщик с таким КПП уже существует!");
        }

        return _alertService.GetSuccess();
    }

    public List<SupplierDto> GetListSearchSupplier(string val)
    {
        var list = GetList();
        return list.Where(x => x.Name.ToLower().Contains(val.ToLower())).ToList();
    }
}
