using PriceAnalysis.DAL.Models.Support;

namespace PriceAnalysis.DAL.Repository.Support;

public class DocFilesRepository : IDocFilesRepository
{
    private readonly DBContext _context;

    public DocFilesRepository(DBContext context)
    {
        _context = context;
    }

    public DocFilesEntity GetItem(int id)
    {
        return _context.docFilesEntity.Find(id);
    }

    public List<DocFilesEntity> GetList()
    {
        return _context.docFilesEntity.ToList();
    }

    public int CreateItem(DocFilesEntity model)
    {
        try
        {
            _context.docFilesEntity.Add(model);
            _context.SaveChanges();

            return model.Id;
        }
        catch (System.Exception)
        {
            return -1;
        }
    }

    public bool DeleteItem(int id)
    {
        try
        {
            DocFilesEntity item = _context.docFilesEntity.Find(id);
            _context.Remove(item);
            _context.SaveChanges();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }



    
}
