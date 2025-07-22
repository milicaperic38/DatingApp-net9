using API.DTOs;
using API.Entities;
using API.Interface;

namespace API.Extension;

public static class AppUserExtensions //static smo stavili da bude, tako da ne moramo kreirati instancu da bismo koristili funkcionalnosti ove klase
{
    
    //ovo this bukvalno govori da je ovo extension metoda, ta metoda se poziva kao da je clan AppUser klase, user.ToDto();
    public static UserDto ToDto(this AppUser user, ITokenService tokenService)
    {
        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }
}