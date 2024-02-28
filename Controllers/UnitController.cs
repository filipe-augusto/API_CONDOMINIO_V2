using API_CONDOMINIO_2.Data;
using API_CONDOMINIO_2.Extensions;
using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_2.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

[ApiController]
[Authorize]
public class UnitController : Controller
{



    [HttpGet("v1/units")]
    public async Task<IActionResult> GetAsyncCache(
      [FromServices] IMemoryCache cache,
      [FromServices] DataContext context)
    {
        try
        {
            var units = cache.GetOrCreate("UnitCache", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return GetUnits(context);
            });
            return Ok(new ResultViewModel<List<Unit>>(units));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Role>>("05X04 - Falha interna no servidor"));
        }
    }
    private List<Unit> GetUnits(DataContext context) => context.Units.ToList();

    [Authorize(Roles = "admin")]
    [HttpGet("v2/units/")]
    public async Task<IActionResult> Get([FromServices] DataContext context)
    {

        try
        {
            var units =await context.Units.ToListAsync();
            return Ok(units);
        }
        catch
        {
            return StatusCode(500, "05x04 - Falha Interna no serivodor");
        }
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("v1/units/{id:int}")]
    public async Task<IActionResult> GetById(
        [FromServices] DataContext context,
        [FromRoute] int id)
    {
        try
        {
            var unit = await context.Units.FirstOrDefaultAsync(x => x.Id == id);
            if (unit == null)
                NotFound("Conteúdo não encontrado");
            return Ok(unit);

        }
        catch
        {
            return StatusCode(500, "05x04 - Falha Interna no serivodor");
        }
    }
    
    [Authorize(Roles = "admin")]
    [HttpPost("v1/units/")]
    public async Task<ActionResult> Post([FromServices] DataContext context,[FromBody] UnitViewModel model)
    {
        try
        {

            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            var block = context.Blocks.FirstOrDefault(x => x.Id == model.BlockId);
            if (block is null)
                return StatusCode(400, new ResultViewModel<string>("05X95 - Invalid unit"));
            
            var unit = new Unit
            {
                NumberUnit = model.NumberUnit,
                Block = block,
                PeopleLiving = model.PeopleLiving,
                Observation = model.Observation,
                HasGarage = model.HasGarage
            };

            await context.Units.AddAsync(unit);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                unidade = model.NumberUnit,
                unidade_ocupada = model.PeopleLiving,
                tem_garagem = model.HasGarage,
                observacao = model.Observation,
            }));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("05X99 - Erro para criar o bloco"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }
    }



    [Authorize(Roles = "admin")]
    [HttpPut("v1/units/{id:int}")]
    public async Task<IActionResult> Put([FromServices] DataContext context, 
        [FromBody] UnitViewModel model, [FromRoute] int id)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var unit = await context.Units.FirstOrDefaultAsync(x => x.Id == id);
            if (unit == null)
                return BadRequest(new ResultViewModel<Unit>(ModelState.GetErrors()));

            unit.Observation = model.Observation;
            unit.HasGarage = model.HasGarage;
            unit.PeopleLiving = model.PeopleLiving;
            unit.NumberUnit = model.NumberUnit;

            context.Units.Update(unit);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                unidade = model.NumberUnit,
                unidade_ocupada = model.PeopleLiving,
                tem_garagem = model.HasGarage,
                observacao = model.Observation,
            }));

        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("05X99 - Este E-mail já está cadastrado"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }
    }


    [Authorize(Roles ="admin")]
    [HttpDelete("v1/units/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromServices] DataContext context, [FromRoute] int id)
    {
        try
        {
            var unit = await context.Units.FirstOrDefaultAsync(x => x.Id == id);
            if (unit == null)
                return BadRequest(new ResultViewModel<string>("Conteúdo não encontrado"));

            context.Units.Remove(unit);
            await context.SaveChangesAsync();

            return Created($"v1/units/{unit.Id}", unit);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "05x13 - Não foi possivel deletar a categoria");
        }
        catch 
        {
            return StatusCode(500, "05x14 - falha interna no servidor");
        }

    }




}

