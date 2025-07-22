using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extension;
using API.Interface;
using API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

//ovo u zagradama su kontruktori i dependency injection
public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] // account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {

        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");
        //sluzi za autoamtsko upravljanje resursima, da ne moramo rucno da oslobadjamo resurse
        using var hmac = new HMACSHA512(); //klasa iz namespacea system.security.cryptography, sluzi da napravi hash kod koristeci tajni kljuc

        var user = new AppUser
        {
            UserName = registerDto.Username,
           // Email = registerDto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), //ovde pretvaramo lozinku u niz bajtova i zatim metoda computehash racuna hes vrednost
            PasswordSalt = hmac.Key // odvde generisemo tajni kljuc
        };
        context.Users.Add(user);
        await context.SaveChangesAsync(); // asinhrona verzija za cuvanje podataka u bazi

        return user.ToDto(tokenService); //iako metoda ima dva parametra ovako se radi
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDTO loginDto)
    {
        // metoda first ir default async ce vratiti prvog usera koji se poklapa po username-u, a ako ga nema vraca null, ostale metode baca exception
        var user = await context.Users.FirstOrDefaultAsync(x =>
            x.UserName == loginDto.Username);
        if (user == null) return Unauthorized("Invalid username"); //unauthorized je 401 korisnik nije autentifikovan ili nije uspeo da se prijavi

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for (int i = 0; i < computeHash.Length; i++)
        {
            if (computeHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid password");
            }
        }

        return user.ToDto(tokenService);
    }

    private async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower()); // Bob =! bob 
    }
}