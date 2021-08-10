using System;
using System.Collections.Generic;

namespace Core.Messaging.Payloads
{
    //TODO: Proper implementation of authentication
    public class LoginResultMessage : IMessagePayload
    {
        public Guid UserId { get; set; }
        public IEnumerable<ActiveGroup> Groups { get; set; }
        public bool IsLoginSuccees { get; set; }
        public string ErrorMessage { get; set; }

        public LoginResultMessage() { } //Must have default constructor


        public class ActiveGroup
        {
            public Guid GroupId { get; set; }
            public string GroupName { get; set; }
        }
    }
}
