using MediatR;

namespace Softplan.API.Application.Commands
{
    public class DeleteTaskCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}