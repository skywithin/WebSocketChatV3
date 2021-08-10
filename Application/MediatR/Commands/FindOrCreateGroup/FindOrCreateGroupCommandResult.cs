using System;
using System.Collections.Generic;

namespace Application.MediatR.Commands.FindOrCreateGroup
{
    public class FindOrCreateGroupCommandResult
    {
        public string GroupName { get; set; }
        public Guid GroupId { get; set; }
        public IEnumerable<Guid> MemberIds { get; set; }
    }
}
