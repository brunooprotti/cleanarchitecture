using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configuration;

internal sealed class VehiculoConfiguration : IEntityTypeConfiguration<Vehiculo>
{
    public void Configure(EntityTypeBuilder<Vehiculo> builder)
    {
        builder.ToTable("vehiculos");
        builder.HasKey(vehiculo => vehiculo.Id);

        //Esto nos permite guardar el objeto direccion dentro del objeto vehiculo.
        //Indicamos que vehiculo es dueño de una direccion.
        //Basicamente a los objectValue con varias properties los agregamos asi, a los demas los agregamos con las rules abajo.
        builder.OwnsOne(vehiculo => vehiculo.Direccion);
        

        //Tomamos el value porque asi indicamos que tome el string, no el objeto Modelo.
        //Con el hasConversion indicamos que vamos a tomar la propiedad Value y vamos a guardarla como string y cada vez que recibamos un modelo lo vamos a crear como objeto Modelo.
        builder.Property(vehiculo => vehiculo.Modelo)
            .HasMaxLength(200)
            .HasConversion( modelo => modelo!.Value, value => new Modelo(value));


        builder.Property(vehiculo => vehiculo.Vin)
            .HasMaxLength(500)
            .HasConversion( vin => vin!.Value, value => new Vin(value));

        builder.OwnsOne(vehiculo => vehiculo.Precio, priceBuilder => {
            priceBuilder.Property(moneda => moneda.TipoMoneda)
                .HasConversion(tipoMoneda => tipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo));
        }); 
        
        builder.OwnsOne(vehiculo => vehiculo.Mantenimiento, priceBuilder => {
            priceBuilder.Property(moneda => moneda.TipoMoneda)
                .HasConversion(tipoMoneda => tipoMoneda.Codigo, codigo => TipoMoneda.FromCodigo(codigo));
        });

        builder.Property<uint>("Version").IsRowVersion();
    }
}