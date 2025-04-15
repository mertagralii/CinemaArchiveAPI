# 🎬 CinemaArchiveAPI

CinemaArchiveAPI, sinema filmlerini ve türlerini yönetmek amacıyla geliştirilmiş bir RESTful Web API projesidir. Bu API ile film ekleme, listeleme, silme, güncelleme ve film türleriyle ilişkili işlemler gerçekleştirilebilir. Proje ASP.NET Core Web API teknolojisi kullanılarak geliştirilmiştir.

## 🚀 Kullanılan Teknolojiler

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- AutoMapper
- Swagger (API dökümantasyonu için)

## 📁 Proje Yapısı

```
CinemaArchiveAPI
│
├── Controllers
│   ├── MoviesController.cs
│   └── GenresController.cs
│
├── Models
│   ├── Movie.cs
│   └── Genre.cs
│
├── Data
│   ├── DataContext.cs
│   └── Seed.cs (örnek veri eklemek için)
│
├── DTOs
│   ├── MovieDTO.cs
│   └── GenreDTO.cs
│
├── Mappings
│   └── AutoMapperProfile.cs
│
├── Program.cs
└── appsettings.json
```

## 🎯 Temel Özellikler

### 🎥 Filmler
- Film listeleme
- Yeni film ekleme
- Film güncelleme
- Film silme
- Film detaylarını görüntüleme

### 🏷️ Türler
- Tür listeleme
- Yeni tür ekleme
- Tür güncelleme
- Tür silme
- Tür detaylarını görüntüleme

## 🔐 Veri Modelleri

### 🎞️ Movie.cs
```csharp
public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public int GenreId { get; set; }
    public Genre Genre { get; set; }
}
```

### 🗂️ Genre.cs
```csharp
public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Movie> Movies { get; set; }
}
```

## 🔄 Örnek API İstekleri

### 📥 Yeni Film Ekleme
```http
POST /api/movies
{
  "title": "Inception",
  "year": 2010,
  "genreId": 1
}
```

### 📤 Film Listeleme
```http
GET /api/movies
```

### ✏️ Film Güncelleme
```http
PUT /api/movies/1
{
  "title": "The Matrix",
  "year": 1999,
  "genreId": 2
}
```

## 📌 Kurulum

1. Projeyi klonla:
```
git clone https://github.com/mertagralii/CinemaArchiveAPI.git
```

2. SQL Server bağlantı ayarlarını `appsettings.json` dosyasından güncelle.
3. Veri tabanı migrasyonlarını çalıştır:
```
dotnet ef database update
```
4. Uygulamayı başlat:
```
dotnet run
```

## 📚 Swagger Dökümantasyonu

Projeyi çalıştırdıktan sonra Swagger arayüzüne [https://localhost:5001/swagger](https://localhost:5001/swagger) adresinden ulaşabilirsin.

---

Geliştirici: **Mert Ağralı** 👨‍💻  
Her türlü öneri ve katkıya açığım! Projeyi beğendiysen ⭐ bırakmayı unutma!

