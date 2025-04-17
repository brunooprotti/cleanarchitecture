using CleanArchitecture.Domain.Abstractions.Specification.Impl;
using System.Linq.Expressions;

namespace CleanArchitecture.Domain.Vehiculos.Specification;

public sealed class VehiculoPaginationSpecification : BaseSpecification<Vehiculo, VehiculoId>
{
    public VehiculoPaginationSpecification(string sort, int pageIndex, int pageSize, string filter) 
        : base(
            x => string.IsNullOrEmpty(filter) || 
            x.Modelo == new Modelo(filter)) 
    {
        ApplyPaging(pageSize * (pageIndex - 1), pageSize);

        ApplyOrderMethod(sort);


    }

    private void ApplyOrderMethod(string sort)
    {
        switch (sort)
        {
            case "modeloAsc":
                AddOrderBy(p => p.Modelo!);
                break;

            case "modeloDesc":
                AddOrderByDescending(p => p.Modelo!);
                break;

            default:
                AddOrderBy(p => p.FechaUltimaAlquiler!);
                break;
        }
    }
      
}
