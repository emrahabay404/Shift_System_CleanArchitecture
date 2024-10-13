using Shift_System.Domain.Common;

namespace Shift_System.Domain.Entities
{
    public class DocumentInfo : BaseAuditableEntity
    {
        public Guid DataId { get; set; }
        public string TableName { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public long FileSize { get; set; }
        // Dosya yolu ya da bulut depolama adresi için alan
        public string? FilePath { get; set; }
        // Dosyanın Base64 formatında saklanması için string alan
        public string? FileBase64Data { get; set; }
    }
}
