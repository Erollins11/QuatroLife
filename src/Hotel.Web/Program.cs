using System.Globalization;
using Hotel.Web;
using Hotel.Web.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMemoryCache();

builder.Services.Configure<ContactOptions>(builder.Configuration.GetSection("Contact"));
builder.Services.Configure<SeoOptions>(builder.Configuration.GetSection("Seo"));
builder.Services.Configure<SeasonOptions>(builder.Configuration.GetSection("Season"));

builder.Services
    .AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(SharedResource));
    });

builder.Services.AddScoped<IDataRepository, JsonDataRepository>();
builder.Services.AddScoped<IHotelContentService, HotelContentService>();
builder.Services.AddSingleton<ILanguageRouteService, LanguageRouteService>();
builder.Services.AddScoped<IBookingUrlResolver, BookingUrlResolver>();
builder.Services.AddScoped<ISubmissionLogService, JsonSubmissionLogService>();

var app = builder.Build();

var supportedCultures = new[]
{
    new CultureInfo("tr"),
    new CultureInfo("en")
};

var requestLocalizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("tr"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

requestLocalizationOptions.RequestCultureProviders =
[
    new RouteDataRequestCultureProvider
    {
        RouteDataStringKey = "culture",
        UIRouteDataStringKey = "culture",
        Options = requestLocalizationOptions
    }
];

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/tr");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseRequestLocalization(requestLocalizationOptions);
app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/tr"));

app.MapControllerRoute("switch-language", "switch-language/{targetCulture?}", new { controller = "Site", action = "SwitchLanguage" });
app.MapControllerRoute("sitemap", "sitemap.xml", new { controller = "Seo", action = "Sitemap" });

app.MapControllerRoute("tr-home", "tr", new { controller = "Site", action = "Home", culture = "tr" });
app.MapControllerRoute("en-home", "en", new { controller = "Site", action = "Home", culture = "en" });

app.MapControllerRoute("tr-accommodation", "tr/konaklama", new { controller = "Site", action = "Accommodation", culture = "tr" });
app.MapControllerRoute("en-accommodation", "en/accommodation", new { controller = "Site", action = "Accommodation", culture = "en" });
app.MapControllerRoute("tr-accommodation-detail", "tr/konaklama/{slug}", new { controller = "Site", action = "AccommodationDetail", culture = "tr" });
app.MapControllerRoute("en-accommodation-detail", "en/accommodation/{slug}", new { controller = "Site", action = "AccommodationDetail", culture = "en" });

app.MapControllerRoute("tr-dining", "tr/yeme-icme", new { controller = "Site", action = "Dining", culture = "tr" });
app.MapControllerRoute("en-dining", "en/dining", new { controller = "Site", action = "Dining", culture = "en" });
app.MapControllerRoute("tr-dining-detail", "tr/yeme-icme/{slug}", new { controller = "Site", action = "DiningDetail", culture = "tr" });
app.MapControllerRoute("en-dining-detail", "en/dining/{slug}", new { controller = "Site", action = "DiningDetail", culture = "en" });

app.MapControllerRoute("tr-experiences", "tr/deneyimler", new { controller = "Site", action = "Experiences", culture = "tr" });
app.MapControllerRoute("en-experiences", "en/experiences", new { controller = "Site", action = "Experiences", culture = "en" });

app.MapControllerRoute("tr-wellness", "tr/wellness", new { controller = "Site", action = "Wellness", culture = "tr" });
app.MapControllerRoute("en-wellness", "en/wellness", new { controller = "Site", action = "Wellness", culture = "en" });

app.MapControllerRoute("tr-offers", "tr/teklifler", new { controller = "Site", action = "Offers", culture = "tr" });
app.MapControllerRoute("en-offers", "en/offers", new { controller = "Site", action = "Offers", culture = "en" });
app.MapControllerRoute("tr-offer-detail", "tr/teklifler/{slug}", new { controller = "Site", action = "OfferDetail", culture = "tr" });
app.MapControllerRoute("en-offer-detail", "en/offers/{slug}", new { controller = "Site", action = "OfferDetail", culture = "en" });

app.MapControllerRoute("tr-events", "tr/etkinlikler", new { controller = "Site", action = "Events", culture = "tr" });
app.MapControllerRoute("en-events", "en/events", new { controller = "Site", action = "Events", culture = "en" });
app.MapControllerRoute("tr-event-submit", "tr/etkinlikler/teklif-iste", new { controller = "Site", action = "SubmitEventInquiry", culture = "tr" });
app.MapControllerRoute("en-event-submit", "en/events/request-proposal", new { controller = "Site", action = "SubmitEventInquiry", culture = "en" });

app.MapControllerRoute("tr-gallery", "tr/galeri", new { controller = "Site", action = "Gallery", culture = "tr" });
app.MapControllerRoute("en-gallery", "en/gallery", new { controller = "Site", action = "Gallery", culture = "en" });

app.MapControllerRoute("tr-contact", "tr/iletisim", new { controller = "Site", action = "Contact", culture = "tr" });
app.MapControllerRoute("en-contact", "en/contact", new { controller = "Site", action = "Contact", culture = "en" });
app.MapControllerRoute("tr-contact-submit", "tr/iletisim/mesaj-gonder", new { controller = "Site", action = "SubmitContactInquiry", culture = "tr" });
app.MapControllerRoute("en-contact-submit", "en/contact/send-message", new { controller = "Site", action = "SubmitContactInquiry", culture = "en" });

app.MapControllerRoute("tr-reservation", "tr/rezervasyon", new { controller = "Site", action = "Reservation", culture = "tr" });
app.MapControllerRoute("en-book", "en/book", new { controller = "Site", action = "Reservation", culture = "en" });

app.MapControllerRoute("tr-kvkk", "tr/kvkk", new { controller = "Site", action = "Kvkk", culture = "tr" });
app.MapControllerRoute("en-kvkk", "en/personal-data", new { controller = "Site", action = "Kvkk", culture = "en" });

app.MapControllerRoute("tr-privacy", "tr/gizlilik", new { controller = "Site", action = "Privacy", culture = "tr" });
app.MapControllerRoute("en-privacy", "en/privacy", new { controller = "Site", action = "Privacy", culture = "en" });

app.MapControllerRoute("tr-cookies", "tr/cerez-politikasi", new { controller = "Site", action = "Cookies", culture = "tr" });
app.MapControllerRoute("en-cookies", "en/cookies", new { controller = "Site", action = "Cookies", culture = "en" });

app.MapControllerRoute("tr-tour", "tr/sanal-tur", new { controller = "Site", action = "Tour", culture = "tr" });
app.MapControllerRoute("en-tour", "en/tour", new { controller = "Site", action = "Tour", culture = "en" });

app.Run();
