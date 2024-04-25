using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;
using Shift_System.Shared;

namespace Shift_System.Application.Features.Shifts.Queries
{
    public record GetAllShiftsQuery : IRequest<Result<List<GetAllShiftsDto>>>;

    internal class GetAllShiftsQueryHandler : IRequestHandler<GetAllShiftsQuery, Result<List<GetAllShiftsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllShiftsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllShiftsDto>>> Handle(GetAllShiftsQuery query, CancellationToken cancellationToken)
        {
            var _shifts = await _unitOfWork.Repository<ShiftList>().Entities
                   .ProjectTo<GetAllShiftsDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

            return await Result<List<GetAllShiftsDto>>.SuccessAsync(_shifts);
        }

    }
}


