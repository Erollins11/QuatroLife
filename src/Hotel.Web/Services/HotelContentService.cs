namespace Hotel.Web.Services;

public sealed class HotelContentService(IDataRepository dataRepository) : IHotelContentService
{
    public Task<IReadOnlyList<RoomItem>> GetRoomsAsync(CancellationToken cancellationToken = default)
        => dataRepository.ReadListAsync<RoomItem>("rooms.json", cancellationToken);

    public async Task<RoomItem?> GetRoomBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var rooms = await GetRoomsAsync(cancellationToken);
        return rooms.FirstOrDefault(item => item.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
    }

    public Task<IReadOnlyList<RestaurantItem>> GetRestaurantsAsync(CancellationToken cancellationToken = default)
        => dataRepository.ReadListAsync<RestaurantItem>("restaurants.json", cancellationToken);

    public async Task<RestaurantItem?> GetRestaurantBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var restaurants = await GetRestaurantsAsync(cancellationToken);
        return restaurants.FirstOrDefault(item => item.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
    }

    public Task<IReadOnlyList<OfferItem>> GetOffersAsync(CancellationToken cancellationToken = default)
        => dataRepository.ReadListAsync<OfferItem>("offers.json", cancellationToken);

    public async Task<OfferItem?> GetOfferBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var offers = await GetOffersAsync(cancellationToken);
        return offers.FirstOrDefault(item => item.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
    }

    public Task<IReadOnlyList<GalleryItem>> GetGalleryItemsAsync(CancellationToken cancellationToken = default)
        => dataRepository.ReadListAsync<GalleryItem>("galleryItems.json", cancellationToken);
}
