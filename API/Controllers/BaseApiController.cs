using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

//ovo u uglastim i nasledjivanje kontrolera se uvek ponavlja i zato cemo da kreiramo bazni kontroler kojeg ce svi ostali da naslede, da  izbegnemo ponavljanje koda
[ApiController]
[Route("api/[controller]")]//controller ce biti zamenjeno prvim delom naziva samog kontrolera /api/users
public class BaseApiController : ControllerBase
{

}