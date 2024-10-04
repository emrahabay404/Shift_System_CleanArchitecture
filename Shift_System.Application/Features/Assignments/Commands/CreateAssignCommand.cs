using AutoMapper;
using MediatR;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Features.Assignments.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Assignments.Commands
{

    public record CreateAssignCommand : IRequest<Result<Guid>>, IMapFrom<AssignList>
    {
        public Guid? ShiftId { get; set; }
        public Guid? TeamId { get; set; }
    }

    internal class CreateAssignCommandHandler : IRequestHandler<CreateAssignCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateAssignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreateAssignCommand command, CancellationToken cancellationToken)
        {
            var _assign = new AssignList()
            {
                ShiftId = command.ShiftId,
                TeamId = command.TeamId,
                CreatedDate = DateTime.Now,
            };
            await _unitOfWork.Repository<AssignList>().AddAsync(_assign);
            _assign.AddDomainEvent(new AssignCreatedEvent(_assign));
            await _unitOfWork.Save(cancellationToken);
            return await Result<Guid>.SuccessAsync(_assign.Id, "Assign_Created");
        }

    }
}
