using Application.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MediatR.Queries.GetChatHistory
{
    public class GetChatHistoryQueryHandler : IRequestHandler<GetChatHistoryQuery, GetChatHistoryQueryResult>
    {
        private readonly GameDbContext _db;

        public GetChatHistoryQueryHandler(GameDbContext db)
        {
            _db = db;
        }

        public async Task<GetChatHistoryQueryResult> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
        {
            var group =
                await _db.Groups
                    .Include(x => x.ChatMessages).ThenInclude(m => m.Author)
                    .SingleAsync(x => x.Id == request.GroupId);

            return new GetChatHistoryQueryResult
            {
                ChatHistory =
                    group.ChatMessages
                        .Select(m =>
                            new ChatRecord
                            (
                                group.Id,
                                m.Author.Id,
                                m.Author.Name,
                                m.Content,
                                m.DateCreatedUtc
                            ))
                        .OrderBy(x => x.DateCreatedUtc)
                        .ToList()
            };
        }
    }
}
