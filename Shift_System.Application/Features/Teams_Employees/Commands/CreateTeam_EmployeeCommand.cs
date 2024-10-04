using AutoMapper;
using MediatR;
using Shift_System.Application.Common.Mappings;
using Shift_System.Application.Features.Teams_Employees.Events;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Teams_Employees.Commands
{
    public record CreateTeam_EmployeeCommand : IRequest<Result<Guid>>, IMapFrom<TeamEmployee>
    {
        public Guid? EmployeeId { get; set; }
        public Guid? TeamId { get; set; }
        public bool? IsLeader { get; set; }
    }

    internal class CreateTeam_EmployeeCommandHandler : IRequestHandler<CreateTeam_EmployeeCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITeam_EmployeeRepository _team_EmployeeRepository;

        public CreateTeam_EmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ITeam_EmployeeRepository team_EmployeeRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _team_EmployeeRepository = team_EmployeeRepository;
        }

        public async Task<Result<Guid>> Handle(CreateTeam_EmployeeCommand command, CancellationToken cancellationToken)
        {
            var one_team_check = _team_EmployeeRepository.One_Team_Check(command);
            if (one_team_check > 0)
            {
                return await Result<Guid>.FailureAsync("Already Has A Team");
            }

            var _teamemp = new TeamEmployee()
            {
                EmployeeId = command.EmployeeId,
                TeamId = command.TeamId,
                IsLeader = false,
                CreatedDate = DateTime.Now,
            };

            await _unitOfWork.Repository<TeamEmployee>().AddAsync(_teamemp);
            _teamemp.AddDomainEvent(new Team_EmployeeCreatedEvent(_teamemp));
            await _unitOfWork.Save(cancellationToken);
            return await Result<Guid>.SuccessAsync(_teamemp.Id, "Team_Employee Created");
        }

    }
}
