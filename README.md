#  Borç Takip Sistemi (C# & .NET Core Web API)

Bu proje, C# Uzmanlık Belgesi alabilmek amacıyla geliştirilmiş; bireysel veya kurumsal borç/alacak kayıtlarını güvenli ve performanslı bir şekilde yönetmeyi sağlayan bir **Web API** uygulamasıdır.

## Canlı Uygulama Linki
Proje yerel bilgisayar dışında, canlı sunucu ortamında (IIS / Plesk) yayına alınmıştır ve SSL sertifikası (HTTPS) ile korunmaktadır:
 **[https://borctakip.site](https://borctakip.site)**

##  Kullanılan Teknolojiler & Mimari
* **Backend:** C# / .NET Core 8.0  Web API
* **Veri Tabanı ORM:** Entity Framework Core (Code-First)
* **Sorgulama Teknolojisi:** LINQ (Filtreleme, Sıralama ve Sayfalama optimizasyonları için)
* **Güvenlik:** Şifrelerin veri tabanında Hash'lenerek saklanması ve Let's Encrypt SSL mekanizması
* **Sunucu & Dağıtım:** Windows Server / IIS / Plesk Panel DevOps Yönetimi

##  Proje Özellikleri & Senaryo
1. **Kullanıcı Yönetimi:** Sisteme kayıt olma ve güvenli giriş (Authentication) mekanizması.
2. **Borç Yönetimi:** Borçlu ekleme, silme, güncelleme ve detaylı listeleme.
3. **Veri Validasyonu:** Hatalı veya eksi değerlerin girişini engelleyen akıllı doğrulama (Validation) katmanı.
