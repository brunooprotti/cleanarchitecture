using System.Data;
using Dapper;

namespace CleanArchitecture.Infrastructure.Data;

internal sealed class DateOnlyTypeHanlder : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value) => DateOnly.FromDateTime((DateTime)value);

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value;
    }
}