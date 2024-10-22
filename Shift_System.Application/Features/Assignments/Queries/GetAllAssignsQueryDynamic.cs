using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shift_System.Application.Extensions;
using Shift_System.Application.Features.Shifts.Queries;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Assignments.Queries
{

    public record GetAllAssignsQueryDynamic(DynamicQuery Query) : IRequest<Result<List<GetAllAssignsDto>>>;

    //internal class GetAllAssignsQueryDynamicHandler : IRequestHandler<GetAllAssignsQueryDynamic, Result<List<GetAllAssignsDto>>>
    internal class GetAllAssignsQueryDynamicHandler : IRequest<PaginatedResult<GetAllShiftsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAssignsQueryDynamicHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //public async Task<Result<List<GetAllAssignsDto>>> Handle(GetAllAssignsQueryDynamic request, CancellationToken cancellationToken)
        //{
        //    var assigns = await _unitOfWork.Repository<AssignList>()
        //        .ApplyDynamicQuery(request.Query)
        //        .ProjectTo<GetAllAssignsDto>(_mapper.ConfigurationProvider)
        //        .ToListAsync(cancellationToken);

        //    return await Result<List<GetAllAssignsDto>>.SuccessAsync(assigns, "AssignList_Listed");
        //}

        public async Task<PaginatedResult<GetAllAssignsDto>> Handle(GetAllAssignsQueryDynamic request, CancellationToken cancellationToken)
        {
            // Dinamik sorguyu çalıştırıyoruz
            var query = _unitOfWork.Repository<AssignList>()
                .ApplyDynamicQuery(request.Query);

            // Sorguyu DTO'ya projeksiyon yapıp, sayfalama işlemi uyguluyoruz
            var paginatedAssigns = await query
                .ProjectTo<GetAllAssignsDto>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.Query.Page, request.Query.PageSize, cancellationToken);

            // PaginatedResult ile dönen sonuçları paketliyoruz
            return PaginatedResult<GetAllAssignsDto>.SuccessAsync(
                paginatedAssigns.Data,
                paginatedAssigns.TotalCount,
                paginatedAssigns.CurrentPage,
                paginatedAssigns.PageSize,
                "AssignList Listed Successfully"
            );
        }






        //public async Task<PaginatedResult<GetAllShiftsDto>> Handle(GetShiftsWithPaginationQuery query, CancellationToken cancellationToken)
        //{
        //    var _shifts = await _unitOfWork.Repository<ShiftList>().Entities
        //.OrderBy(x => x.Shift_Name)
        //.ProjectTo<GetAllShiftsDto>(_mapper.ConfigurationProvider)
        //.ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);

        //    return PaginatedResult<GetAllShiftsDto>.SuccessAsync(
        //        _shifts.Data,
        //        _shifts.TotalCount,
        //        _shifts.CurrentPage,
        //        _shifts.PageSize,
        //        Messages.Teams_Listed_TR
        //    );
        //}

    }
}