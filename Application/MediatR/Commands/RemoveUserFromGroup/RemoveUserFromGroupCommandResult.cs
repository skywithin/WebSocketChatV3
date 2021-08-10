using System;
using System.Collections.Generic;
using System.Text;

namespace Application.MediatR.Commands.RemoveUserFromGroup
{
    public class RemoveUserFromGroupCommandResult
    {
        public bool IsUserRemoved { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; set; }

    }
}
