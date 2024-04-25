using AutoMapper;
using MediatR;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Features.Teams_Employees.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared;

namespace Shift_System.Application.Features.Teams_Employees.Commands
{
   public record Delete_Team_EmployeeCommand : IRequest<Result<int>>, IMapFrom<TeamEmployee>
    {
        public int Id { get; set; }
        public Delete_Team_EmployeeCommand()
        {
        }
        public Delete_Team_EmployeeCommand(int id)
        {
            Id = id;
        }
    }

    internal class DeleteTeam_EmployeeCommandHandler : IRequestHandler<Delete_Team_EmployeeCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteTeam_EmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<Result<int>> Handle(Delete_Team_EmployeeCommand command, CancellationToken cancellationToken)
        {
            var _Team_Employee = await _unitOfWork.Repository<TeamEmployee>().GetByIdAsync(command.Id);
            if (_Team_Employee != null)
            {
                await _unitOfWork.Repository<TeamEmployee>().DeleteAsync(_Team_Employee);
                _Team_Employee.AddDomainEvent(new Team_EmployeeDeletedEvent(_Team_Employee));

                await _unitOfWork.Save(cancellationToken);

                return await Result<int>.SuccessAsync(_Team_Employee.Id, "Team Employee Deleted");
            }
            else
            {
                return await Result<int>.FailureAsync("Team Employee Not Found.");
            }
        }
    }
}
