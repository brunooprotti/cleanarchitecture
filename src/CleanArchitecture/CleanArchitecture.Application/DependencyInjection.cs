using CleanArchitecture.Application.Abstractions.Behaviours;
using CleanArchitecture.Domain.Alquileres;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        //Asi registramos todos los Queries/Commands con sus respectivos Handlers
        services.AddMediatR(configuration =>  {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                configuration.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                configuration.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            } 
        );

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        //Aca registramos un servicio de DOMAIN ya que en ese proyecto no tenemos Inyeccion de dependencias!
        services.AddTransient<PrecioService>();

        return services;
    }
}
