using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Vehiculos;

public sealed class Vehiculo : Entity<VehiculoId>
{
    private Vehiculo() { }
    public Vehiculo(VehiculoId id, Modelo? modelo, Vin? vin, Direccion? direccion, Moneda? precio, Moneda? mantenimiento, DateTime? fechaUltimaAlquiler, List<Accesorio> accesorios) : base(id)
    {
        Modelo = modelo;
        Vin = vin;
        Direccion = direccion;
        Precio = precio;
        Mantenimiento = mantenimiento;
        FechaUltimaAlquiler = fechaUltimaAlquiler;
        Accesorios = accesorios;
    }

    public Modelo? Modelo { get; private set; }
    public Vin? Vin { get; private set; }
    public Direccion? Direccion { get; private set; }
    public Moneda? Precio { get; private set; }  
    public Moneda? Mantenimiento { get; private set; }
    public DateTime? FechaUltimaAlquiler { get; internal set; }
    public List<Accesorio> Accesorios { get; private set; } = new ();
}
