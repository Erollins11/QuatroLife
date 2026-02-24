using Hotel.Web.Services;
using Hotel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Hotel.Web.Controllers;

public sealed class SiteController(
    IHotelContentService hotelContentService,
    ILanguageRouteService languageRouteService,
    IBookingUrlResolver bookingUrlResolver,
    ISubmissionLogService submissionLogService,
    IOptions<SeasonOptions> seasonOptions,
    IOptions<SeoOptions> seoOptions,
    IStringLocalizer<SharedResource> localizer) : Controller
{
    [HttpGet]
    public IActionResult SwitchLanguage(string? targetCulture, string? returnUrl)
    {
        var normalizedCulture = NormalizeCulture(targetCulture);
        var mappedPath = languageRouteService.MapPathToCulture(returnUrl ?? "/", normalizedCulture);
        var safePath = Url.IsLocalUrl(mappedPath)
            ? mappedPath
            : languageRouteService.PathFor(normalizedCulture, "home");
        return LocalRedirect(safePath);
    }

    [HttpGet]
    public async Task<IActionResult> Home(string culture, CancellationToken cancellationToken)
    {
        var context = CreateContext(culture, "Seo.Home.Title", "Seo.Home.Description", "/img/hotel/home-hero.jpg");
        var rooms = await hotelContentService.GetRoomsAsync(cancellationToken);
        var offers = await hotelContentService.GetOffersAsync(cancellationToken);

        var model = new HomePageViewModel
        {
            Page = context,
            QuickLinks =
            [
                new QuickLinkViewModel { TitleKey = "Nav.Accommodation", DescriptionKey = "Home.Quick.Accommodation", Url = languageRouteService.PathFor(context.Culture, "konaklama"), Image = "/img/hotel/room-lagoon-suite.jpg" },
                new QuickLinkViewModel { TitleKey = "Nav.Dining", DescriptionKey = "Home.Quick.Dining", Url = languageRouteService.PathFor(context.Culture, "yeme-icme"), Image = "/img/hotel/dining-aura-main.jpg" },
                new QuickLinkViewModel { TitleKey = "Nav.Experiences", DescriptionKey = "Home.Quick.Experiences", Url = languageRouteService.PathFor(context.Culture, "deneyimler"), Image = "/img/hotel/experience-yacht.jpg" },
                new QuickLinkViewModel { TitleKey = "Nav.Wellness", DescriptionKey = "Home.Quick.Wellness", Url = languageRouteService.PathFor(context.Culture, "wellness"), Image = "/img/hotel/wellness-spa.jpg" },
                new QuickLinkViewModel { TitleKey = "Nav.Offers", DescriptionKey = "Home.Quick.Offers", Url = languageRouteService.PathFor(context.Culture, "teklifler"), Image = "/img/hotel/experience-beach-pool.jpg" }
            ],
            RoomRateOptions = rooms
                .OrderBy(item => item.StartingPrice)
                .Select(item => new RoomRateOptionViewModel
                {
                    Slug = item.Slug,
                    TitleKey = item.TitleKey,
                    StartingPrice = item.StartingPrice
                })
                .ToList(),
            FeaturedOffers = offers.Take(2).ToList()
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Accommodation(string culture, CancellationToken cancellationToken)
    {
        var context = CreateContext(culture, "Seo.Accommodation.Title", "Seo.Accommodation.Description", "/img/hotel/room-deluxe.jpg");
        var rooms = await hotelContentService.GetRoomsAsync(cancellationToken);
        return View(new AccommodationPageViewModel { Page = context, Rooms = rooms });
    }

    [HttpGet]
    public async Task<IActionResult> AccommodationDetail(string culture, string slug, CancellationToken cancellationToken)
    {
        var room = await hotelContentService.GetRoomBySlugAsync(slug, cancellationToken);
        if (room is null)
        {
            return NotFound();
        }

        var context = CreateContext(culture, room.TitleKey, room.ShortDescriptionKey, room.Images.FirstOrDefault() ?? "/img/hotel/room-deluxe.jpg");
        return View(new RoomDetailPageViewModel { Page = context, Room = room });
    }

    [HttpGet]
    public async Task<IActionResult> Dining(string culture, CancellationToken cancellationToken)
    {
        var context = CreateContext(culture, "Seo.Dining.Title", "Seo.Dining.Description", "/img/hotel/dining-aura-main.jpg");
        var restaurants = await hotelContentService.GetRestaurantsAsync(cancellationToken);
        return View(new DiningPageViewModel { Page = context, Restaurants = restaurants });
    }

    [HttpGet]
    public async Task<IActionResult> DiningDetail(string culture, string slug, CancellationToken cancellationToken)
    {
        var restaurant = await hotelContentService.GetRestaurantBySlugAsync(slug, cancellationToken);
        if (restaurant is null)
        {
            return NotFound();
        }

        var context = CreateContext(culture, restaurant.TitleKey, restaurant.ShortDescriptionKey, restaurant.Images.FirstOrDefault() ?? "/img/hotel/dining-aura-main.jpg");
        return View(new DiningDetailPageViewModel { Page = context, Restaurant = restaurant });
    }

    [HttpGet]
    public IActionResult Experiences(string culture)
    {
        var context = CreateContext(culture, "Seo.Experiences.Title", "Seo.Experiences.Description", "/img/hotel/experience-yacht.jpg");
        return View(new StandardPageViewModel { Page = context });
    }

    [HttpGet]
    public IActionResult Wellness(string culture)
    {
        var context = CreateContext(culture, "Seo.Wellness.Title", "Seo.Wellness.Description", "/img/hotel/wellness-spa.jpg");
        return View(new StandardPageViewModel { Page = context });
    }

    [HttpGet]
    public async Task<IActionResult> Offers(string culture, CancellationToken cancellationToken)
    {
        var context = CreateContext(culture, "Seo.Offers.Title", "Seo.Offers.Description", "/img/hotel/experience-beach-pool.jpg");
        var offers = await hotelContentService.GetOffersAsync(cancellationToken);
        return View(new OffersPageViewModel { Page = context, Offers = offers });
    }

    [HttpGet]
    public async Task<IActionResult> OfferDetail(string culture, string slug, CancellationToken cancellationToken)
    {
        var offer = await hotelContentService.GetOfferBySlugAsync(slug, cancellationToken);
        if (offer is null)
        {
            return NotFound();
        }

        var context = CreateContext(culture, offer.TitleKey, offer.ShortDescriptionKey, offer.Images.FirstOrDefault() ?? "/img/hotel/experience-beach-pool.jpg");
        return View(new OfferDetailPageViewModel { Page = context, Offer = offer });
    }

    [HttpGet]
    public IActionResult Events(string culture)
    {
        var context = CreateContext(culture, "Seo.Events.Title", "Seo.Events.Description", "/img/hotel/events-wedding.jpg");
        var submitted = TempData["EventsSubmitted"] as string == "1";

        return View(new EventsPageViewModel
        {
            Page = context,
            Form = new EventInquiryInputModel(),
            IsSubmitted = submitted
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitEventInquiry(string culture, EventInquiryInputModel form, CancellationToken cancellationToken)
    {
        var normalizedCulture = NormalizeCulture(culture);
        if (!ModelState.IsValid)
        {
            var pagePath = languageRouteService.PathFor(normalizedCulture, "etkinlikler");
            var context = CreateContext(normalizedCulture, "Seo.Events.Title", "Seo.Events.Description", "/img/hotel/events-wedding.jpg", pagePath);
            return View("Events", new EventsPageViewModel { Page = context, Form = form, IsSubmitted = false });
        }

        await submissionLogService.LogAsync("event-inquiry", normalizedCulture, form, cancellationToken);
        TempData["EventsSubmitted"] = "1";
        return Redirect(languageRouteService.PathFor(normalizedCulture, "etkinlikler"));
    }

    [HttpGet]
    public async Task<IActionResult> Gallery(string culture, CancellationToken cancellationToken)
    {
        var context = CreateContext(culture, "Seo.Gallery.Title", "Seo.Gallery.Description", "/img/hotel/home-hero.jpg");
        var items = await hotelContentService.GetGalleryItemsAsync(cancellationToken);
        var categories = items.Select(item => item.Category).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

        return View(new GalleryPageViewModel
        {
            Page = context,
            Items = items,
            Categories = categories
        });
    }

    [HttpGet]
    public IActionResult Contact(string culture)
    {
        var context = CreateContext(culture, "Seo.Contact.Title", "Seo.Contact.Description", "/img/hotel/contact-location.jpg");
        var submitted = TempData["ContactSubmitted"] as string == "1";

        return View(new ContactPageViewModel
        {
            Page = context,
            Form = new ContactInputModel(),
            IsSubmitted = submitted
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitContactInquiry(string culture, ContactInputModel form, CancellationToken cancellationToken)
    {
        var normalizedCulture = NormalizeCulture(culture);
        if (!ModelState.IsValid)
        {
            var pagePath = languageRouteService.PathFor(normalizedCulture, "iletisim");
            var context = CreateContext(normalizedCulture, "Seo.Contact.Title", "Seo.Contact.Description", "/img/hotel/contact-location.jpg", pagePath);
            return View("Contact", new ContactPageViewModel { Page = context, Form = form, IsSubmitted = false });
        }

        await submissionLogService.LogAsync("contact", normalizedCulture, form, cancellationToken);
        TempData["ContactSubmitted"] = "1";
        return Redirect(languageRouteService.PathFor(normalizedCulture, "iletisim"));
    }

    [HttpGet]
    public IActionResult Reservation(string culture)
    {
        var context = CreateContext(culture, "Seo.Book.Title", "Seo.Book.Description", "/img/hotel/experience-beach-pool.jpg");
        return View(new ReservationPageViewModel
        {
            Page = context,
            BookingConfigured = context.IsBookingConfigured
        });
    }

    [HttpGet]
    public IActionResult Kvkk(string culture)
    {
        var context = CreateContext(culture, "Seo.Kvkk.Title", "Seo.Kvkk.Description");
        return View(new StandardPageViewModel { Page = context });
    }

    [HttpGet]
    public IActionResult Privacy(string culture)
    {
        var context = CreateContext(culture, "Seo.Privacy.Title", "Seo.Privacy.Description");
        return View(new StandardPageViewModel { Page = context });
    }

    [HttpGet]
    public IActionResult Cookies(string culture)
    {
        var context = CreateContext(culture, "Seo.Cookies.Title", "Seo.Cookies.Description");
        return View(new StandardPageViewModel { Page = context });
    }

    [HttpGet]
    public IActionResult Tour(string culture)
    {
        var context = CreateContext(culture, "Seo.Tour.Title", "Seo.Tour.Description", "/img/hotel/experience-yacht.jpg");
        return View(new StandardPageViewModel { Page = context });
    }

    private PageContextViewModel CreateContext(string culture, string titleKey, string descriptionKey, string? ogImagePath = null, string? pathOverride = null)
    {
        var normalizedCulture = NormalizeCulture(culture);
        var currentPath = !string.IsNullOrWhiteSpace(pathOverride)
            ? pathOverride
            : (Request.Path.HasValue
                ? Request.Path.Value!
                : languageRouteService.PathFor(normalizedCulture, "home"));

        var returnUrl = $"{currentPath}{Request.QueryString}";
        var switchToTr = Url.Action(nameof(SwitchLanguage), "Site", new { targetCulture = "tr", returnUrl }) ?? "/tr";
        var switchToEn = Url.Action(nameof(SwitchLanguage), "Site", new { targetCulture = "en", returnUrl }) ?? "/en";

        var booking = bookingUrlResolver.Resolve(normalizedCulture);
        var season = seasonOptions.Value;

        var canonicalUrl = BuildAbsoluteUrl(currentPath);
        var altTr = BuildAbsoluteUrl(languageRouteService.MapPathToCulture(currentPath, "tr"));
        var altEn = BuildAbsoluteUrl(languageRouteService.MapPathToCulture(currentPath, "en"));
        var openGraphImage = BuildAbsoluteUrl(ogImagePath ?? "/img/og/default-og.jpg");

        return new PageContextViewModel
        {
            Culture = normalizedCulture,
            CurrentPath = currentPath,
            SwitchToTrUrl = switchToTr,
            SwitchToEnUrl = switchToEn,
            BookingUrl = booking.Url,
            IsBookingConfigured = booking.IsConfigured,
            IsBookingExternal = booking.IsExternal,
            ShowSeasonBanner = season.Enabled,
            SeasonMessage = season.Enabled && !string.IsNullOrWhiteSpace(season.MessageKey)
                ? localizer[season.MessageKey].Value
                : null,
            Seo = new SeoViewModel
            {
                Title = $"{localizer[titleKey]} | {seoOptions.Value.SiteName}",
                Description = localizer[descriptionKey],
                CanonicalUrl = canonicalUrl,
                AlternateTrUrl = altTr,
                AlternateEnUrl = altEn,
                OpenGraphImageUrl = openGraphImage
            }
        };
    }

    private string BuildAbsoluteUrl(string path)
    {
        var origin = Request.Host.HasValue
            ? $"{Request.Scheme}://{Request.Host.Value}"
            : "https://localhost";

        if (string.IsNullOrWhiteSpace(path))
        {
            return origin;
        }

        return path.StartsWith('/') ? $"{origin}{path}" : $"{origin}/{path}";
    }

    private static string NormalizeCulture(string? culture)
        => string.Equals(culture, "en", StringComparison.OrdinalIgnoreCase) ? "en" : "tr";
}
