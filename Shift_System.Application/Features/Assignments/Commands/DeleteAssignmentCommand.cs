using AutoMapper;
using MediatR;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Features.Assignments.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Assignments.Commands
{
    public record DeleteAssignmentCommand : IRequest<Result<int>>, IMapFrom<AssignList>
    {
        public int Id { get; set; }
        public DeleteAssignmentCommand()
        {
        }
        public DeleteAssignmentCommand(int id)
        {
            Id = id;
        }
    }

    internal class DeleteAssignmentCommandHandler : IRequestHandler<DeleteAssignmentCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteAssignmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<Result<int>> Handle(DeleteAssignmentCommand command, CancellationToken cancellationToken)
        {
            var _AssignList = await _unitOfWork.Repository<AssignList>().GetByIdAsync(command.Id);
            if (_AssignList != null)
            {
                await _unitOfWork.Repository<AssignList>().DeleteAsync(_AssignList);
                _AssignList.AddDomainEvent(new AssignListDeletedEvent(_AssignList));

                await _unitOfWork.Save(cancellationToken);

                return await Result<int>.SuccessAsync(_AssignList.Id, "AssignList_Deleted");
            }
            else
            {
                return await Result<int>.FailureAsync("AssignList_Not_Found");
            }
        }
    }
}
