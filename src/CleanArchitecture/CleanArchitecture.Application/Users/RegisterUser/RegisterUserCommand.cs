using CleanArchitecture.Application.Abstractions.Messaging;

namespace CleanArchitecture.Application.Users.RegisterUser;

/// <summary>
/// Command para registrar un nuevo usuario
/// </summary>
/// <param name="Email">Email para el nuevo usuario</param>
/// <param name="Nombre">Nombre para el nuevo usuario</param>
/// <param name="Apellido">Apellido para el nuevo usuario</param>
/// <param name="Password">Password para el nuevo usuario</param>
public sealed record RegisterUserCommand(string Email, string Nombre, string Apellido, string Password) : ICommand<Guid>;