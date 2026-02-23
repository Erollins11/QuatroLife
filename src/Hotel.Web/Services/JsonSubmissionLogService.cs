using System.Text.Json;

namespace Hotel.Web.Services;

public sealed class JsonSubmissionLogService(
    IWebHostEnvironment environment,
    ILogger<JsonSubmissionLogService> logger) : ISubmissionLogService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = false
    };

    public async Task LogAsync<T>(string formName, string culture, T payload, CancellationToken cancellationToken = default)
    {
        var directory = Path.Combine(environment.ContentRootPath, "App_Data", "Submissions");
        Directory.CreateDirectory(directory);

        var fileName = $"{DateTime.UtcNow:yyyyMMdd}-{formName}.jsonl";
        var filePath = Path.Combine(directory, fileName);

        var envelope = new
        {
            timestampUtc = DateTime.UtcNow,
            formName,
            culture,
            payload
        };

        var line = JsonSerializer.Serialize(envelope, JsonOptions) + Environment.NewLine;
        await File.AppendAllTextAsync(filePath, line, cancellationToken);
        logger.LogInformation("Form submission logged: {FormName} ({Culture})", formName, culture);
    }
}
