using System;
using System.Collections.Generic;

namespace Application.MediatR.Commands.FindOrCreateUser
{
    public class FindOrCreateUserCommandResult
    {
        public Guid UserId { get; set; }
        public IEnumerable<ActiveGroup> Groups { get; set; }
    }

    public class ActiveGroup
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
