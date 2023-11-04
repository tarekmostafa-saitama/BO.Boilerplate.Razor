using Shared.ServiceContracts;

namespace Application.Common.FileProviders;

public interface IFileUploaderService : IScopedService
{
    Task<string> UploadAsync(Stream data, string fileName, string folderName = "uploads", bool compress = false);
    void ClearDirectory(string folderName = "uploads");
    void Delete(string fileName, string folderName = "uploads");
    bool Exists(string fileName, string folderName = "uploads");
}