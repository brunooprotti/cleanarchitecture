using FluentValidation;

namespace CleanArchitecture.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.Nombre).NotEmpty().WithMessage("El nombre no puede ser nulo o vacio");
        RuleFor(c => c.Apellido).NotEmpty().WithMessage("El apellido no puede ser nulo o vacio");
        RuleFor(c => c.Email).EmailAddress().WithMessage("El email debe tener un formato valido");
        RuleFor(c => c.Password).NotEmpty().MinimumLength(5);
    }
}