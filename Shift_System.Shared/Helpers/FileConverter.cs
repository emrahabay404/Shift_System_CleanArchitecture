using Microsoft.AspNetCore.Http;

namespace Shift_System.Shared.Helpers
{
    public static class FileConverter
    {
        private const long MaxFileSize = 15 * 1024 * 1024; // 15 MB

        // Dosyayı Base64'e dönüştür
        public static string ConvertFileToBase64(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                throw new FileNotFoundException("Belirtilen dosya bulunamadı.");

            byte[] fileBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(fileBytes);
        }

        // Dosyanın geçerliliğini kontrol et
        public static bool IsValidFile(IFormFile file)
        {
            return file.Length <= MaxFileSize && HasAllowedExtension(file);
        }

        // Desteklenen dosya uzantıları kontrolü
        private static bool HasAllowedExtension(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            var allowedExtensions = new HashSet<string>
            {
                ".jpg", ".jpeg", ".png", ".pdf", ".docx", ".xlsx", ".gif", ".bmp", ".tiff", ".webp",
                ".txt", ".csv", ".ppt", ".pptx", ".xls", ".mp4", ".mov", ".avi", ".mp3", ".wav", ".ogg"
            };
            return allowedExtensions.Contains(extension);
        }

        // Dosya türünü belirlemek için kullanılır
        public static string GetFileType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            var mimeTypes = new Dictionary<string, string>
            {
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" },
                { ".gif", "image/gif" },
                { ".bmp", "image/bmp" },
                { ".tiff", "image/tiff" },
                { ".webp", "image/webp" },
                { ".pdf", "application/pdf" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { ".txt", "text/plain" },
                { ".csv", "text/csv" },
                { ".ppt", "application/vnd.ms-powerpoint" },
                { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
                { ".xls", "application/vnd.ms-excel" },
                { ".mp4", "video/mp4" },
                { ".mov", "video/quicktime" },
                { ".avi", "video/x-msvideo" },
                { ".mp3", "audio/mpeg" },
                { ".wav", "audio/wav" },
                { ".ogg", "audio/ogg" }
            };

            return mimeTypes.ContainsKey(extension) ? mimeTypes[extension] : "application/octet-stream";
        }
    }
}
