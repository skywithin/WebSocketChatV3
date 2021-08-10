using System;

namespace ChatClient
{
    public class ClientContext
    {
        public string UserName { get; set; }

        public Guid UserId { get; set; }

        public string CurrentGroupName { get; set; }

        public Guid? CurrentGroupId { get; set; }

        public bool IsInGroup => !string.IsNullOrEmpty(CurrentGroupName);
    }
}
