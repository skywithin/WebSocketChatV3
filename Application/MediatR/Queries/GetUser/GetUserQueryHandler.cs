using Application.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MediatR.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQueryResult>
    {
        private readonly GameDbContext _db;

        public GetUserQueryHandler(GameDbContext db)
        {
            _db = db;
        }

        public async Task<GetUserQueryResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            GetUserQueryResult result = null;

            var user = 
                await _db.Users
                    .Include(x => x.Groups)
                    .Where(x => x.Id == request.UserId)
                    .FirstOrDefaultAsync();

            var userGroups = 
                user.Groups
                    .Select(x => new ActiveGroup { GroupId = x.Id, GropName = x.Name })
                    .ToList();

            if (user != null)
            {
                result =
                    new GetUserQueryResult
                    {
                        UserId = user.Id,
                        UserName = user.Name,
                        Groups = userGroups
                    };
            }

            return result;
        }
    }
}
