namespace Hotel.Web.Services;

public sealed class LanguageRouteService : ILanguageRouteService
{
    private static readonly Dictionary<string, string> TrToEn = new(StringComparer.OrdinalIgnoreCase)
    {
        ["konaklama"] = "accommodation",
        ["yeme-icme"] = "dining",
        ["deneyimler"] = "experiences",
        ["wellness"] = "wellness",
        ["teklifler"] = "offers",
        ["etkinlikler"] = "events",
        ["galeri"] = "gallery",
        ["iletisim"] = "contact",
        ["rezervasyon"] = "book",
        ["kvkk"] = "personal-data",
        ["gizlilik"] = "privacy",
        ["cerez-politikasi"] = "cookies",
        ["sanal-tur"] = "tour"
    };

    private static readonly Dictionary<string, string> EnToTr = TrToEn.ToDictionary(pair => pair.Value, pair => pair.Key, StringComparer.OrdinalIgnoreCase);

    public string PathFor(string culture, string routeKey, string? slug = null)
    {
        var normalizedCulture = NormalizeCulture(culture);
        var key = routeKey.Trim().ToLowerInvariant();

        if (key is "home")
        {
            return $"/{normalizedCulture}";
        }

        string segment;
        if (normalizedCulture == "tr")
        {
            segment = key;
        }
        else
        {
            segment = TrToEn.TryGetValue(key, out var mapped) ? mapped : key;
        }

        return slug is null
            ? $"/{normalizedCulture}/{segment}"
            : $"/{normalizedCulture}/{segment}/{slug}";
    }

    public string MapPathToCulture(string currentPath, string targetCulture)
    {
        var normalizedTargetCulture = NormalizeCulture(targetCulture);
        var input = currentPath ?? string.Empty;

        var queryStart = input.IndexOf('?');
        var fragmentStart = input.IndexOf('#');
        var endOfPath = queryStart >= 0 && fragmentStart >= 0
            ? Math.Min(queryStart, fragmentStart)
            : queryStart >= 0 ? queryStart : fragmentStart;

        var pathOnly = endOfPath >= 0 ? input[..endOfPath] : input;
        var suffix = endOfPath >= 0 ? input[endOfPath..] : string.Empty;

        var segments = pathOnly.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length == 0)
        {
            return $"/{normalizedTargetCulture}{suffix}";
        }

        var sourceCulture = NormalizeCulture(segments[0]);
        if (segments[0] is not ("tr" or "en"))
        {
            return $"/{normalizedTargetCulture}{suffix}";
        }

        if (segments.Length == 1)
        {
            return $"/{normalizedTargetCulture}{suffix}";
        }

        var mappedSegments = segments.ToArray();
        mappedSegments[0] = normalizedTargetCulture;

        if (sourceCulture == normalizedTargetCulture)
        {
            return "/" + string.Join('/', mappedSegments) + suffix;
        }

        var sourceDictionary = sourceCulture == "tr" ? TrToEn : EnToTr;
        if (!sourceDictionary.TryGetValue(segments[1], out var mappedFirstSegment))
        {
            return $"/{normalizedTargetCulture}{suffix}";
        }

        mappedSegments[1] = mappedFirstSegment;
        return "/" + string.Join('/', mappedSegments) + suffix;
    }

    private static string NormalizeCulture(string? culture)
        => string.Equals(culture, "en", StringComparison.OrdinalIgnoreCase) ? "en" : "tr";
}
