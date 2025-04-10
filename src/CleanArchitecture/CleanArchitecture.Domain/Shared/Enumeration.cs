using System.Reflection;

namespace CleanArchitecture.Domain.Shared;

public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();

    public int Id { get; protected init; }
    public string Name { get; protected init; } = string.Empty;

    public Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static TEnum? FromValue(int id)
        => Enumerations.TryGetValue(id, out TEnum? enumeration) ? enumeration : default;

    public static TEnum? FromName(string name)
        => Enumerations.Values.SingleOrDefault(x => x.Name == name);

    public static List<TEnum> GetValues()
        => Enumerations.Values.ToList();

    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetType() == other.GetType() && Id == other.Id;
    }

    public override bool Equals(object? obj) => obj is Enumeration<TEnum> other && Equals(other);

    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => Name;

    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);

        var fieldsForType = enumerationType.GetFields(
            BindingFlags.Public | 
            BindingFlags.Static |
            BindingFlags.FlattenHierarchy
        ).Where(fieldInfo => enumerationType.IsAssignableFrom(fieldInfo.FieldType))
        .Select(fieldInfo => (TEnum) fieldInfo.GetValue(default)!);

        return fieldsForType.ToDictionary(x => x.Id);
    }
}
