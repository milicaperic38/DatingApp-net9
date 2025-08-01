using System.Text;
using API.Data;
using API.Extensions;
using API.Interface;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//jedna instanca po HTTP zahtevu, kreirace se instanca TokenService klase, kada se zavrsi zahtev instanca se unistava
//znaci u ovom slucaju, kada se korisnik uloguje unistava se instanca servisa, ali to nema veze sa tim koliko token traje
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
//kako da validiramo token

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>(); // prvo smo registrovali middleware da bi http zahtev prvo prolazio kroz njega
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("http://localhost:4200","https://localhost:4200"));

app.UseAuthentication(); // ko si? authentication mora da
app.UseAuthorization(); // pravo pristupa

app.MapControllers();

app.Run(); //ova komanda izvrsava nasu aplikaciju
