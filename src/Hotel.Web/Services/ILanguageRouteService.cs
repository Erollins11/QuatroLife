namespace Hotel.Web.Services;

public interface ILanguageRouteService
{
    string PathFor(string culture, string routeKey, string? slug = null);
    string MapPathToCulture(string currentPath, string targetCulture);
}
