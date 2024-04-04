using MediportaTask.Entities;
using MediportaTask.Features.Tags.GetPaginated;
using MediportaTask.Features.Tags.ReseedDatabase;
using MediportaTask.Features.Tags.Services;
using MediportaTask.Misc;

namespace MediportaTask.Features.Tags;

public sealed class TagsModule : IModule
{
    public IEndpointRouteBuilder Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("tags", GetPaginatedTagsHandler.Handler);
        builder.MapPost("tags/reseed", ReseedTagsHandler.Handler);

        return builder;
    }

    public IServiceCollection Register(IServiceCollection services)
    {
        services.AddScoped<ITagsService, TagsService>();
        services.AddScoped<IOrderConfiguration<Tag>, TagOrderConfiguration>();
        
        return services;
    }
}
