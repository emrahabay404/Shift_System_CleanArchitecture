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

            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folderPath, uniqueFileName);

            Directory.CreateDirectory(folderPath); // Klasör yoksa oluştur

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        public bool DeleteFile(string folderPath, string fileName)
        {
            var filePath = Path.Combine(folderPath, fileName);
            Console.WriteLine($"Silinmek istenen dosya (servis): {filePath}");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Console.WriteLine($"Dosya başarıyla silindi: {filePath}");
                return true;
            }

            Console.WriteLine("Dosya bulunamadı.");
            return false;
        }
    }
}