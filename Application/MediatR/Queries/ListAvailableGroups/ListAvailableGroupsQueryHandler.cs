using Application.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MediatR.Queries.ListAvailableGroups
{
    public class ListAvailableGroupsQueryHandler : IRequestHandler<ListAvailableGroupsQuery, ListAvailableGroupsQueryResult>
    {
        private readonly GameDbContext _db;

        public ListAvailableGroupsQueryHandler(GameDbContext db)
        {
            _db = db;
        }

        public async Task<ListAvailableGroupsQueryResult> Handle(ListAvailableGroupsQuery request, CancellationToken cancellationToken)
        {
            var groups =
                await _db.Groups
                    .Include(x => x.Members)
                    .ToListAsync();

            return new ListAvailableGroupsQueryResult
            {
                ActiveGroups = groups
                    .Select(g => new ActiveGroup( g.Id, g.Name, g.Members.Count() ))
                    .ToList()
            };
        }
    }
}
