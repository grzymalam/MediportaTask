using MediportaTask.Features.Tags.Services;
using MediportaTask.Misc;
using Microsoft.AspNetCore.Mvc;

namespace MediportaTask.Features.Tags.ReseedDatabase;

public static class ReseedTagsHandler
{
    public static async Task<IResult> Handler([FromServices] ITagsService tagsService, [FromServices] IUnitOfWork unitOfWork, CancellationToken ct)
    {
        var tags = await tagsService.FetchTags(ct);
        if(tags is null)
        {
            return Results.NotFound();
        }

        await tagsService.RemoveExistingTags();
        await tagsService.AddTags(tags);
        await unitOfWork.CommitAsync();

        return Results.Ok();
    }
}
