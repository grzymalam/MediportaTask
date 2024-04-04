using MediportaTask.Entities;
using MediportaTask.Misc;
using System.Linq.Expressions;

namespace MediportaTask.Features.Tags.GetPaginated;

public sealed class TagOrderConfiguration : IOrderConfiguration<Tag>
{
    public IDictionary<string, Expression<Func<Tag, object>>> PropertyNameAccessors => new Dictionary<string, Expression<Func<Tag, object>>>
        {
            { nameof(Tag.Name), t => t.Name },
            { nameof(Tag.Count), t => t.Count }
        };

    public Expression<Func<Tag, object>> ResolvePropertyAccessor(string name)
    {
        return PropertyNameAccessors.FirstOrDefault(pa => pa.Key == name).Value 
            ?? PropertyNameAccessors.First().Value;
    }
}
