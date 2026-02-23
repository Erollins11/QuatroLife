namespace Hotel.Web.Services;

public interface IHotelContentService
{
    Task<IReadOnlyList<RoomItem>> GetRoomsAsync(CancellationToken cancellationToken = default);
    Task<RoomItem?> GetRoomBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RestaurantItem>> GetRestaurantsAsync(CancellationToken cancellationToken = default);
    Task<RestaurantItem?> GetRestaurantBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OfferItem>> GetOffersAsync(CancellationToken cancellationToken = default);
    Task<OfferItem?> GetOfferBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<GalleryItem>> GetGalleryItemsAsync(CancellationToken cancellationToken = default);
}
