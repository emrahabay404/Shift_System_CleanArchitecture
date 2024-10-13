using Shift_System.Domain.Common;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Documents.Events
{
    public class DocumentCreatedEvent : BaseEvent
    {
        public DocumentInfo _DocumentInfo { get; }

        public DocumentCreatedEvent(DocumentInfo documentInfo)
        {
            _DocumentInfo = documentInfo;
        }
    }
}
