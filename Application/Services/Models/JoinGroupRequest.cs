using System;

namespace Application.Services.Models
{
    public class JoinGroupRequest
    {
        public string GroupName { get; set; }
        public Guid UserId { get; set; }

        public JoinGroupRequest(
            string groupName,
            Guid userId)
        {
            GroupName = groupName;
            UserId = userId;
        }
    }
}
