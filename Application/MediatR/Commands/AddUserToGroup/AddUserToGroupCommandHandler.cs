using Application.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MediatR.Commands.AddUserToGroup
{
    public class AddUserToGroupCommandHandler : IRequestHandler<AddUserToGroupCommand>
    {
        private readonly GameDbContext _db;

        public AddUserToGroupCommandHandler(GameDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(AddUserToGroupCommand request, CancellationToken cancellationToken)
        {
            var user = await _db.Users.SingleAsync(u => u.Id == request.UserId);
                    
            var group =
                await _db.Groups
                    .Include(g => g.Members)
                    .Where(g => g.Id == request.GroupId)
                    .SingleAsync();

            group.Members.Add(user);
            await _db.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
