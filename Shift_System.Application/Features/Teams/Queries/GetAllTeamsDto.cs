using Shift_System.Application.Common.Mappings;
using Shift_System.Domain.Entities;
using System.Text.Json.Serialization;

namespace Shift_System.Application.Features.Teams.Queries
{
    public class GetAllTeamsDto : IMapFrom<Team>
    {
        public Guid Id { get; init; }
        public string TeamName { get; set; } = string.Empty;
        [JsonIgnore]
        public string FileName { get; set; } = string.Empty;

    }
}
