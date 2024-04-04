using MediportaTask.Data.Contexts;
using MediportaTask.Entities;
using MediportaTask.Features.Tags.Services;
using MediportaTask.Misc;
using MediportaTask.Misc.Http;

namespace MediportaTask.HostedServices.FetchSOTags;

public sealed class FetchSOTagsHostedService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<FetchSOTagsHostedService> _logger;

    public FetchSOTagsHostedService(
        IServiceScopeFactory scopeFactory, 
        ILogger<FetchSOTagsHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await RunAsync(cancellationToken);
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was error while fetching tags from SO. Waiting before retrying...");
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task RunAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var tagsService = scope.ServiceProvider.GetService<ITagsService>();
        if(tagsService is null)
        {
            throw new Exception($"No service implementation found for {nameof(ITagsService)}.");
        }

        if (await tagsService.AreTagsSeeded())
        {
            return;
        }

        var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
        if (unitOfWork is null)
        {
            throw new Exception($"No service implementation found for {nameof(IUnitOfWork)}.");
        }

        var tags = await tagsService.FetchTags(cancellationToken);

        await tagsService.RemoveExistingTags();
        await tagsService.AddTags(tags);
        await unitOfWork.CommitAsync();
    }
}
