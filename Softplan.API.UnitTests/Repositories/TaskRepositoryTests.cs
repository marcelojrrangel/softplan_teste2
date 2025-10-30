using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Softplan.API.Infrastructure.Data;
using Softplan.API.Domain.Interfaces;
using Softplan.API.Infrastructure.Repositories;
using TaskEntity = Softplan.API.Domain.Entities.Task;

namespace Softplan.API.UnitTests.Repositories;

public class TaskRepositoryTests : IDisposable
{
    private readonly DataContext _context;
    private readonly TaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _testUserId = "user-123";

    public TaskRepositoryTests()
    {
        // 1. Configura o provedor In-Memory com um nome de banco de dados único para isolamento
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DataContext(options);
        _repository = new TaskRepository(_context);
        _unitOfWork = new UnitOfWork(_context);

        // 2. Popula o banco de dados In-Memory com dados iniciais (Seed Data)
        _context.Tasks.AddRange(
            new TaskEntity { Id = 1, Title = "Tarefa Antiga", UserId = _testUserId, IsCompleted = false },
            new TaskEntity { Id = 2, Title = "Tarefa do Usuário 123", UserId = _testUserId, IsCompleted = true },
            new TaskEntity { Id = 3, Title = "Tarefa de Outro Usuário", UserId = "user-456", IsCompleted = false }
        );
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTask_WhenIdExists()
    {
        // Arrange
        var expectedId = 2;

        // Act
        var result = await _repository.GetByIdAsync(expectedId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedId, result.Id);
        Assert.Equal("Tarefa do Usuário 123", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTasks()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task FindAsync_ShouldReturnFilteredTasks()
    {
        // Act
        // Busca tarefas completadas
        var result = await _repository.FindAsync(t => t.IsCompleted);

        // Assert
        Assert.NotNull(result);
        // Apenas a tarefa com Id 2 está completa no seed
        Assert.Single(result);
        Assert.Equal(2, result.First().Id);
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnOnlyUserTasks()
    {
        // Act
        var result = await _repository.GetByUserIdAsync(_testUserId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.True(result.All(t => t.UserId == _testUserId));
    }

        [Fact]
        public async Task AddAsync_ShouldAddTheTaskToContext()
        {
            // Arrange
            var newTask = new TaskEntity { Title = "Nova Tarefa", UserId = _testUserId };

        // Act
        await _repository.AddAsync(newTask);
        await _unitOfWork.SaveChangesAsync(); // Salva as mudanças

        // Assert
        // Verifica se o total de itens aumentou
        var totalTasks = await _context.Tasks.CountAsync();
        Assert.Equal(4, totalTasks);

        // Verifica se a nova tarefa está lá
        var addedTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Title == "Nova Tarefa");
        Assert.NotNull(addedTask);
    }

    [Fact]
    public async Task Update_ShouldModifyTheTaskInContext()
    {
        // Arrange
        var taskToUpdate = await _repository.GetByIdAsync(1);
        var newTitle = "Título Atualizado";

        // Act
        taskToUpdate.Title = newTitle;
        _repository.Update(taskToUpdate); // Marca como modificado
        await _unitOfWork.SaveChangesAsync(); // Persiste a mudança

        // Assert
        // Busca a tarefa novamente para confirmar a atualização
        var updatedTask = await _repository.GetByIdAsync(1);
        Assert.NotNull(updatedTask);
        Assert.Equal(newTitle, updatedTask.Title);
    }

    [Fact]
    public async System.Threading.Tasks.Task Delete_ShouldRemoveTheTaskFromContext()
    {
        // Arrange
        var taskToDelete = await _repository.GetByIdAsync(3);
        var initialCount = await _context.Tasks.CountAsync(); // 3

        // Act
        _repository.Delete(taskToDelete); // Marca como deletado
        await _unitOfWork.SaveChangesAsync(); // Persiste a remoção

        // Assert
        var finalCount = await _context.Tasks.CountAsync();
        Assert.Equal(initialCount - 1, finalCount);

        // Tenta buscar a tarefa deletada
        var deletedTask = await _repository.GetByIdAsync(3);
        Assert.Null(deletedTask);
    }
}
