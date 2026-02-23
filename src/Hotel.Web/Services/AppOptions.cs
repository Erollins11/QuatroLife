namespace Hotel.Web.Services;

public sealed class ContactOptions
{
    public string Phone { get; set; } = "+90 252 000 00 00";
    public string WhatsApp { get; set; } = "+90 555 000 00 00";
    public string Email { get; set; } = "hello@quatrolife.com";
    public string Address { get; set; } = "Gumbet Mahallesi, Bodrum, Mugla, Turkiye";
}

public sealed class SeoOptions
{
    public string SiteName { get; set; } = "Quatro Life";
    public string DefaultDescriptionKey { get; set; } = "Seo.Default.Description";
}

public sealed class SeasonOptions
{
    public bool Enabled { get; set; }
    public string MessageKey { get; set; } = "Season.Message.Summer2026";
}
