using System;
using System.Collections.Generic;

namespace Application.MediatR.Queries.GetUser
{
    public class GetUserQueryResult
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }

        public IEnumerable<ActiveGroup> Groups { get; set; }
    }

    public class ActiveGroup
    {
        public Guid GroupId { get; set; }
        public string GropName { get; set; }
    }
}
