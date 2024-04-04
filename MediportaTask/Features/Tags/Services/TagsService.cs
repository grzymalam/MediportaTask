using MediportaTask.Data.Contexts;
using MediportaTask.Entities;
using MediportaTask.Features.Tags.GetPaginated;
using MediportaTask.HostedServices.FetchSOTags;
using MediportaTask.Misc;
using MediportaTask.Misc.Database;
using MediportaTask.Misc.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace MediportaTask.Features.Tags.Services;

public sealed class TagsService : ITagsService
{
    private readonly MediportaDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<FetchSOTagsHostedService> _logger;
    private readonly IOrderConfiguration<Tag> _orderConfig;
    private readonly MediportaConfig _config;

    private const string StackOverflowTagsBasePath = "https://api.stackexchange.com/2.3/tags";
    private const int TagQuantityToFetch = 1000;
    private const int PageSize = 100;

    public TagsService(MediportaDbContext context,
        IHttpClientFactory httpClientFactory,
        ILogger<FetchSOTagsHostedService> logger,
        IOptions<MediportaConfig> config,
        IOrderConfiguration<Tag> orderConfig)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _config = config.Value;
        _orderConfig = orderConfig;
    }

    public async Task<GetPaginatedTagsResponse> GetPaginatedTags(GetPaginatedTagsRequest request)
    {
        var tags = await _context.Tags
            .ApplyPagination(request, _orderConfig)
            .Select(tag => new TagDto()
            {
                Id = tag.Id,
                Name = tag.Name,
                CountPercentageShare = tag.CountPercentageShare
            })
            .ToListAsync();

        var totalTags = await _context.Tags.CountAsync();

        return new GetPaginatedTagsResponse()
        {
            Total = (uint)totalTags,
            PageSize = request.PageSize,
            PageNumber = request.PageNumber,
            Items = tags,
        };
    }

    public async Task<IEnumerable<Tag>> FetchTags(CancellationToken cancellationToken)
    {
        var tasks = new List<Task>();
        var tags = new ConcurrentBag<StackOverflowTagDto>();
        var client = _httpClientFactory.CreateClient("stackoverflow");

        var page = 1;
        var tagsLeftToFetch = TagQuantityToFetch;
        while (tagsLeftToFetch > 0)
        {
            var uri = new Uri(StackOverflowTagsBasePath);
            uri = uri.AddQueryParameter("key", _config.StackOverflowApiKey);
            uri = uri.AddQueryParameter("site", "stackoverflow");
            uri = uri.AddQueryParameter("order", "desc");
            uri = uri.AddQueryParameter("filter", "default");
            uri = uri.AddQueryParameter("page", page.ToString());
            uri = uri.AddQueryParameter("pagesize", (PageSize - tagsLeftToFetch % PageSize).ToString());

            var fetchTask = Task.Run(async () =>
            {
                var fetchedTags = await client.GetFromJsonAsync<GetTagsResponse>(uri, cancellationToken);
                if (fetchedTags is null)
                {
                    _logger.LogError("Could not fetch tags from SO. Page {0}, PageSize {1}", page, PageSize);
                    throw new Exception($"Could not fetch tags from SO. Page {page}, PageSize {PageSize}");
                }

                fetchedTags.Items.ForEach(item => tags.Add(item));
            });

            tasks.Add(fetchTask);
            page += 1;
            tagsLeftToFetch -= PageSize;
        }

        await Task.WhenAll(tasks).WaitAsync(cancellationToken);

        var totalTags = tags.Sum(t => t.Count);
        return tags.Select(tag => new Tag(tag.Count, tag.Count * 1m / (totalTags * 1m), tag.Name));
    }

    public async Task AddTags(IEnumerable<Tag> tags)
    {
        await _context.AddRangeAsync(tags);
    }

    public async Task RemoveExistingTags()
    {
        var existingTags = await _context.Tags.ToListAsync();

        _context.RemoveRange(existingTags);
    }

    public async Task<bool> AreTagsSeeded()
    {
        var tagQuantity = await _context.Tags.CountAsync();
        return tagQuantity == TagQuantityToFetch;
    }
}
