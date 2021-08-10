using MediatR;
using System;

namespace Application.MediatR.Commands.AddUserToGroup
{
    public class AddUserToGroupCommand : IRequest
    {
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
    }
}
