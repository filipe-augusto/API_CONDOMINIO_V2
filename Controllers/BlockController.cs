using Microsoft.AspNetCore.Mvc;
using API_CONDOMINIO_2.Data;
using API_CONDOMINIO_2.Extensions;
using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_2.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using API_CONDOMINIO_V2.Repositories.Contracts;
namespace API_CONDOMINIO_2.Controllers;

    [ApiController]
[Authorize]
public class BlockController : Controller
    {

    private readonly DataContext _context;
    private readonly IMemoryCache _cache;
    private readonly IBlockRepository _blockRepository;


    public BlockController(DataContext context, IMemoryCache cache, IBlockRepository blockRepository)
    {
        _context = context;
        _cache = cache;
        _blockRepository = blockRepository;
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
        public async Task<IActionResult> GetAllBlocks()
        {

            try
            {
                var blocks =  await _blockRepository.GetAllBlocksAsync();
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
            var block = await _blockRepository.GetBlockByIdAsync(id);
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


            await _blockRepository.AddBlockAsync(block);

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
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var existingBlock = await _blockRepository.GetBlockByIdAsync(id);
            if (existingBlock == null)
                return NotFound(new ResultViewModel<string>("Block não encontrado"));

            existingBlock.NameBlock = model.NameBlock;
            existingBlock.QuantityFloor = model.QuantityFloor;
            existingBlock.QuantityeUnit = model.QuantityeUnit;

            await _blockRepository.UpdateBlockAsync(existingBlock);

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

