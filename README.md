# Borç Takip API & Web Arayüzü

Bu proje, kişisel veya küçük işletme finans kayıtlarını (alınan ve verilen borçlar) güvenli ve modern bir arayüz ile takip etmenizi sağlayan tam yığın (full-stack) bir web uygulamasıdır.

## Mimari ve Teknolojiler

Proje, güncel yazılım mühendisliği prensiplerine uygun olarak Backend ve Frontend katmanlarına ayrılmıştır.

### Backend (Arka Uç)
- **Framework:** .NET 8.0 (ASP.NET Core Web API)
- **Veritabanı:** Entity Framework Core (SQLite)
- **Mimari:** MVC (Model-View-Controller) deseni temel alınarak tasarlanmıştır.
  - `Models/`: Veritabanı tablolarını temsil eden varlık (entity) sınıfları.
  - `Controllers/`: API isteklerini karşılayan ve iş mantığını yürüten sınıflar.
  - `Data/`: Veritabanı bağlantı (DbContext) bağlamı.

### Frontend (Ön Yüz)
- **Teknolojiler:** HTML5, CSS3, Vanilla JavaScript (ES6+ Asenkron Fetch API)
- **Yapı:** 
  - Responsive (Mobil uyumlu) tasarım.
  - Özel CSS değişkenleri (CSS variables) ve modern arayüz (UI) prensipleri.
  - Tüm statik dosyalar `wwwroot/` klasörü altında izole edilmiştir.

## Kurulum ve Çalıştırma

1. Projeyi bilgisayarınıza indirin veya klonlayın.
2. Terminal üzerinden proje dizinine gidin.
3. Uygulamayı ayağa kaldırmak için aşağıdaki komutu çalıştırın:
   ```bash
   dotnet run
   ```
4. Tarayıcınızdan `http://localhost:5000` veya konsolda belirtilen porta giderek uygulamayı kullanmaya başlayabilirsiniz.

## Özellikler

- **Kullanıcı Yönetimi:** Güvenli oturum açma ve yeni hesap oluşturma.
- **Finansal Takip:** "Borç Aldım" ve "Borç Verdim" şeklinde detaylı kayıt tutma.
- **Durum Güncelleme:** Tek tıkla ödenen/alınan borçların statüsünü değiştirme.
- **Dinamik İstatistikler:** Toplam borç, toplam alacak ve net finansal durumu anlık hesaplama.

## Geliştirici Notu
Bu proje temiz kod (clean code) prensipleri, modüler dosya yapısı ve modern web standartları gözetilerek özenle geliştirilmiştir.
