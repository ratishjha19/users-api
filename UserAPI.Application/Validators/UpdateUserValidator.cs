using DemoApp.Application.DTOs.User;
using FluentValidation;

namespace DemoApp.Application.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must be at most 100 characters.");

            RuleFor(x => x.Age)
                .GreaterThan(0)
                .WithMessage("Age must be greater than 0.");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required.")
                .MaximumLength(100);

            RuleFor(x => x.State)
                .NotEmpty()
                .WithMessage("State is required.")
                .MaximumLength(100);

            RuleFor(x => x.Pincode)
                .NotEmpty()
                .WithMessage("Pincode is required.")
                .MaximumLength(10)
                .WithMessage("Pincode must be at most 10 characters.");
        }
    }
}
