using MediportaTask.Entities;
using MediportaTask.Features.Tags.GetPaginated;

namespace MediportaTask.Features.Tags.Services;

public interface ITagsService
{
    Task<bool> AreTagsSeeded();
    Task AddTags(IEnumerable<Tag> tags);
    Task RemoveExistingTags();
    Task<IEnumerable<Tag>> FetchTags(CancellationToken cancellationToken);
    Task<GetPaginatedTagsResponse> GetPaginatedTags(GetPaginatedTagsRequest request);
}