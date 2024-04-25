using Shift_System.Application.Common.Mappings;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Teams.Queries
{
    public class GetAllTeamsDto : IMapFrom<Team>
    {
        public int Id { get; init; }
        public string TeamName { get; set; }

    }
}
