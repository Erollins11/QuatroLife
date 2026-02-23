namespace Hotel.Web.Services;

public interface IBookingUrlResolver
{
    (string Url, bool IsExternal, bool IsConfigured) Resolve(string culture);
}
