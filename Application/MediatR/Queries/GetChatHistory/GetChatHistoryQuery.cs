using MediatR;
using System;

namespace Application.MediatR.Queries.GetChatHistory
{
    public class GetChatHistoryQuery : IRequest<GetChatHistoryQueryResult>
    {
        public Guid GroupId { get; }

        public GetChatHistoryQuery(Guid groupId)
        {
            GroupId = groupId;
        }
    }
}
