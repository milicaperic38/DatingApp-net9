using System.Net;
using System.Text.Json;
using SQLitePCL;

namespace API.Middleware;


//predstavlja centralizovano hvatanje greske
//next=predstavlja sledeci middleware u pajplajnu, kada zavrsi poziva await next da prosledi zahtev dalje
//logger za logovanje greske recimo u konzolu
// IHostEnviroment omogucava da proverimo da li smo u development, staging ili production okruzenju
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    //ovu metodu ASP.NET Core automatski pozica za svaki http zahtev 
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context); //cilj nam je da hendlamo errore ako je sve okej to saljemo dalje, ako nema sledeci middleware u pajplajnu salje se u kontroler na api koji treba
        }
        catch (System.Exception e)
        {
            logger.LogError(e, e.Message); // ovo ce da loguje u terminalu
            context.Response.ContentType = "application/json"; // odgovor ce biti u json formatu
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //postavljamo status code na 500 sto znaci da je greska na serveru

            var response = env.IsDevelopment() // proverava da li smo u developmet rezimu
                ? new ApiException(context.Response.StatusCode, e.Message, e.StackTrace) // e.StackTrace ce prikazati u kom tacno delu koda se desila greska
                : new ApiException(context.Response.StatusCode, e.Message, "Internal server error"); // u production rezimu skrivamo gde se desila greska da neko 
                                                                                                     //ne bi otkrio arhitekturu nase aplikacije

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // detail, message pisace tako, nece Message...
            };

            var json = JsonSerializer.Serialize(response, options); // ovde response je objekat koji pretvaramo u json, i pomocu options formatiramo
            await context.Response.WriteAsync(json); //saljemo klijentu odgovor
        }
    }
}