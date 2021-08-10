using Application.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MediatR.Commands.RemoveUserFromGroup
{
    class RemoveUserFromGroupCommandHandler : IRequestHandler<RemoveUserFromGroupCommand, RemoveUserFromGroupCommandResult>
    {
        private readonly GameDbContext _db;

        public RemoveUserFromGroupCommandHandler(GameDbContext db)
        {
            _db = db;
        }

        public async Task<RemoveUserFromGroupCommandResult> Handle(RemoveUserFromGroupCommand request, CancellationToken cancellationToken)
        {
            var group = 
                await _db.Groups
                    .Include(x => x.Members)
                    .SingleAsync(x => x.Id == request.GroupId);

            var user = group.Members.SingleOrDefault(x => x.Id == request.UserId);

            group.Members.Remove(user);
            _db.SaveChanges();

            return new RemoveUserFromGroupCommandResult
            {
                GroupName = group.Name,
                UserName = user.Name,
            };
        }
    }
}
