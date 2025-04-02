
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
    // Aþaðýdaki Kod Satýrý Ne Ýþe Yarar? (Bu Dönüþtürme Ýþlemi Yapýyor)
    // RESTful API’lerde kebab-case formatý tercih edilir. Aþaðýdaki Kod satýrý ise URL’deki parametreleri kebab-case formatýna çevirir.
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
    //Büyük harfle baþlayan kelimeleri (camelCase veya PascalCase) kebab-case formatýna çeviriyor.
    // Regex kullanarak, küçük harften büyük harfe geçildiðinde araya - ekliyor.
    // Sonuç olarak küçük harfe çeviriyor.

    // Örnekler : 
    // Girdi(Controller Adý) - Çýktý(Dönüþen URL)
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


            // Aþaðýdaki Kod Ne Ýþe Yarar? (Bu Dönüþtürülen URL'leri Controller'e ekliyor Slugify’ý tüm rotalara otomatik uygular.)

            // Bu kod satýrý, API’de kebab-case formatýný kullanabilmemizi saðlar.
            // Örneðin, bir controller adý camelCase veya PascalCase formatýnda olabilir.
            // Bu kod satýrý sayesinde, URL’deki parametreler kebab-case formatýna çevrilir.
            // Bunu tüm controller rotalarýna uygulamak için RouteTokenTransformerConvention kullanýyoruz:

            builder.Services
             .AddControllers(options =>
             {
                 options.Conventions.Add(
                     new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
             })
             .AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; //Gereksiz null JSON deðerlerini gizler.
                 options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Döngüsel referans kaynaklý JSON hatalarýný engeller.
             });

            // Artýk tüm controller endpoint'leri otomatik olarak kebab-case formatýna dönüþecek.
            // SEO uyumluluðu ve kullanýcý dostu URL’ler elde etmiþ olduk.




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
