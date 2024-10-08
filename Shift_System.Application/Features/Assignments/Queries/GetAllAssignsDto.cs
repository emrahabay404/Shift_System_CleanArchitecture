﻿using Shift_System.Application.Common.Mappings;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Assignments.Queries
{
    public class GetAllAssignsDto : IMapFrom<AssignList>
    {
        public int Id { get; init; }

        public Guid? ShiftId { get; set; }
        public Guid? TeamId { get; set; }

    }
}
