namespace Hotel.Web.ViewModels;

public sealed class SeoViewModel
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string CanonicalUrl { get; init; }
    public required string AlternateTrUrl { get; init; }
    public required string AlternateEnUrl { get; init; }
    public required string OpenGraphImageUrl { get; init; }
}
