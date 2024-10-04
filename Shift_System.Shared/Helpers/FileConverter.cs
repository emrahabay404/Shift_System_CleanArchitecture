namespace Shift_System.Shared.Helpers
{
    public static class FileConverter
    {
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
            return fileInfo.Length; // Dosyanın byte cinsinden boyutunu döndürür
        }
        public static string GetFileType(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException("Belirtilen dosya bulunamadı.");
            }

            string extension = Path.GetExtension(filePath);

            if (MimeTypes.TryGetValue(extension, out string mimeType))
            {
                return mimeType;
            }

            return "application/octet-stream"; // Bilinmeyen dosya türleri için varsayılan MIME türü
        }
        private static readonly Dictionary<string, string> MimeTypes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            {".txt", "text/plain"},
            {".pdf", "application/pdf"},
            {".doc", "application/msword"},
            {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {".xls", "application/vnd.ms-excel"},
            {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".gif", "image/gif"},
            {".csv", "text/csv"},
            {".zip", "application/zip"},
            {".mp3", "audio/mpeg"},
            {".mp4", "video/mp4"},
            {".avi", "video/x-msvideo"},
            {".json", "application/json"},
            {".xml", "application/xml"},
            {".html", "text/html"},
            {".css", "text/css"},
            {".js", "application/javascript"}
        };
    }
}