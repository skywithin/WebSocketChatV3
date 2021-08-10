using System;
using System.Collections.Generic;

namespace Core.Messaging.Payloads.Server
{
    public class GroupListMessage : IMessagePayload
    {
        public IEnumerable<GroupInfo> Groups { get; set; }

        public GroupListMessage() { } //Must have default constructor

        public class GroupInfo
        {
            public Guid GroupId { get; set; }
            public string GroupName { get; set; }
            public int MemberCount { get; set; }
            public int MaxMemberCount { get; set; }
        }
    }
}
