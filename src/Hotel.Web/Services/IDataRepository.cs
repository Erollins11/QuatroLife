namespace Hotel.Web.Services;

public interface IDataRepository
{
    Task<IReadOnlyList<T>> ReadListAsync<T>(string fileName, CancellationToken cancellationToken = default);
}
