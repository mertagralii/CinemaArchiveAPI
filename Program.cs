
using CinemaArchiveAPI.Data;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Runtime.ConstrainedExecution;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace CinemaArchiveAPI
{
    // A�a��daki Kod Sat�r� Ne ��e Yarar? (Bu D�n��t�rme ��lemi Yap�yor)
    // RESTful API�lerde kebab-case format� tercih edilir. A�a��daki Kod sat�r� ise URL�deki parametreleri kebab-case format�na �evirir.
    public sealed class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object? value)
        {
            if (value == null) return null;
            string? str = value.ToString();
            if (string.IsNullOrEmpty(str)) return null;
            return Regex.Replace(str, "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
    //B�y�k harfle ba�layan kelimeleri (camelCase veya PascalCase) kebab-case format�na �eviriyor.
    // Regex kullanarak, k���k harften b�y�k harfe ge�ildi�inde araya - ekliyor.
    // Sonu� olarak k���k harfe �eviriyor.

    // �rnekler : 
    // Girdi(Controller Ad�) - ��kt�(D�n��en URL)
    // CinemaController - cinema-controller
    // GetMovieDetails - get-movie-details
    // AddNewUser - add-new-user
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            // A�a��daki Kod Ne ��e Yarar? (Bu D�n��t�r�len URL'leri Controller'e ekliyor Slugify�� t�m rotalara otomatik uygular.)

            // Bu kod sat�r�, API�de kebab-case format�n� kullanabilmemizi sa�lar.
            // �rne�in, bir controller ad� camelCase veya PascalCase format�nda olabilir.
            // Bu kod sat�r� sayesinde, URL�deki parametreler kebab-case format�na �evrilir.
            // Bunu t�m controller rotalar�na uygulamak i�in RouteTokenTransformerConvention kullan�yoruz:

            builder.Services
             .AddControllers(options =>
             {
                 options.Conventions.Add(
                     new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
             })
             .AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; //Gereksiz null JSON de�erlerini gizler.
                 options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // D�ng�sel referans kaynakl� JSON hatalar�n� engeller.
             });

            // Art�k t�m controller endpoint'leri otomatik olarak kebab-case format�na d�n��ecek.
            // SEO uyumlulu�u ve kullan�c� dostu URL�ler elde etmi� olduk.




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
