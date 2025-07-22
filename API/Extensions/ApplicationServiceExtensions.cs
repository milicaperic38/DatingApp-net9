using API.Data;
using API.Interface;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{

    //extension metode prepoznajemo po ovome this. i uvek primim parametrom pozivamo metodu, a drugi prosledjujemo kao argument
    public static IServiceCollection AddApplicationServices(this IServiceCollection service, IConfiguration config)
    {
        service.AddControllers();
        //registrujemo DataContext u dependency injection
        //govorimo da koristimo SQLite bazu, i appsettings.json trazimo konekcioni s tring koji se zove defaultconnection
        service.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        service.AddCors();
        service.AddScoped<ITokenService, TokenService>();
        return service;
    }
}