using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Alquileres;

public static class AlquilerErrors
{
    public static Error NotFound = new Error(
        "Alquiler.NotFound",
        "El alquiler con el Id especificado no fue encontrado"
    );

    //Cuando tratamos de alquilar el mismo auto en las mismas fechas
    public static Error Overlap = new Error(
        "Alquiler.Overlap",
        "El alquiler esta siendo tomado por 2 o mas clientes al mismo tiempo en la misma fecha"
    );
    
    public static Error NotReserved = new Error(
        "Alquiler.NotReserved",
        "El alquiler no esta reservado"
    );
    
    public static Error NotConfirmed = new Error(
        "Alquiler.NotConfirmed",
        "El alquiler no esta confirmado"
    );
    
    public static Error AlreadyStarted = new Error(
        "Alquiler.AlreadyStarted",
        "El alquiler ya ha comenzado"
    );
}
