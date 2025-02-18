using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Users.RegisterUser;

internal class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);
        //1. Validar que el usuario no exista
        if(!await _userRepository.IsUserExists(email))
            return Result.Failure<Guid>(UserErrors.AlreadyExists);
        

        //2. Encriptar el password para no guardarlo plano en la DB
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        //3. Crear el usuario
        var user = CreateNewUser(request, passwordHash);
        
        //4. Guardar el usuario en la DB
        _userRepository.Add(user);
        await _unitOfWork.SaveChangesAsync();
        
        return user.Id!.Value;
    }

    private User CreateNewUser(RegisterUserCommand request, string passwordHash) 
        => User.Create(
            new Nombre(request.Nombre),
            new Apellido(request.Apellido),
            new Email(request.Email),
            new PasswordHash(passwordHash)
        );
}
