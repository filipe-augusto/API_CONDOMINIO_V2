using API_CONDOMINIO_2.Extensions;
using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_2.ViewModel;
using API_CONDOMINIO_V2.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
namespace API_CONDOMINIO_2.Controllers;
[Authorize]
[ApiController]
    public class ResidentsController : Controller
    {
    
    private readonly IMemoryCache _cache;
    private readonly IResidentRepository _residentRepository;

    public ResidentsController( IMemoryCache cache, IResidentRepository residentRepository)
    {
        _residentRepository = residentRepository;
        _cache = cache;
    }

    [HttpGet("v1/residents")]
    public async Task<IActionResult> GetAsyncCache()
    {
        try
        {
            var residents = await _cache.GetOrCreate("residentCache", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return await _residentRepository.GetAllResidentsAsync();
            });
            return Ok(new ResultViewModel<List<Resident>>(residents));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Resident>>("05X04 - Falha interna no servidor"));
        }
    }

    [Authorize(Roles = "admin")]
        [HttpGet("v2/residents/")]
        public async Task<IActionResult> Get()
        {

            try
            {
                var blocks = await _residentRepository.GetAllResidentsWithDetailsAsync();
                return Ok(blocks);
            }
            catch
            {
                return StatusCode(500, "05x04 - Falha Interna no serivodor");
            }
        }

    [Authorize(Roles = "admin")]
    [HttpGet("v1/residents/{id:int}")]
    public async Task<IActionResult> GetId(
        [FromRoute] int id)
    {
        try
        {
            var block = await _residentRepository.GetResidentByIdWithDetailsAsync(id);
            if (block == null)
                NotFound("Conteúdo não encontrado");
            return Ok(block);
        }
        catch 
        {
            return StatusCode(500, "05x04 - Falha interna no servidor");
        }
    }

    [Authorize(Roles ="admin")]
    [HttpPost("v1/residents/")]
    public async Task<IActionResult> Post([FromBody] ResidentViewModel model)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var unit =  await _residentRepository.GetUnitByIdAsync(model.UnitId);
            if ( unit is null)
                return StatusCode(400, new ResultViewModel<string>("05X97 - Unit does not exist"));

            var sex = await _residentRepository.GetSexByIdAsync(model.SexId); 
            if (sex is null)
                return StatusCode(400, new ResultViewModel<string>("05X97 - Sex does not exist"));

            var resident = new Resident
            {
                Name = model.Name,
                Email = model.Email,
                Excluded = model.Excluded,
                Defaulter = model.Defaulter,
                Phone = model.Phone,
                SexId = model.SexId,
                Unit = unit,
                Observation = model.Observation,
                DisabledPerson = model.DisabledPerson,
                Responsible = model.Responsible
                
                
            };
            await _residentRepository.AddResidentAsync(resident);
   
            return Ok(new ResultViewModel<dynamic>(new
            {
                Nome = model.Name,
                Telefone = model.Phone,
                Email = model.Email,
             
                Responsavel = model.Responsible,
                
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

    [Authorize(Roles = "admin")]
    [HttpPut("v1/residents/{id:int}")]
    public async Task<IActionResult> Put([FromBody] ResidentViewModel model,
        [FromRoute] int id)
    {
        try {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
           var unit = await _residentRepository.GetUnitByIdAsync(model.UnitId);
            if (unit is null)
                return StatusCode(400, new ResultViewModel<string>("05X97 - Unit does not exist"));

            var sex = await _residentRepository.GetSexByIdAsync(model.SexId);
            if (sex is null)
                return StatusCode(400, new ResultViewModel<string>("05X97 - Sex does not exist"));

            var resident = await _residentRepository.GetResidentByIdAsync(id);
            if (resident == null)
                return BadRequest(new ResultViewModel<Unit>(ModelState.GetErrors()));

            resident.Name = model.Name;
            resident.Phone = model.Phone;
            resident.Email = model.Email;
            resident.Observation = model.Observation;
            resident.Defaulter = model.Defaulter;
            resident.DisabledPerson = model.DisabledPerson;
            resident.Responsible    = model.Responsible;
            resident.SexId = model.SexId;

          await  _residentRepository.UpdateResidentAsync(resident);

            return Ok(new ResultViewModel<dynamic>(new
            {
                Nome = model.Name,
                Observacao = model.Observation,
                Sexo = resident.Sex.Name,
                Unidade = resident.Unit.NumberUnit.ToString(),
            }));

        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("05X99 - Erro para atualizar os dados do morador"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }
    }

    [Authorize(Roles = "admin")]
    [HttpPost("v1/residents/{id:int}")]
    public async Task<IActionResult> DeleteAsync( [FromRoute] int id)
    {
        try
        {
            var resident = await _residentRepository.GetResidentByIdAsync(id);
            if (resident == null)
                return BadRequest(new ResultViewModel<string>("Conteúdo não encontrado"));

            await _residentRepository.DeleteResidentAsync(resident);
  

            return Created($"v1/residents/{resident.Id}", resident);
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

