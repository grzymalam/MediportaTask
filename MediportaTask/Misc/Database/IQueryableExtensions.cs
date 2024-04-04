using MediportaTask.Dtos.Requests.Abstractions;
using MediportaTask.Dtos.Requests.Enums;

namespace MediportaTask.Misc.Database;

public static class IQueryableExtensions
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> queryable, IPaginatedRequest request, IOrderConfiguration<T> orderConfig)
    {
        var orderPropertyInfo = typeof(T)
            .GetProperty(request.OrderPropertyName);

        if(orderPropertyInfo is null) 
        {
            throw new ArgumentException("Attempted to sort by a property that does not exist.");
        }

        queryable = request.OrderDirection == OrderDirection.Asc 
            ? queryable.OrderBy(orderConfig.ResolvePropertyAccessor(request.OrderPropertyName))
            : queryable.OrderByDescending(orderConfig.ResolvePropertyAccessor(request.OrderPropertyName));
        
        queryable = queryable
            .Skip((int)(request.PageSize * request.PageNumber))
            .Take((int)request.PageSize);

        return queryable;
    }
}
