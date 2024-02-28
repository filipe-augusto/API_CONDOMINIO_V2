using API_CONDOMINIO_2.Data;
using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_2.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
namespace API_CONDOMINIO_2.Controllers;

[ApiController]
    public class RolesController : Controller
    {



    [HttpGet("v1/roles")]
    public async Task<IActionResult> GetAsyncCache(
      [FromServices] IMemoryCache cache,
      [FromServices] DataContext context)
    {
        try
        {
            var roles = cache.GetOrCreate("RolesCache", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return GetRoles(context);
            });
            return Ok(new ResultViewModel<List<Role>>(roles));
        }
        catch(Exception ex)
        {
            return StatusCode(500, new ResultViewModel<List<Role>>("05X04 - Falha interna no servidor"));
        }
    }
    private List<Role> GetRoles(DataContext context) => context.Role.ToList();


}

