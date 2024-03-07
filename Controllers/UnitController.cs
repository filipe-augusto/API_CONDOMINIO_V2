using API_CONDOMINIO_2.Data;
using API_CONDOMINIO_2.Extensions;
using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_2.ViewModel;
using API_CONDOMINIO_V2.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

[ApiController]
[Authorize]
public class UnitController : Controller
{

    private readonly IMemoryCache _cache;
    private readonly IUnitRepository _unitRepository;

    public UnitController(IMemoryCache cache, IUnitRepository unitRepository)
    {
        _unitRepository = unitRepository;
        _cache = cache;
    }

    [HttpGet("v1/units")]
    public async Task<IActionResult> GetAsyncCache()
    {
        try
        {
            var units = await _cache.GetOrCreate("UnitCache", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return await _unitRepository.GetAllUnitsAsync();
            });
            return Ok(new ResultViewModel<List<Unit>>(units));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Role>>("05X04 - Falha interna no servidor"));
        }
    }
 

    [Authorize(Roles = "admin")]
    [HttpGet("v2/units/")]
    public async Task<IActionResult> Get()
    {

        try
        {
            var units = await _unitRepository.GetAllUnitsAsync();
            return Ok(units);
        }
        catch
        {
            return StatusCode(500, "05x04 - Falha Interna no serivodor");
        }
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("v1/units/{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var unit = await _unitRepository.GetUnitByIdAsync(id);
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
    public async Task<ActionResult> Post([FromBody] UnitViewModel model)
    {
        try
        {

            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var result = await _unitRepository.AddUnitAsync(model);
            if (result)
                return Ok(new ResultViewModel<dynamic>(new
                {
                    unidade = model.NumberUnit,
                    unidade_ocupada = model.PeopleLiving,
                    tem_garagem = model.HasGarage,
                    observacao = model.Observation,
                }));
            else
                return StatusCode(500, new ResultViewModel<string>("07x556 Failed to add unit"));

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
    public async Task<IActionResult> Put(
        [FromBody] UnitViewModel model, [FromRoute] int id)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var unit = await _unitRepository.GetUnitByIdAsync(id);
            if (unit == null)
                return BadRequest(new ResultViewModel<Unit>(ModelState.GetErrors()));

            unit.Observation = model.Observation;
            unit.HasGarage = model.HasGarage;
            unit.PeopleLiving = model.PeopleLiving;
            unit.NumberUnit = model.NumberUnit;
            await _unitRepository.UpdateUnitAsync(unit);

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

}

