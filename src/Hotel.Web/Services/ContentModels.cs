using System.Text.Json.Serialization;

namespace Hotel.Web.Services;

public sealed class RoomItem
{
    public string Slug { get; set; } = string.Empty;
    public string TitleKey { get; set; } = string.Empty;
    public string ShortDescriptionKey { get; set; } = string.Empty;
    public List<string> FeaturesKeys { get; set; } = [];
    public List<string> Images { get; set; } = [];
    public int Capacity { get; set; }
    public int SizeSqm { get; set; }
    public string BedType { get; set; } = string.Empty;
    public string ViewType { get; set; } = string.Empty;
    public bool IsVilla { get; set; }
    public bool HasPrivatePool { get; set; }
}

public sealed class RestaurantItem
{
    public string Slug { get; set; } = string.Empty;
    public string TitleKey { get; set; } = string.Empty;
    public string ShortDescriptionKey { get; set; } = string.Empty;
    public List<string> FeaturesKeys { get; set; } = [];
    public List<string> Images { get; set; } = [];
    public string WorkingHoursKey { get; set; } = string.Empty;
    public string DressCodeKey { get; set; } = string.Empty;
}

public sealed class OfferItem
{
    public string Slug { get; set; } = string.Empty;
    public string TitleKey { get; set; } = string.Empty;
    public string ShortDescriptionKey { get; set; } = string.Empty;
    public List<string> FeaturesKeys { get; set; } = [];
    public List<string> Images { get; set; } = [];
    public string ValidityKey { get; set; } = string.Empty;
}

public sealed class GalleryItem
{
    public string Slug { get; set; } = string.Empty;
    public string TitleKey { get; set; } = string.Empty;
    public string ShortDescriptionKey { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    [JsonPropertyName("isVideoPlaceholder")]
    public bool IsVideoPlaceholder { get; set; }
}
