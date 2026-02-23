using Hotel.Web.Services;

namespace Hotel.Web.ViewModels;

public sealed class QuickLinkViewModel
{
    public required string TitleKey { get; init; }
    public required string DescriptionKey { get; init; }
    public required string Url { get; init; }
    public required string Image { get; init; }
}

public sealed class HomePageViewModel
{
    public required PageContextViewModel Page { get; init; }
    public required IReadOnlyList<QuickLinkViewModel> QuickLinks { get; init; }
}

public sealed class AccommodationPageViewModel
{
    public required PageContextViewModel Page { get; init; }
    public required IReadOnlyList<RoomItem> Rooms { get; init; }
}

public sealed class RoomDetailPageViewModel
{
    public required PageContextViewModel Page { get; init; }
    public required RoomItem Room { get; init; }
}

public sealed class DiningPageViewModel
{
    public required PageContextViewModel Page { get; init; }
    public required IReadOnlyList<RestaurantItem> Restaurants { get; init; }
}

public sealed class DiningDetailPageViewModel
{
    public required PageContextViewModel Page { get; init; }
    public required RestaurantItem Restaurant { get; init; }
}

public sealed class OffersPageViewModel
{
    public required PageContextViewModel Page { get; init; }
    public required IReadOnlyList<OfferItem> Offers { get; init; }
}

public sealed class OfferDetailPageViewModel
{
    public required PageContextViewModel Page { get; init; }
    public required OfferItem Offer { get; init; }
}

public sealed class GalleryPageViewModel
{
    public required PageContextViewModel Page { get; init; }
    public required IReadOnlyList<GalleryItem> Items { get; init; }
    public required IReadOnlyList<string> Categories { get; init; }
}

public sealed class StandardPageViewModel
{
    public required PageContextViewModel Page { get; init; }
}

public sealed class ReservationPageViewModel
{
    public required PageContextViewModel Page { get; init; }
    public bool BookingConfigured { get; init; }
}
