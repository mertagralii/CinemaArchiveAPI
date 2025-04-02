using CinemaArchiveAPI.Data;
using CinemaArchiveAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaArchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CinemaController(AppDbContext context)
        {
            _context = context;
        }
        // NOT :  HttpGet, HttpPost, HttpPut, HttpDelete gibi Http metotlarının action kısımlarındaki isimlerinin slugfy olmasıın istiyorsan aşağıdaki gibi action koyman gerekiyor.
        // Bu Projenin Program.cs kısmında Slugfy ve önemli notlar bulunmaktadır.

        #region Cinema Operations

        [HttpGet("[action]")]
        public IActionResult CinemaList() // Sinema Listesini Getirme
        {
            var cinemaList = _context.Cinemas.Include(x => x.Director).ToList();
            if (cinemaList.Count == 0)
            {
                return Ok("Hata! Şuanda Sinema Listesine Ulaşılamıyor.");
            }
            return Ok(cinemaList);
        }

        [HttpPost("[action]")] // Sinema Ekleme
        public IActionResult CreateCinema([FromBody] Cinema cinemaModel)
        {
            if (cinemaModel == null)
            {
                return Ok("Hata ! Boş veri gönderemezsiniz.");
            }
            if (cinemaModel.Id != 0)
            {
                return Ok("Hata ! ID değeri girmeyiniz.");
            }
            var selectedDirector = _context.Directors.FirstOrDefault(x => x.Id == cinemaModel.Director.Id);
            if (selectedDirector == null)
            {
                return Ok($"Hata ! {cinemaModel.Director.Id} Seçilen Id'ye ait bir Direktor Bulunmuyor.");
            }
            cinemaModel.Director = selectedDirector;
            var createCinema = _context.Cinemas.Add(cinemaModel);
            if (createCinema == null)
            {
                return Ok("Hata! Ekleme İşlemi Gerçekleşirken bir hata meydana geldi. ");
            }
            _context.SaveChanges();
            return Ok("Başarılı! Sinema Ekleme İşlemi Yapıldı.");
        }

        [HttpPut("[action]")] // Sinema Güncelleme
        public IActionResult UpdateCinema([FromBody] Cinema cinema)
        {
            var selectedDirector = _context.Directors.FirstOrDefault(x => x.Id == cinema.Director.Id);
            if (selectedDirector == null)
            {
                return Ok($"Hata ! {cinema.Director.Id} Seçilen Id'ye ait bir Direktor Bulunmuyor.");
            }
            var selectedCinema = _context.Cinemas.FirstOrDefault(x => x.Id == cinema.Id);
            if (selectedCinema == null)
            {
                return Ok($"Hata!{cinema.Id} ID'li Sinema Bulunamadı.");
            }
            selectedCinema.Title = cinema.Title;
            selectedCinema.Year = cinema.Year;
            selectedCinema.Director = selectedDirector;
            _context.SaveChanges();
            return Ok($"Başarılı !{selectedCinema.Id} ID'li Sinemanın Güncelleme işlemi gerçekleştirildi");
        }

        [HttpDelete("[action]")]
        public IActionResult DeleteCinema(int Id)  // Sinema Silme
        {
            var selectedCinema = _context.Cinemas.FirstOrDefault(x => x.Id == Id);
            if (selectedCinema == null)
            {
                return Ok($"Hata!{Id} ID'li Sinema Bulunamadı.");
            }
            var deleteCinema = _context.Cinemas.Remove(selectedCinema);
            if (deleteCinema == null)
            {
                return Ok("Hata! Silme İşlemi Gerçekleşirken bir hata meydana geldi. ");
            }
            _context.SaveChanges();
            return Ok($"Başarılı !{Id} ID'li Sinema Silme işlemi gerçekleştirildi");
        }
        #endregion

        #region Director Operations

        [HttpGet("[action]")]
        public IActionResult DirectorList() // Direktör Listesini Getirme
        {
            var directorList = _context.Directors.ToList();
            if (directorList.Count == 0)
            {
                return Ok("Hata! Şuanda Direktör Listesine Ulaşılamıyor.");
            }
            return Ok(directorList);
        }

        [HttpPost("[action]")]
        public IActionResult CreateDirector([FromBody]Director director) // Direktör Ekleme
        {
            if (director == null)
            {
                return Ok("Hata ! Boş veri gönderemezsiniz.");
            }
            if (director.Id != 0)
            {
                return Ok("Hata ! ID değeri girmeyiniz.");
            }
            var createDirector = _context.Directors.Add(director);
            if (createDirector == null)
            {
                return Ok("Hata! Ekleme İşlemi Gerçekleşirken bir hata meydana geldi. ");
            }
            _context.SaveChanges();
            return Ok("Başarılı! Direktör Ekleme İşlemi Yapıldı.");
        }

        [HttpPut("[action]")]
        public IActionResult UpdateDirector([FromBody] Director director)  // Direktör Güncelleme
        {
            if (director == null)
            {
                return Ok("Hata ! Boş veri gönderemezsiniz.");
            }
            var selectedDirector = _context.Directors.FirstOrDefault(x => x.Id == director.Id);
            if (selectedDirector == null)
            {
                return Ok($"Hata!{director.Id} ID'li Direktör Bulunamadı.");
            }
            selectedDirector.Name = director.Name;
            selectedDirector.Surname = director.Surname;
            _context.SaveChanges();
            return Ok($"Başarılı ! {selectedDirector.Id} ID'li Direktörün Güncelleme işlemi gerçekleştirildi");
        }

        [HttpDelete("[action]")]
        public IActionResult DeleteDirector(int Id) // Direktör Silme
        {
            var selectedDirector = _context.Directors.FirstOrDefault(x => x.Id == Id);
            if (selectedDirector == null)
            {
                return Ok($"Hata!{Id} ID'li Direktör Bulunamadı.");
            }
            var deleteDirector = _context.Directors.Remove(selectedDirector);
            if (deleteDirector == null)
            {
                return Ok("Hata! Silme İşlemi Gerçekleşirken bir hata meydana geldi. ");
            }
            _context.SaveChanges();
            return Ok($"Başarılı !{Id} ID'li Direktör Silme işlemi gerçekleştirildi");
        }

        #endregion
    }
}
