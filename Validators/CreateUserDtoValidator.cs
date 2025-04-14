
using FluentValidation;
using SchedulingSystemAPI.DTOs.UserDtos;

namespace SchedulingSystemAPI.Validators;

// Na pasta Validators
public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}