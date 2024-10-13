using Shift_System.Domain.Common;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Documents.Events
{
    public class DocumentDeletedEvent : BaseEvent
    {
        public DocumentInfo _DocumentInfo { get; }
        public DocumentDeletedEvent(DocumentInfo documentInfo)
        {
            _DocumentInfo = documentInfo;
        }
    }
}