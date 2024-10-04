using Shift_System.Domain.Common;

namespace Shift_System.Domain.Entities
{
    public class MenuSearchBar : BaseAuditableEntity
    {

        public string? MenuName { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public string? Url { get; set; }

    }
}
