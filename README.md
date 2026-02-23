# [OTEL_ADI] - Bodrum Hotel Website (.NET 8 MVC)

Premium hissi veren, SSR odakli, TR/EN route-localized ve SEO uyumlu ASP.NET Core MVC projesi.

## Teknoloji

- .NET 8
- ASP.NET Core MVC + Razor Views (SSR)
- Bootstrap 5 (CDN) + custom CSS (`src/Hotel.Web/wwwroot/css/site.css`)
- IStringLocalizer + `.resx` localization
- JSON tabanli icerik (rooms/restaurants/offers/gallery)

## Proje Yapisi

- `44Life.sln`
- `src/Hotel.Web`
  - `Controllers/`
  - `Services/`
  - `ViewModels/`
  - `Views/`
  - `Data/`
  - `Resources/`
  - `wwwroot/`

## Kurulum ve Calistirma

1. SDK kontrol (yerelde):

```bash
dotnet --version
```

2. Restore:

```bash
dotnet restore
```

3. Build (Release):

```bash
dotnet build -c Release
```

4. Calistirma:

```bash
dotnet run --project src/Hotel.Web
```

5. Test (varsa):

```bash
dotnet test
```

## Publish

```bash
dotnet publish -c Release -o ./publish
```

## Konfigurasyon

`src/Hotel.Web/appsettings.json`:

- `Booking:Url`
- `Contact:Phone`
- `Contact:WhatsApp`
- `Contact:Email`
- `Contact:Address`
- `Seo:SiteName`
- `Seo:DefaultDescriptionKey`
- `Season:Enabled`
- `Season:MessageKey`

Environment variable override:

- `BOOKING_URL` -> `Booking:Url` degerini override eder.

BOOKING_URL bos ise butonlar `/{culture}/rezervasyon` veya `/{culture}/book` fallback sayfasina yonlenir.

## Dil ve Route Yapisi

Varsayilan dil `tr`.

- `/` -> `/tr` redirect
- TR: `/tr/konaklama`, `/tr/yeme-icme`, `/tr/deneyimler`, `/tr/wellness`, `/tr/teklifler`, `/tr/etkinlikler`, `/tr/galeri`, `/tr/iletisim`
- EN: `/en/accommodation`, `/en/dining`, `/en/experiences`, `/en/wellness`, `/en/offers`, `/en/events`, `/en/gallery`, `/en/contact`

Ek:

- Legal: `/tr/kvkk`, `/tr/gizlilik`, `/tr/cerez-politikasi`, `/en/personal-data`, `/en/privacy`, `/en/cookies`
- Reservation fallback: `/tr/rezervasyon`, `/en/book`
- Phase 2 prep: `/tr/sanal-tur`, `/en/tour`

## SEO

- Page bazli title/description/canonical
- `hreflang` (TR/EN)
- OpenGraph
- JSON-LD (`Hotel` schema)
- `sitemap.xml` endpoint (`/sitemap.xml`)
- `robots.txt` (`src/Hotel.Web/wwwroot/robots.txt`)

## Formlar

- Etkinlik formu: server-side validation + JSONL log
- Iletisim formu: server-side validation + JSONL log
- Log konumu: `src/Hotel.Web/App_Data/Submissions`

## Not

Bu calisma ortaminda .NET SDK kurulu olmadigi icin `dotnet restore/build/run/test` komutlari burada calistirilamadi. Proje dosyalari SDK kurulu bir makinede dogrulanmalidir.
