using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Softplan.API.Presentation.DTOs;
using Softplan.API.Application.Commands;
using Softplan.API.Application.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Softplan.API.Presentation.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/tasks")]
    [Authorize]
    [ApiVersion("1.0")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TasksController> _logger;

        public TasksController(IMediator mediator, ILogger<TasksController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponse>> CreateTask(CreateTaskRequest request)
        {
            try
            {
                var command = new CreateTaskCommand
                {
                    Title = request.Title,
                    Description = request.Description,
                    DueDate = request.DueDate,
                    UserId = request.UserId
                };
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(CreateTask), new { userId = result.UserId }, result);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed for task creation: {Errors}", string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)));
                var problemDetails = new ProblemDetails
                {
                    Status = 400,
                    Title = "Bad Request",
                    Detail = "Validation failed",
                    Extensions = { ["errors"] = ex.Errors.Select(e => e.ErrorMessage) }
                };
                return BadRequest(problemDetails);
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<TaskResponse>>> GetTasksByUser(string userId)
        {
            var query = new GetTasksByUserQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteTask(int id)
        {
            var command = new CompleteTaskCommand { Id = id };
            var task = await _mediator.Send(command);

            if (!task)
            {
                return Problem(
                    detail: $"Task with ID {id} not found",
                    statusCode: 404,
                    title: "Not Found"
                );
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var command = new DeleteTaskCommand { Id = id };
            var task = await _mediator.Send(command);

            if (!task)
            {
                return Problem(
                    detail: $"Task with ID {id} not found",
                    statusCode: 404,
                    title: "Not Found"
                );
            }

            return NoContent();
        }
    }
}
