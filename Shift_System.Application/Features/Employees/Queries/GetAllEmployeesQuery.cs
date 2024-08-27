using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Employees.Queries
{
    public record GetAllEmployeesQuery : IRequest<Result<List<GetAllEmployeesDto>>>;

    internal class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, Result<List<GetAllEmployeesDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllEmployeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllEmployeesDto>>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
        {
            var employees = await _unitOfWork.Repository<Employee>().Entities
                   .ProjectTo<GetAllEmployeesDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

            return await Result<List<GetAllEmployeesDto>>.SuccessAsync(employees, "Employees_Listed");
      }
    }
}