using Microsoft.AspNetCore.Http;

namespace Shift_System.Application.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderPath);
        bool DeleteFile(string folderPath, string fileName);
    }
}
