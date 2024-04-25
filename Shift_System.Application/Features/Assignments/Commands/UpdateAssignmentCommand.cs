using AutoMapper;
using MediatR;
using Shift_System.Application.Features.Assignments.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared;

namespace Shift_System.Application.Features.Assignments.Commands
{
    public record UpdateAssignmentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int? ShiftId { get; set; }
        public int? TeamId { get; set; }
    }

    internal class UpdateAssignmentCommandHandler : IRequestHandler<UpdateAssignmentCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAssignmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdateAssignmentCommand command, CancellationToken cancellationToken)
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

                return await Result<int>.SuccessAsync(_assign.Id, "Assign Updated.");
            }
            else
            {
                return await Result<int>.FailureAsync("Assign Not Found.");
            }
        }
    }
}
