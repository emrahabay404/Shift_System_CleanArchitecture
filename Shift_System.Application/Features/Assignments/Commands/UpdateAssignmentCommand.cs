using AutoMapper;
using MediatR;
using Shift_System.Application.Features.Assignments.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Assignments.Commands
{
    public record UpdateAssignmentCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public Guid? ShiftId { get; set; }
        public Guid? TeamId { get; set; }
    }

    internal class UpdateAssignmentCommandHandler : IRequestHandler<UpdateAssignmentCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAssignmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(UpdateAssignmentCommand command, CancellationToken cancellationToken)
        {

            var _assign = await _unitOfWork.Repository<AssignList>().GetByIdAsync(command.Id);

            if (_assign != null)
            {
                _assign.ShiftId = command.ShiftId;
                _assign.TeamId = command.TeamId;
                _assign.UpdatedDate = DateTime.Now;

                await _unitOfWork.Repository<AssignList>().UpdateAsync(_assign);
                _assign.AddDomainEvent(new AssignUpdatedEvent(_assign));

                await _unitOfWork.Save(cancellationToken);

                return await Result<Guid>.SuccessAsync(_assign.Id, "Assign_Updated");
            }
            else
            {
                return await Result<Guid>.FailureAsync("Assign_Not_Found");
            }
        }
    }
}
