using API.Controllers;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;


public class BuggyController(DataContext context) : BaseApiController
{
    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetAuth()
    {
        return "secret text";
    }

    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        var thing = context.Users.Find(-1); // trazimo usera sa id-jem -1
        if (thing == null) return NotFound();
        return thing;
    }

    [HttpGet("server-error")]
    public ActionResult<AppUser> GetServerError()
    {
      
        var thing = context.Users.Find(-1) ?? throw new Exception("A bad thing has happened");// trazimo usera sa id-jem -1, pokusavamo da napavimo akciju sa necim sto je null, dobijamo null reference exception
        return thing;
       
    }

    [HttpGet("bad-request")] //400 i nesto tip greske
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("This was not a good request");
    }



}