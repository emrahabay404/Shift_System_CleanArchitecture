using Microsoft.AspNetCore.Http;
using Shift_System.Application.Interfaces;

namespace Shift_System.Infrastructure.Services
{
    public class FileUploadService : IFileUploadService
    {
        public async Task<string> UploadFileAsync(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0)
                return null;

            // Dosya adını benzersiz yapmak için
            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(folderPath, uniqueFileName);

            // Klasör yoksa oluştur
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Dosyayı kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath; // Kaydedilen dosya yolunu döndür
        }

        public bool DeleteFile(string folderPath, string fileName)
        {
            var filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }
    }
}