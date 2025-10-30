using MediatR;

namespace Softplan.API.Application.Commands
{
    public class CompleteTaskCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}