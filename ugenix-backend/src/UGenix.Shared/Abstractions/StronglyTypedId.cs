using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UGenix.Shared.Abstractions;

public interface IStronglyTypedId
{
    Guid Value { get; }
}

public readonly record struct StronglyTypedId<TValue>(Guid Value) : IStronglyTypedId
{
    public override string ToString() => Value.ToString();
}

// JSON Converter for Strongly Typed IDs
public class StronglyTypedIdJsonConverter<T> : JsonConverter<T> where T : struct, IStronglyTypedId
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var guid = reader.GetGuid();
        return (T)Activator.CreateInstance(typeToConvert, guid)!;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
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

