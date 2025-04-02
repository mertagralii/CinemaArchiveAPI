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
        #region Cinema Operations

        [HttpGet("CinemaList")]
        public IActionResult CinemaList() // Sinema Listesini Getirme
        {
            var cinemaList = _context.Cinemas.Include(x => x.Director).ToList();
            if (cinemaList.Count == 0)
            {
                return Ok("Hata! Şuanda Sinema Listesine Ulaşılamıyor.");
            }
            return Ok(cinemaList);
        }

        [HttpPost("CreateCinema")] // Sinema Ekleme
        public IActionResult CreateCinema([FromBody] Cinema cinemaModel)
        {
            if (cinemaModel == null)
            {
                return Ok("Hata ! Boş veri gönderemezsiniz.");
            }
            var selectedDirector = _context.Directors.FirstOrDefault(x => x.Id == cinemaModel.Director.Id);
            if (selectedDirector == null)
            {
                return Ok($"Hata ! {selectedDirector.Id} Seçilen Id'ye ait bir Direktor Bulunmuyor.");
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

        [HttpPut("UpdateCinema")] // Sinema Güncelleme
        public IActionResult UpdateCinema([FromBody] Cinema cinema)
        {
            var selectedCinema = _context.Cinemas.FirstOrDefault(x => x.Id == cinema.Id);
            if (selectedCinema == null)
            {
                return Ok($"Hata!{cinema.Id} ID'li Sinema Bulunamadı.");
            }
            selectedCinema.Title = cinema.Title;
            selectedCinema.Year = cinema.Year;
            selectedCinema.Director = cinema.Director;
            _context.SaveChanges();
            return Ok($"Başarılı !{selectedCinema.Id} ID'li Sinemanın Güncelleme işlemi gerçekleştirildi");
        }

        [HttpDelete("DeleteCinema")]
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

        [HttpGet("DirectorList")]
        public IActionResult DirectorList()
        {
            var directorList = _context.Directors.ToList();
            if (directorList.Count == 0)
            {
                return Ok("Hata! Şuanda Direktör Listesine Ulaşılamıyor.");
            }
            return Ok(directorList);
        }

        [HttpPost("CreateDirector")]
        public IActionResult CreateDirector([FromBody]Director director) 
        {
            if (director == null)
            {
                return Ok("Hata ! Boş veri gönderemezsiniz.");
            }
            var createDirector = _context.Directors.Add(director);
            if (createDirector == null)
            {
                return Ok("Hata! Ekleme İşlemi Gerçekleşirken bir hata meydana geldi. ");
            }
            _context.SaveChanges();
            return Ok("Başarılı! Direktör Ekleme İşlemi Yapıldı.");
        }

        [HttpPut("UpdateDirector")]
        public IActionResult UpdateDirector([FromBody] Director director) 
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
            return Ok($"Başarılı !{selectedDirector.Id} ID'li Direktörün Güncelleme işlemi gerçekleştirildi");
        }

        [HttpDelete("DeleteDirector")]
        public IActionResult DeleteDirector(int Id)
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
