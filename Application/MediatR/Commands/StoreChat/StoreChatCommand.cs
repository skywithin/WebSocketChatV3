using MediatR;
using System;

namespace Application.MediatR.Commands.StoreChat
{
    public class StoreChatCommand : IRequest
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid GroupId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreatedUtc { get; set; }
    }
}
