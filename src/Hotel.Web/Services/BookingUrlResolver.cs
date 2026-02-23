namespace Hotel.Web.Services;

public sealed class BookingUrlResolver(
    IConfiguration configuration,
    ILanguageRouteService languageRouteService) : IBookingUrlResolver
{
    public (string Url, bool IsExternal, bool IsConfigured) Resolve(string culture)
    {
        var envBookingUrl = Environment.GetEnvironmentVariable("BOOKING_URL");
        var configBookingUrl = configuration["Booking:Url"];
        var bookingUrl = string.IsNullOrWhiteSpace(envBookingUrl) ? configBookingUrl : envBookingUrl;

        if (!string.IsNullOrWhiteSpace(bookingUrl))
        {
            return (bookingUrl, Uri.IsWellFormedUriString(bookingUrl, UriKind.Absolute), true);
        }

        var fallback = languageRouteService.PathFor(culture, "rezervasyon");
        return (fallback, false, false);
    }
}
