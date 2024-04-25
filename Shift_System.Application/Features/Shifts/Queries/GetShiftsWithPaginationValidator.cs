using FluentValidation;

namespace Shift_System.Application.Features.Shifts.Queries
{
    public class GetShiftsWithPaginationValidator : AbstractValidator<GetShiftsWithPaginationQuery>
    {

        public GetShiftsWithPaginationValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageNumber at least greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageSize at least greater than or equal to 1.");
        }

    }
}
