using MediatR;
using System;

namespace Application.MediatR.Queries.GetUser
{
    public class GetUserQuery : IRequest<GetUserQueryResult>
    {
        public Guid UserId { get; }

        public GetUserQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
