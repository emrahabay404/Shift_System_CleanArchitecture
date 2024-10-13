using Microsoft.AspNetCore.Http;

namespace Shift_System.Shared.Helpers
{
    public static class FileConverter
    {
        // Dosyayı Base64'e dönüştür
        public static string ConvertFileToBase64(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException("Belirtilen dosya bulunamadı.");
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(fileBytes);
        }

        public static long GetFileSize(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException("Belirtilen dosya bulunamadı.");
            }

            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }

        public static bool IsValidFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            var mimeType = file.ContentType.ToLower();

            return AllowedExtensions.Contains(extension) && AllowedMimeTypes.Contains(mimeType);
        }

        // İzin verilen MIME türleri listesi (Tüm popüler resim ve ofis dosyaları)
        private static readonly HashSet<string> AllowedMimeTypes = new HashSet<string>
        {
            "image/jpeg", // JPEG
            "image/png",  // PNG
            "image/gif",  // GIF
            "image/webp", // WEBP
            "application/pdf", // PDF
            "application/msword", // DOC
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document", // DOCX
            "application/vnd.ms-excel", // XLS
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" // XLSX
        };

        // İzin verilen dosya uzantıları listesi
        private static readonly HashSet<string> AllowedExtensions = new HashSet<string>
        {
            ".jpg", ".jpeg", ".png", ".gif", ".webp", // Resim dosyaları
            ".pdf", ".doc", ".docx", ".xls", ".xlsx" // Ofis dosyaları
        };

        public static string GetFileType(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("Dosya adı boş olamaz.");
            }

            string extension = Path.GetExtension(fileName).ToLower();

            if (MimeTypes.TryGetValue(extension, out string mimeType))
            {
                return mimeType;
            }

            return "application/octet-stream"; // Bilinmeyen dosya türleri için varsayılan MIME türü
        }

        // MIME türlerini içeren statik Dictionary
        private static readonly Dictionary<string, string> MimeTypes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            { ".txt", "text/plain" },
            { ".pdf", "application/pdf" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".xls", "application/vnd.ms-excel" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".png", "image/png" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".gif", "image/gif" },
            { ".webp", "image/webp" }
        };
    }
}
