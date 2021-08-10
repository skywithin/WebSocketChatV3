using System;

namespace Application.Services.Models
{
    public class JoinGroupResult
    {
        public bool IsAllowed { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public Guid UserId { get; set; }
        public string DeniedReason { get; set; }

        public JoinGroupResult(
            bool isAllowed,
            Guid groupId,
            string groupName,
            Guid userId,
            string deniedReason = null)
        {
            IsAllowed = isAllowed;
            GroupId = groupId;
            GroupName = groupName;
            UserId = userId;
            DeniedReason = deniedReason;
        }
    }
}
