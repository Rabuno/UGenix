using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UGem.Shared.Abstractions;

namespace UGem.Persistence.Converters;

public class StronglyTypedIdValueConverter<TId> : ValueConverter<TId, Guid> 
    where TId : struct, IStronglyTypedId
{
    public StronglyTypedIdValueConverter() 
        : base(id => id.Value, value => Create(value))
    {
    }

    private static TId Create(Guid value) => (TId)Activator.CreateInstance(typeof(TId), value)!;
}
