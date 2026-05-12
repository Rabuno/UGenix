using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UGenix.Shared.Abstractions;

namespace UGenix.Persistence.Converters;

public class StronglyTypedIdValueConverter<TId> : ValueConverter<TId, Guid> 
    where TId : struct, IStronglyTypedId
{
    public StronglyTypedIdValueConverter() 
        : base(id => id.Value, value => Create(value))
    {
    }

    private static TId Create(Guid value) => (TId)Activator.CreateInstance(typeof(TId), value)!;
}

