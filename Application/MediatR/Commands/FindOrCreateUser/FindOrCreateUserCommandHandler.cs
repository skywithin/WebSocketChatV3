using Application.Db;
using Application.Db.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MediatR.Commands.FindOrCreateUser
{
    public class FindOrCreateUserCommandHandler : IRequestHandler<FindOrCreateUserCommand, FindOrCreateUserCommandResult>
    {
        private readonly GameDbContext _db;

        public FindOrCreateUserCommandHandler(GameDbContext db)
        {
            _db = db;
        }

        public async Task<FindOrCreateUserCommandResult> Handle(FindOrCreateUserCommand request, CancellationToken cancellationToken)
        {
            var userName = request.UserName;

            var user = 
                await _db.Users
                    .Include(x => x.Groups)
                    .FirstOrDefaultAsync(x => x.Name == userName);

            if (user == null)
            {
                user = new UserEntity { Name = userName, Groups = new List<GroupEntity>() };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }

            var groups =
                user.Groups
                    .Select(x => new ActiveGroup { GroupId = x.Id, GroupName = x.Name })
                    .ToList();

            return new FindOrCreateUserCommandResult
            {
                UserId = user.Id,
                Groups = groups
            };
        }
    }
}
