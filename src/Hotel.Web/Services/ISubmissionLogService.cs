namespace Hotel.Web.Services;

public interface ISubmissionLogService
{
    Task LogAsync<T>(string formName, string culture, T payload, CancellationToken cancellationToken = default);
}
