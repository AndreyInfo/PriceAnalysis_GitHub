using PriceAnalysis.DAL.Models.Support;
using Microsoft.Extensions.Configuration;
using PriceAnalysis.DAL.Repository.Support;
using System.Net;
using System.IO.Compression;

namespace PriceAnalysis.DAL.Repository.FileStorage;

public class FileStorageRepository : IFileStorageRepository
{
    public IConfiguration _configuration { get; }
    public readonly IDocFilesRepository _docFilesRepository;

    public FileStorageRepository(
          IConfiguration configuration
        , IDocFilesRepository docFilesRepository
        )
    {
        _configuration = configuration;
        _docFilesRepository = docFilesRepository;
    }

    public async Task SaveFileInTempDir(DocFilesEntity entity)
    {
        var path = _configuration.GetSection("AppSettings:TempPath").Value + entity.GUID.ToString();

        var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
        await entity.StreamData.CopyToAsync(fileStream);
        fileStream.Close();
    }

    public async Task<int> SaveFileInDocFiles(DocFilesEntity entity)
    {
        entity.GUID = Guid.NewGuid();
        entity.CreateDate = DateTime.UtcNow;
        entity.CreatorId = 1; //TODO Убрать заглушку
        var path = _configuration.GetSection("AppSettings:DocFilesPath").Value + entity.GUID.ToString();

        var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
        await entity.StreamData.CopyToAsync(fileStream);
        fileStream.Close();

        int docFileId = _docFilesRepository.CreateItem(entity);

        return docFileId;
    }

    public void DeleteFileInDocFiles(DocFilesEntity entity)
    {
        string path = _configuration.GetSection("AppSettings:DocFilesPath").Value + entity.GUID.ToString();
        File.Delete(path);

        _docFilesRepository.DeleteItem(entity.Id);
    }

    public void CopyDocFileInTempDir(DocFilesEntity entity)
    {
            string pathTemp = _configuration.GetSection("AppSettings:TempPath").Value;
            string pathSource = _configuration.GetSection("AppSettings:DocFilesPath").Value + entity.GUID.ToString();
            string pathDestionation = pathTemp + "_" + entity.GUID.ToString() + @"\" + entity.FileName;


            string subpath = "_" + entity.GUID.ToString();
            DirectoryInfo dirInfo = new DirectoryInfo(pathTemp);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            dirInfo.CreateSubdirectory(subpath);

            File.Copy(pathSource, pathDestionation, true);
    }

    public string CopyDocFilesInTempDirAndCreateZip(List<int> docFileIdList)
    {
        string pathTemp = _configuration.GetSection("AppSettings:TempPath").Value;
        string subpath = Guid.NewGuid().ToString();
        DirectoryInfo dirInfo = new DirectoryInfo(pathTemp);
        if (!dirInfo.Exists)
        {
            dirInfo.Create();
        }
        dirInfo.CreateSubdirectory(subpath);
        string pathDestionation;
        string pathSource;

        foreach (var item in docFileIdList)
        {
            var docFile = _docFilesRepository.GetItem(item);
            pathDestionation = pathTemp + subpath + @"\" + docFile.FileName;
            pathSource = _configuration.GetSection("AppSettings:DocFilesPath").Value + docFile.GUID.ToString();

            File.Copy(pathSource, pathDestionation, true);
        }

        ZipFile.CreateFromDirectory(pathTemp + subpath, pathTemp + subpath + ".zip");

        return subpath;
    }

}
