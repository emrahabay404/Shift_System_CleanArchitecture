using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Shift_System.Application.Extensions;
using Shift_System.Application.Features.Shifts.Queries;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Features.Assignments.Queries
{
    public record GetAllAssignsQueryDynamic(DynamicQuery Query) : IRequest<PaginatedResult<GetAllAssignsDto>>;

    internal class GetAllAssignsQueryDynamicHandler : IRequestHandler<GetAllAssignsQueryDynamic, PaginatedResult<GetAllAssignsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAssignsQueryDynamicHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginatedResult<GetAllAssignsDto>> Handle(GetAllAssignsQueryDynamic request, CancellationToken cancellationToken)
        {
            var assigns = await _unitOfWork.Repository<AssignList>()
                .ApplyDynamicQuery(request.Query)
            .ProjectTo<GetAllAssignsDto>(_mapper.ConfigurationProvider)
               .ToPaginatedListAsync(request.Query.Page, request.Query.PageSize, cancellationToken);

            //return await Result<List<GetAllAssignsDto>>.SuccessAsync(assigns, "AssignList_Listed");



            return PaginatedResult<GetAllShiftsDto>.SuccessAsync(
                   assigns.Data,
                   assigns.TotalCount,
                   assigns.CurrentPage,
                   assigns.PageSize,
                   Messages.Teams_Listed_TR
               );



        }


    }
}



















//public record GetAllAssignsQueryDynamic(DynamicQuery Query) : IRequest<Result<List<GetAllAssignsDto>>>;
//internal class GetAllAssignsQueryDynamicHandler : IRequestHandler<GetAllAssignsQueryDynamic, Result<List<GetAllAssignsDto>>>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;

//    public GetAllAssignsQueryDynamicHandler(IUnitOfWork unitOfWork, IMapper mapper)
//    {
//        _unitOfWork = unitOfWork;
//        _mapper = mapper;
//    }
//    public async Task<Result<List<GetAllAssignsDto>>> Handle(GetAllAssignsQueryDynamic request, CancellationToken cancellationToken)
//    {
//        var assigns = await _unitOfWork.Repository<AssignList>()
//            .ApplyDynamicQuery(request.Query)
//            .ProjectTo<GetAllAssignsDto>(_mapper.ConfigurationProvider)
//            .ToListAsync(cancellationToken);

//        return await Result<List<GetAllAssignsDto>>.SuccessAsync(assigns, "AssignList_Listed");
//    }


