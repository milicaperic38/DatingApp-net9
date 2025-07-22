using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

// [Authorize] //stavili smo ga na nivou klase i znaci da svaki zahtev morace da salje token za  autorizaciju pristupa metodi. U Authorization u headeru zahteva ce biti token
public class UsersController(DataContext context) : BaseApiController
{
    //kada koristimo actionResult mozemo da vratimo HTTP odgovor, mozemo da vratimo notFound, BadRequest..
    //Task se koristi u kombinaciji sa async i task predstavlja asinhron rezultat koji dolazi u buducnosti
    [AllowAnonymous] // ovu anotaciju koristimo kada je globalno authorize pa zelimo da bude bez autorizacije stavimo tu anot
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() //IEnumrable je intefejs koji predstavlja bilo koju kolekciju(list,array,set..)moze se prolaziti kroz elemente
    {
        var users = await context.Users.ToListAsync();//await dodajemo tu gde ce potencijalno doci do blikiranja resursa
        return users;
    }

    // pozeljno bi bilo da koristimo asinhron pristup, ppogotovo kada pristupamo bazi podataka

    ///Da bi mogli da razlikujemo koju metodu gadjamo, moramo izmeniti rutu, jer iako promenimo naziv metode, on nece moci da prepozna koju da pogodi, ako su obe httpGet
    [Authorize]
    [HttpGet("{id:int}")] // /api/users/2 =primer putanje, sa ovim int sto smo dodali, osigurali smo se da parametar id mora biti tipa int. nece proci string
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound(); // 404 not found
        }
        return user;
    }
}
