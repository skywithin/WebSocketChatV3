using System;
using System.Collections.Generic;

namespace Application.MediatR.Queries.ListAvailableGroups
{
    public class ListAvailableGroupsQueryResult 
    {
        public IEnumerable<ActiveGroup> ActiveGroups { get; set; }
    }

    public class ActiveGroup
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public int MembersCount { get; set; }

        public ActiveGroup(Guid groupId, string groupName, int membersCount)
        {
            GroupId = groupId;
            GroupName = groupName;
            MembersCount = membersCount;
        }
    }
}
