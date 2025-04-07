
namespace PriceAnalysis.BLL.Services.Support;

public class FileStorageService : IFileStorageService
{
    public void ClearTempDir()
    {
        string dirName = @"wwwroot\temp\";

        DirectoryInfo dirInfo = new DirectoryInfo(dirName);
        dirInfo.Delete(true);

        DirectoryInfo dirInfoNew = new DirectoryInfo(dirName);
        if (!dirInfoNew.Exists)
        {
            dirInfoNew.Create();
        }
    }
}
