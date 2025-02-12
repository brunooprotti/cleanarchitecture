using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres.Events;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Domain.Alquileres;

public sealed class Alquiler : Entity<AlquilerId>
{
    private Alquiler(){ }

    private Alquiler(AlquilerId id,VehiculoId vehiculoId, UserId userId, DateRange duracion, Moneda? precioPorPeriodo, Moneda? precioPorMantenimiento, Moneda? accesorios, Moneda? precioTotal, AlquilerStatus status, DateTime fechaCreacion) : base(id)
    {
        VehiculoId = vehiculoId;
        UserId = userId;
        PrecioPorPeriodo = precioPorPeriodo;
        PrecioPorMantenimiento = precioPorMantenimiento;
        Accesorios = accesorios;
        PrecioTotal = precioTotal;
        Status = status;
        Duracion = duracion;
        FechaCreacion = fechaCreacion;
    }

    public VehiculoId? VehiculoId { get; private set; }
    public UserId? UserId { get; private set; }
    public Moneda? PrecioPorPeriodo { get; private set; }
    public Moneda? PrecioPorMantenimiento { get; private set; }
    public Moneda? Accesorios { get; private set; }
    public Moneda? PrecioTotal { get; private set; }

    public AlquilerStatus Status { get; private set; }

    public DateRange? Duracion { get; private set; }

    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaConfirmacion { get; private set; }
    public DateTime FechaDenegacion { get; private set; }
    public DateTime FechaCompletado { get; private set; }
    public DateTime FechaCancelacion { get; private set; }

    public static Alquiler Reservar(Vehiculo vehiculo, UserId userId, DateRange duracion, DateTime fechaCreacion, PrecioService precioService)
    {
        var precioDetalle = precioService.CalcularPrecio(vehiculo,duracion);

        var alquiler = new Alquiler(AlquilerId.New(), vehiculo.Id!,userId,duracion,precioDetalle.PrecioPorPeriodo,precioDetalle.Mantenimiento,precioDetalle.Accesorios,precioDetalle.PrecioTotal,AlquilerStatus.Reservado,fechaCreacion);
        alquiler.RaiseDomainEvent(new AlquilerReservadoDomainEvent(alquiler.Id!));

        vehiculo.FechaUltimaAlquiler = fechaCreacion;

        return alquiler;
    }

    public Result Confirmar(DateTime utcNow)
    {
        if (Status != AlquilerStatus.Reservado)
        {
            //Se dispara una excepton con un mensaje de error
            return Result.Failure(AlquilerErrors.NotReserved);
        }

        Status = AlquilerStatus.Confirmado;
        FechaConfirmacion = utcNow;

        RaiseDomainEvent(new AlquilerConfirmadoDomainEvent(Id!));

        return Result.Success();
    }

    public Result Rechazar(DateTime utcNow)
    {
        if (Status != AlquilerStatus.Reservado)
        {
            //Se dispara una excepton con un mensaje de error
            return Result.Failure(AlquilerErrors.NotReserved);
        }

        Status = AlquilerStatus.Rechazado;
        FechaDenegacion = utcNow;

        RaiseDomainEvent(new AlquilerRechazadoDomainEvent(Id!));

        return Result.Success();
    }    
    
    public Result Cancelar(DateTime utcNow)
    {
        if (Status != AlquilerStatus.Confirmado)
        {
            //Se dispara una excepton con un mensaje de error
            return Result.Failure(AlquilerErrors.NotConfirmed);
        }

        var currentDate = DateOnly.FromDateTime(utcNow);

        //Error por si ya comenzo el alquiler y lo quieren cancelar
        if(currentDate > Duracion!.Inicio)
        {
            return Result.Failure(AlquilerErrors.AlreadyStarted);
        }
        
        Status = AlquilerStatus.Cancelado;
        FechaCancelacion = utcNow;

        RaiseDomainEvent(new AlquilerCanceladoDomainEvent(Id!));

        return Result.Success();
    }    
    
    public Result Completar(DateTime utcNow)
    {
        if (Status != AlquilerStatus.Confirmado)
        {
            //Se dispara una excepton con un mensaje de error
            return Result.Failure(AlquilerErrors.NotConfirmed);
        }

        Status = AlquilerStatus.Completado;
        FechaCancelacion = utcNow;

        RaiseDomainEvent(new AlquilerCompletadoDomainEvent(Id!));

        return Result.Success();
    }
}
