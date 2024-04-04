using MediportaTask.Dtos.Requests.Abstractions;
using MediportaTask.Dtos.Requests.Enums;

namespace MediportaTask.Features.Tags.GetPaginated;

public sealed class GetPaginatedTagsRequest : IPaginatedRequest
{
    public uint PageNumber { get; set; }
    public uint PageSize { get; set; }
    public OrderDirection OrderDirection { get; set; }
    public string OrderPropertyName { get; set; }
}
