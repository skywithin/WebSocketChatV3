using Application.Db;
using Application.Db.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MediatR.Commands.FindOrCreateGroup
{
    public class FindOrCreateGroupCommandHandler : IRequestHandler<FindOrCreateGroupCommand, FindOrCreateGroupCommandResult>
    {
        private readonly GameDbContext _db;

        public FindOrCreateGroupCommandHandler(GameDbContext db)
        {
            _db = db;
        }

        public async Task<FindOrCreateGroupCommandResult> Handle(FindOrCreateGroupCommand request, CancellationToken cancellationToken)
        {
            var groupName = request.GroupName;

            var group = 
                await _db.Groups
                    .Include(x => x.Members)
                    .Where(x => x.Name == groupName)
                    .SingleOrDefaultAsync();

            if (group == null)
            {
                group = new GroupEntity { Name = groupName };

                _db.Groups.Add(group);
                await _db.SaveChangesAsync();
            }

            return new FindOrCreateGroupCommandResult
            {
                GroupId = group.Id,
                GroupName = groupName,
                MemberIds = group.Members?.Select(x => x.Id).ToList() ?? new List<Guid>()
            };
        }
    }
}
