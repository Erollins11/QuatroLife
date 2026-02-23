using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Hotel.Web.Services;

public sealed class JsonDataRepository(
    IWebHostEnvironment environment,
    IMemoryCache cache,
    ILogger<JsonDataRepository> logger) : IDataRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<IReadOnlyList<T>> ReadListAsync<T>(string fileName, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"json::{typeof(T).Name}::{fileName}";
        if (cache.TryGetValue(cacheKey, out IReadOnlyList<T>? cached) && cached is not null)
        {
            return cached;
        }

        var dataDirectory = Path.Combine(environment.ContentRootPath, "Data");
        var filePath = Path.Combine(dataDirectory, fileName);

        if (!File.Exists(filePath))
        {
            logger.LogWarning("Data file missing: {FilePath}", filePath);
            return [];
        }

        await using var stream = File.OpenRead(filePath);
        var items = await JsonSerializer.DeserializeAsync<List<T>>(stream, JsonOptions, cancellationToken);
        var result = (IReadOnlyList<T>)(items ?? []);

        cache.Set(cacheKey, result, TimeSpan.FromMinutes(20));
        return result;
    }
}
