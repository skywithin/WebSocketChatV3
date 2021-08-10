using Application.Db;
using Application.Db.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MediatR.Commands.StoreChat
{
    public class StoreChatCommandHandler : IRequestHandler<StoreChatCommand>
    {
        private readonly GameDbContext _db;

        public StoreChatCommandHandler(GameDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(StoreChatCommand request, CancellationToken cancellationToken)
        {
            var user = await _db.Users.SingleAsync(x => x.Id == request.UserId);
            var group = await _db.Groups.SingleAsync(x => x.Id == request.GroupId);

            _db.ChatMessages.Add(
                new ChatMessageEntity
                {
                    Content = request.Content,
                    DateCreatedUtc = request.DateCreatedUtc,

                    Author = user,
                    Group = group,
                });

            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
