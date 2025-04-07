using PriceAnalysis.DAL.Models.Support;

namespace PriceAnalysis.DAL.Repository.FileStorage;

public interface IFileStorageRepository
{
    Task SaveFileInTempDir(DocFilesEntity entity);
    Task<int> SaveFileInDocFiles(DocFilesEntity entity);
    void DeleteFileInDocFiles(DocFilesEntity entity);
    void CopyDocFileInTempDir(DocFilesEntity entity);
    string CopyDocFilesInTempDirAndCreateZip(List<int> docFileIdList);
}
