using MediatR;

namespace Application.MediatR.Commands.FindOrCreateGroup
{
    public class FindOrCreateGroupCommand : IRequest<FindOrCreateGroupCommandResult>
    {
        public string GroupName { get; set; }
    }
}
