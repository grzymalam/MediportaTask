using System.Linq.Expressions;

namespace MediportaTask.Misc;

public interface IOrderConfiguration<T>
{
    public IDictionary<string, Expression<Func<T, object>>> PropertyNameAccessors { get; }
    public Expression<Func<T, object>> ResolvePropertyAccessor(string name);
}
