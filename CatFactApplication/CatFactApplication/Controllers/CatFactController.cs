using CatFactApplication.Models;
using CatFactApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatFactApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class CatFactController(ICatFactService catFactService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CatFactDto>> GetFact()
    {
        return Ok(await catFactService.GetCatFactAsync());
    }
    
}