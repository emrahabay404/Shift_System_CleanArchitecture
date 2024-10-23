using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shift_System.Application.Extensions;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Shifts.Queries
{
    public record GetShiftsDinamik : IRequest<PaginatedResult<GetAllShiftsDto>>
    {
        public DynamicQuery Query { get; }

        public GetShiftsDinamik(DynamicQuery query)
        {
            Query = query;
        }
    }

    internal class GetShiftsDinamikHandler : IRequestHandler<GetShiftsDinamik, PaginatedResult<GetAllShiftsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetShiftsDinamikHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllShiftsDto>> Handle(GetShiftsDinamik request, CancellationToken cancellationToken)
        {
            var shiftsQuery = _unitOfWork.Repository<ShiftList>().Entities.AsQueryable();

            // Generic filtreleme helper sınıfını kullanarak filtre uygula
            if (request.Query.Filter != null)
            {
                shiftsQuery = QueryFilterHelper.ApplyFilter(shiftsQuery, request.Query.Filter);
            }

            // Sıralama uygulama
            if (request.Query.Sort != null && request.Query.Sort.Any())
            {
                foreach (var sort in request.Query.Sort)
                {
                    if (sort.Dir.ToLower() == "desc")
                    {
                        shiftsQuery = shiftsQuery.OrderByDescending(e => EF.Property<object>(e, sort.Field));
                    }
                    else
                    {
                        shiftsQuery = shiftsQuery.OrderBy(e => EF.Property<object>(e, sort.Field));
                    }
                }
            }

            var paginatedList = await shiftsQuery
                .ProjectTo<GetAllShiftsDto>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.Query.Page, request.Query.PageSize, cancellationToken);

            return PaginatedResult<GetAllShiftsDto>.SuccessAsync(
                paginatedList.Data,
                paginatedList.TotalCount,
                paginatedList.CurrentPage,
                paginatedList.PageSize,
                Messages.Teams_Listed_TR
            );
        }

    }
}