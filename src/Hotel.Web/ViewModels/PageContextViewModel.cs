namespace Hotel.Web.ViewModels;

public sealed class PageContextViewModel
{
    public required string Culture { get; init; }
    public required string CurrentPath { get; init; }
    public required string SwitchToTrUrl { get; init; }
    public required string SwitchToEnUrl { get; init; }
    public required string BookingUrl { get; init; }
    public bool IsBookingExternal { get; init; }
    public bool IsBookingConfigured { get; init; }
    public bool ShowSeasonBanner { get; init; }
    public string? SeasonMessage { get; init; }
    public required SeoViewModel Seo { get; init; }
}
