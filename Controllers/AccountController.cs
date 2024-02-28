using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.Text.RegularExpressions;
using API_CONDOMINIO_2.Services;
using API_CONDOMINIO_2.ViewModel;
using API_CONDOMINIO_2.Data;
using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_2.Extensions;
namespace API_CONDOMINIO_2.Controllers;

[ApiController]
public class AccountController : Controller
{


    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login(
  [FromBody] LoginViewModel model,
  [FromServices] DataContext context,
  [FromServices] TokenService tokenService)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = await context
            .Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

        if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

        try
        {
            var token = tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }
    }

   

    [Authorize(Roles = "user")]
    [HttpGet("v1/user")]
    public IActionResult GetUser() => Ok(User.Identity.Name);

    [Authorize(Roles = "author")]
    [HttpGet("v1/author")]
    public IActionResult GetAuthor() => Ok(User.Identity.Name);

    [Authorize(Roles = "admin")]
    [HttpGet("v1/admin")]
    public IActionResult GetAdmin() => Ok(User.Identity.Name);

    [Authorize(Roles = "admin")]
    //    "user": "admin@filipe.com",
      //  "password": ")9uX%6A4B6{BYgyTRBDDzP7Dd"
    //
    [HttpPost("v1/accounts/")]
    public async Task<IActionResult> Post(
    [FromBody] RegisterViewModel model,
    [FromServices] DataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
        };

        var password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(password);

        try
        {


            var role = await context.Role.FirstOrDefaultAsync(x => x.Id == model.IdRole);

            user.Roles = new List<Role> { role };
            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
                password
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
    [HttpPut("v1/accounts/{IdUser:int}")]
    public async Task<IActionResult> Put([FromRoute] int IdUser,
     [FromBody] RegisterViewModel model,
     [FromServices] DataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));



        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == IdUser);
        if (user == null)
            return BadRequest(new ResultViewModel<User>(ModelState.GetErrors()));


        user.Name = model.Name;
        user.Email = model.Email;
        user.Image = model.Image??user.Image;
        try
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
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
    [HttpDelete("v1/accounts/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id,
    [FromServices] DataContext context)
    {

        try
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return BadRequest(new ResultViewModel<User>("Conteúdo não encontrado."));

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return Created($"v1/accounts/{user.Id}", user);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, "05x13 - Não foi possivel deletar a categoria");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "05x14 - falha interna no servidor");

        }
    }

    [Authorize]
    [HttpGet("v1/accounts/")]
    public async Task<IActionResult> GetAsync([FromServices] DataContext context)
    {
        try
        {
            var users = await context.Users.ToListAsync();

            return Ok(users);

        }
        catch 
        {
            return StatusCode(500, "05X04 - Falha interna no servidor");
        }
    }

    [Authorize(Roles = "admin")]
    [HttpGet("v1/accounts/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id,
    [FromServices] DataContext context)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                NotFound(("Couteúdo não encontrado."));
            return Ok(user);
        }
        catch
        {
            return StatusCode(500, "05X04 - Falha interna no servidor");
        }
    }

    [AllowAnonymous]
    [HttpPost("v1/login")]
    public IActionResult Login([FromServices] TokenService tokenService)
    {

        var token = tokenService.GenerateToken(null);

        return Ok(token);
    }

}

