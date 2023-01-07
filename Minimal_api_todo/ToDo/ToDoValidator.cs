using FluentValidation;

namespace ToDo.MinimalApi;

public class ToDoValidator : AbstractValidator<ToDoModel>
{
	public ToDoValidator()
	{
		RuleFor(t => t.Value)
			.NotEmpty()
			.MinimumLength(5)
			.WithMessage("Value of a todo must be at least 5 characters");
	}
}

