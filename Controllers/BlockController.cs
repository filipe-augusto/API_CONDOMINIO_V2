using Microsoft.AspNetCore.Mvc;
using API_CONDOMINIO_2.Data;
using API_CONDOMINIO_2.Extensions;
using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_2.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
namespace API_CONDOMINIO_2.Controllers;

    [ApiController]
[Authorize]
public class BlockController : Controller
    {

    private readonly DataContext _context;
    private readonly IMemoryCache _cache;

    public BlockController(DataContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }
    [HttpGet("v1/blocks")]
    public async Task<IActionResult> GetAsyncCache()
    {
        try
        {
            var blocks = _cache.GetOrCreate("BlockCache", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return GetBocks(_context);
            });
            return Ok(new ResultViewModel<List<Block>>(blocks));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Block>>("05X04 - Falha interna no servidor"));
        }
    }
    private List<Block> GetBocks(DataContext context) => context.Blocks.ToList();




    [Authorize(Roles = "admin")]
        [HttpGet("v2/blocks/")]
        public async Task<IActionResult> Get()
        {

            try
            {
                var blocks = await _context.Blocks.ToListAsync();
                return Ok(blocks);
            }
            catch
            {
                return StatusCode(500, "05x04 - Falha Interna no serivodor");
            }
        }


    [Authorize(Roles = "admin")]
    [HttpGet("v1/blocks/{id:int}")]
    public async Task<IActionResult> GetId(
        [FromRoute] int id)
    {
        try
        {
            var block = await _context.Blocks.FirstOrDefaultAsync(y => y.Id == id);
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
    [HttpPost("v1/blocks/")]
    public async Task<IActionResult> Post([FromBody] BlockViewModel model
      )
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var block = new Block
            {
                NameBlock = model.NameBlock,
                QuantityeUnit = model.QuantityeUnit,
                QuantityFloor = model.QuantityFloor,
            };
            await _context.Blocks.AddAsync(block);
            await _context.SaveChangesAsync();


            return Ok(new ResultViewModel<dynamic>(new
            {
                identificacaoBlock = model.NameBlock,
                QuantidadeUnidades = model.QuantityeUnit,
                QuantidadeAndares = model.QuantityFloor,
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
    [HttpPut("v1/blocks/{id:int}")]
    public async Task<IActionResult> Put(
        [FromBody] BlockViewModel model, [FromRoute] int id)
    {
        try {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var block = await _context.Blocks.FirstOrDefaultAsync(x => x.Id == id);
            if (block == null)
                return BadRequest(new ResultViewModel<Unit>(ModelState.GetErrors()));

            block.NameBlock = model.NameBlock;
            block.QuantityFloor = model.QuantityFloor;
            block.QuantityeUnit = model.QuantityeUnit;

            _context.Blocks.Update(block);
            await _context.SaveChangesAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                identificacaoBlock = model.NameBlock,
                QuantidadeUnidades = model.QuantityeUnit,
                QuantidadeAndares = model.QuantityFloor,
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

