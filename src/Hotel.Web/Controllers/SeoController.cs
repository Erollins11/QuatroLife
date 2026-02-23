using System.Xml.Linq;
using Hotel.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Web.Controllers;

public sealed class SeoController(
    IHotelContentService hotelContentService,
    ILanguageRouteService languageRouteService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Sitemap(CancellationToken cancellationToken)
    {
        var staticRouteKeys = new[]
        {
            "home",
            "konaklama",
            "yeme-icme",
            "deneyimler",
            "wellness",
            "teklifler",
            "etkinlikler",
            "galeri",
            "iletisim",
            "rezervasyon",
            "kvkk",
            "gizlilik",
            "cerez-politikasi",
            "sanal-tur"
        };

        var urls = new List<string>();
        foreach (var culture in new[] { "tr", "en" })
        {
            urls.AddRange(staticRouteKeys.Select(routeKey => languageRouteService.PathFor(culture, routeKey)));
        }

        var rooms = await hotelContentService.GetRoomsAsync(cancellationToken);
        var restaurants = await hotelContentService.GetRestaurantsAsync(cancellationToken);
        var offers = await hotelContentService.GetOffersAsync(cancellationToken);

        foreach (var culture in new[] { "tr", "en" })
        {
            urls.AddRange(rooms.Select(room => languageRouteService.PathFor(culture, "konaklama", room.Slug)));
            urls.AddRange(restaurants.Select(restaurant => languageRouteService.PathFor(culture, "yeme-icme", restaurant.Slug)));
            urls.AddRange(offers.Select(offer => languageRouteService.PathFor(culture, "teklifler", offer.Slug)));
        }

        var baseUri = Request.Host.HasValue ? $"{Request.Scheme}://{Request.Host.Value}" : "https://localhost";

        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var xml = new XDocument(
            new XElement(ns + "urlset",
                urls.Distinct(StringComparer.OrdinalIgnoreCase).Select(path =>
                    new XElement(ns + "url",
                        new XElement(ns + "loc", $"{baseUri}{path}"),
                        new XElement(ns + "lastmod", DateTime.UtcNow.ToString("yyyy-MM-dd"))
                    ))));

        return Content(xml.ToString(), "application/xml");
    }
}
