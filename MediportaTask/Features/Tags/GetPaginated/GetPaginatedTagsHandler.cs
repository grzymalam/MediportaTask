using MediportaTask.Features.Tags.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediportaTask.Features.Tags.GetPaginated;

public static class GetPaginatedTagsHandler
{
    public static async Task<IResult> Handler([FromBody]GetPaginatedTagsRequest request, [FromServices] ITagsService tagsService)
        => Results.Ok(await tagsService.GetPaginatedTags(request));
}
