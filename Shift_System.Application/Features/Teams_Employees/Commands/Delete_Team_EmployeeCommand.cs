using AutoMapper;
using MediatR;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Features.Teams_Employees.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Teams_Employees.Commands
{
    public record Delete_Team_EmployeeCommand : IRequest<Result<Guid>>, IMapFrom<TeamEmployee>
    {
        public Guid Id { get; set; }
        public Delete_Team_EmployeeCommand()
        {
        }
        public Delete_Team_EmployeeCommand(Guid id)
        {
            Id = id;
        }
    }

    internal class DeleteTeam_EmployeeCommandHandler : IRequestHandler<Delete_Team_EmployeeCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteTeam_EmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<Result<Guid>> Handle(Delete_Team_EmployeeCommand command, CancellationToken cancellationToken)
        {
            var _Team_Employee = await _unitOfWork.Repository<TeamEmployee>().GetByIdAsync(command.Id);
            if (_Team_Employee != null)
            {
                await _unitOfWork.Repository<TeamEmployee>().DeleteAsync(_Team_Employee);
                _Team_Employee.AddDomainEvent(new Team_EmployeeDeletedEvent(_Team_Employee));

                await _unitOfWork.Save(cancellationToken);

                return await Result<Guid>.SuccessAsync(_Team_Employee.Id, "Team_Employee_Deleted");
            }
            else
            {
                return await Result<Guid>.FailureAsync("Team_Employee_Not_Found");
            }
        }
    }
}
