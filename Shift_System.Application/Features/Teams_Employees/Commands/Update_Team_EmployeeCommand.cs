using AutoMapper;
using MediatR;
using Shift_System.Application.Features.Teams_Employees.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Teams_Employees.Commands
{
    public record Update_Team_EmployeeCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? TeamId { get; set; }
        public bool? IsLeader { get; set; }
    }

    internal class UpdateTeam_EmployeeCommandHandler : IRequestHandler<Update_Team_EmployeeCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITeam_EmployeeRepository _team_EmployeeRepository;

        public UpdateTeam_EmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ITeam_EmployeeRepository team_EmployeeRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _team_EmployeeRepository = team_EmployeeRepository;
        }

        public async Task<Result<Guid>> Handle(Update_Team_EmployeeCommand command, CancellationToken cancellationToken)
        {
            var one_team_check = _team_EmployeeRepository.One_Team_Check_2(command);
            var Leader_Check = _team_EmployeeRepository.Team_Leader_Check_2(command);
            if (Leader_Check > 0)
            {
                return await Result<Guid>.FailureAsync("Team Leader Cannot Be Transferred");
            }
            if (one_team_check > 0)
            {
                return await Result<Guid>.FailureAsync("Already Has A Team");
            }

            var _TeamEmployee = await _unitOfWork.Repository<TeamEmployee>().GetByIdAsync(command.Id);
            if (_TeamEmployee != null)
            {
                _TeamEmployee.EmployeeId = command.EmployeeId;
                _TeamEmployee.TeamId = command.TeamId;
                _TeamEmployee.IsLeader = command.IsLeader;
                await _unitOfWork.Repository<TeamEmployee>().UpdateAsync(_TeamEmployee);
                _TeamEmployee.AddDomainEvent(new TeamEmployeeUpdatedEvent(_TeamEmployee));
                await _unitOfWork.Save(cancellationToken);
                return await Result<Guid>.SuccessAsync(_TeamEmployee.Id, "Team_Employee_Updated");
            }
            else
            {
                return await Result<Guid>.FailureAsync("Team_Employee_Not_Found");
            }
        }
    }
}
