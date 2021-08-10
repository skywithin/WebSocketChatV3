using MediatR;

namespace Application.MediatR.Commands.FindOrCreateUser
{
    public class FindOrCreateUserCommand : IRequest<FindOrCreateUserCommandResult>
    {
        public string UserName { get; set; }
    }
}
