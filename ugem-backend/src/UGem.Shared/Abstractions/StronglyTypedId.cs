using System.ComponentModel;
using System.Text.Json.Serialization;

namespace UGem.Shared.Abstractions;

public interface IStronglyTypedId
{
    Guid Value { get; }
}

public abstract readonly record struct StronglyTypedId<TValue>(Guid Value) : IStronglyTypedId
    where TValue : struct, IStronglyTypedId
{
    public override string ToString() => Value.ToString();
}

// Domain Specific IDs
[JsonConverter(typeof(StronglyTypedIdJsonConverter<UserId>))]
public readonly record struct UserId(Guid Value) : IStronglyTypedId
{
    public static UserId New() => new(Guid.NewGuid());
    public static UserId Empty => new(Guid.Empty);
}

[JsonConverter(typeof(StronglyTypedIdJsonConverter<RestaurantId>))]
public readonly record struct RestaurantId(Guid Value) : IStronglyTypedId
{
    public static RestaurantId New() => new(Guid.NewGuid());
    public static RestaurantId Empty => new(Guid.Empty);
}

[JsonConverter(typeof(StronglyTypedIdJsonConverter<VoucherId>))]
public readonly record struct VoucherId(Guid Value) : IStronglyTypedId
{
    public static VoucherId New() => new(Guid.NewGuid());
}

// NOTE: Custom JsonConverter and TypeConverter implementation is omitted here for brevity 
// but is required for Swagger/API compatibility in a real setup.
