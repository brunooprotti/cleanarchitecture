using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Application.Abstractions.Clock;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Alquileres.ReservarAlquiler;

internal sealed class ReservarAlquilerCommandHanlder : ICommandHandler<ReservarAlquilerCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly IAlquilerRepository _alquilerRepository;
    private readonly PrecioService _precioService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ReservarAlquilerCommandHanlder(
        IUserRepository userRepository, 
        IVehiculoRepository vehiculoRepository, 
        IAlquilerRepository alquilerRepository, 
        PrecioService precioService, 
        IUnitOfWork unitOfWork, 
        IDateTimeProvider dateTimeProvider
        )
    {
        _userRepository = userRepository;
        _vehiculoRepository = vehiculoRepository;
        _alquilerRepository = alquilerRepository;
        _precioService = precioService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<Guid>> Handle(ReservarAlquilerCommand request, CancellationToken cancellationToken)
    {
        //busqueda de usuario
        var userId = new UserId(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user == null){
            return Result.Failure<Guid>(UserErrors.NotFound);
        }
        var vehiculoId = new VehiculoId(request.VehiculoId);
        var vehiculo = await _vehiculoRepository.GetByIdAsync(vehiculoId, cancellationToken);

        if (vehiculo == null){
            return Result.Failure<Guid>(VehiculoErrors.NotFound);
        }

        var duracion = DateRange.Create(request.FechaInicio,request.FechaFin);

        if(await _alquilerRepository.IsOverlappingAsync(vehiculo, duracion, cancellationToken)){
            return Result.Failure<Guid>(AlquilerErrors.Overlap);
        }

        
        try
        {
            var alquiler = Alquiler.Reservar(vehiculo, userId, duracion, _dateTimeProvider.CurrentTime, _precioService);

            _alquilerRepository.Add(alquiler);

            await _unitOfWork.SaveChangesAsync();
            
            return alquiler.Id!.value;     
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Guid>(AlquilerErrors.Overlap);
        }
        
    }
}
