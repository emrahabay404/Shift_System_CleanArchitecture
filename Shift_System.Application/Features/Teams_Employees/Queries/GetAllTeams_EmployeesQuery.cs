using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shift_System.Application.Features.Teams.Queries;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Teams_Employees.Queries
{

   public record GetAllTeams_EmployeesQuery : IRequest<Result<List<GetAllTeams_EmployeesDto>>>;

   internal class GetAllTeams_EmployeesQueryHandler : IRequestHandler<GetAllTeams_EmployeesQuery, Result<List<GetAllTeams_EmployeesDto>>>
   {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IMapper _mapper;

      public GetAllTeams_EmployeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
      {
         _unitOfWork = unitOfWork;
         _mapper = mapper;
      }

      public async Task<Result<List<GetAllTeams_EmployeesDto>>> Handle(GetAllTeams_EmployeesQuery query, CancellationToken cancellationToken)
      {
         var _teamsEmp = await _unitOfWork.Repository<TeamEmployee>().Entities
                .ProjectTo<GetAllTeams_EmployeesDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

         return await Result<List<GetAllTeams_EmployeesDto>>.SuccessAsync(_teamsEmp, "Team_Employee_Listed");

      }
   }
}