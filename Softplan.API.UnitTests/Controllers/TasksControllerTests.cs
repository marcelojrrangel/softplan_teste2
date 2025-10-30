using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Softplan.API.Presentation.Controllers;
using Softplan.API.Application.DTOs;
using Softplan.API.Application.Commands;
using Softplan.API.Application.Queries;
using Xunit;

namespace Softplan.API.UnitTests.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<TasksController>> _logger;

        public TasksControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<TasksController>>();
        }

        private TasksController CreateController()
        {
            return new TasksController(_mediator.Object, _logger.Object);
        }

        [Fact]
        public async Task CreateTask_ReturnsCreatedAtAction()
        {
            // Arrange
            var controller = CreateController();
            var request = new CreateTaskRequest { Title = "Test Task", Description = "Test Description", UserId = "testuser" };
            var response = new TaskResponse { Id = 1, Title = "Test Task", Description = "Test Description", UserId = "testuser" };

            _mediator.Setup(x => x.Send(It.Is<CreateTaskCommand>(c => c.Title == request.Title && c.UserId == request.UserId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await controller.CreateTask(request);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("CreateTask", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task GetTasksByUser_ReturnsTasks()
        {
            // Arrange
            var controller = CreateController();
            var task1 = new TaskResponse { Id = 1, Title = "Test Task", Description = "Test Description", UserId = "testuser" };
            var task2 = new TaskResponse { Id = 2, Title = "Test Task2", Description = "Test Description2", UserId = "testuser" };
            var tasksMock = new List<TaskResponse> { task1, task2 };

            _mediator.Setup(x => x.Send(It.Is<GetTasksByUserQuery>(q => q.UserId == "testuser"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tasksMock);

            // Act
            var result = await controller.GetTasksByUser("testuser");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<TaskResponse>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var tasks = Assert.IsAssignableFrom<IEnumerable<TaskResponse>>(okResult.Value);
            Assert.Equal(2, tasks.Count());
        }

        [Fact]
        public async Task CompleteTask_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();

            _mediator.Setup(x => x.Send(It.Is<CompleteTaskCommand>(c => c.Id == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await controller.CompleteTask(1);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
            Assert.Equal(404, problemDetails.Status);
        }

        [Fact]
        public async Task CompleteTask_ReturnsNoContent()
        {
            // Arrange
            var controller = CreateController();

            _mediator.Setup(x => x.Send(It.Is<CompleteTaskCommand>(c => c.Id == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await controller.CompleteTask(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTask_ReturnsNoContent()
        {
            // Arrange
            var controller = CreateController();
            _mediator.Setup(x => x.Send(It.Is<DeleteTaskCommand>(c => c.Id == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await controller.DeleteTask(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTask_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();
            _mediator.Setup(x => x.Send(It.Is<DeleteTaskCommand>(c => c.Id == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await controller.DeleteTask(1);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
            Assert.Equal(404, problemDetails.Status);
        }
    }
}
