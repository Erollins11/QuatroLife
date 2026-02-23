# AGENTS.md

## Kod Standartlari

- Teknoloji: .NET 8 + ASP.NET Core MVC + Razor Views
- Node.js/npm/yarn bagimliligi yoktur.
- Localization: `IStringLocalizer<SharedResource>` + `.resx`
- Varsayilan kultur: `tr`
- Route tabanli dil yapisi zorunludur (`/tr/...`, `/en/...`).
- Veri kaynagi: `src/Hotel.Web/Data/*.json`
- Servis katmani: `IDataRepository`, `JsonDataRepository`, `IHotelContentService`
- Formlar server-side validation ile calisir, JSON log yazar.
- Telifli gorsel kullanilmaz; sadece placeholder SVG.

## Klasor Yapisi

- `src/Hotel.Web/Controllers`
- `src/Hotel.Web/Services`
- `src/Hotel.Web/ViewModels`
- `src/Hotel.Web/Views`
- `src/Hotel.Web/Data`
- `src/Hotel.Web/Resources`
- `src/Hotel.Web/wwwroot`

## Dogrulama Komutlari

```bash
dotnet restore
dotnet build -c Release
dotnet test
dotnet run --project src/Hotel.Web
```

## SEO ve Icerik Kontrol Listesi

- [ ] Her sayfada title + description
- [ ] canonical + hreflang
- [ ] OpenGraph etiketleri
- [ ] JSON-LD Hotel schema
- [ ] `/sitemap.xml` calisiyor
- [ ] `robots.txt` erisilebilir

## Rezervasyon Davranisi

- Her sayfada sticky rezervasyon CTA gorunmeli.
- `BOOKING_URL` (env) > `Booking:Url` (config) onceligi.
- URL bossa fallback:
  - TR: `/tr/rezervasyon`
  - EN: `/en/book`

## Not

- Build ve runtime dogrulamasi SDK kurulu ortamda yapilmalidir.
