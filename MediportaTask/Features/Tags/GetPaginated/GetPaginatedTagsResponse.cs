using MediportaTask.Dtos.Responses.Abstractions;

namespace MediportaTask.Features.Tags.GetPaginated;

public sealed class GetPaginatedTagsResponse : IPaginatedResponse<TagDto>
{
    public uint Total { get; set; }
    public uint PageSize { get; set; }
    public uint PageNumber { get; set; }
    public IEnumerable<TagDto> Items { get; set; }
}
