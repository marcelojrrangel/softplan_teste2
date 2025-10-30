using FluentValidation;
using TaskEntity = Softplan.API.Domain.Entities.Task;

namespace Softplan.API.Application.DTOs
{
    public class TaskValidator : AbstractValidator<TaskEntity>
    {
        public TaskValidator()
        {
            RuleFor(t => t.Title)
                .NotEmpty().WithMessage("O título é obrigatório.")
                .MaximumLength(100).WithMessage("O título deve ter no máximo 100 caracteres.");

            RuleFor(t => t.UserId)
                .NotEmpty().WithMessage("O UserId é obrigatório.");

            RuleFor(t => t.Description)
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.")
                .When(t => !string.IsNullOrEmpty(t.Description));

            RuleFor(t => t.DueDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("A data de vencimento deve ser no futuro.")
                .When(t => t.DueDate.HasValue);
        }
    }
}