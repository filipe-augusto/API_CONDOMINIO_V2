using API_CONDOMINIO_2.Data;
using API_CONDOMINIO_2.Extensions;
using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_2.Services;
using API_CONDOMINIO_2.ViewModel;
using API_CONDOMINIO_V2.Repositories;
using API_CONDOMINIO_V2.Repositories.Contracts;
using API_CONDOMINIO_V2.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SecureIdentity.Password;
namespace API_CONDOMINIO_2.Controllers;

[ApiController]
public class AccountController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ITokenService _tokenService;


    public AccountController(IUserRepository userRepository, IAccountRepository accountRepository ,ITokenService tokenService)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _tokenService = tokenService;
    }
    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = await _accountRepository.GetUser(model.Email);
  
        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

 
        if (!_accountRepository.CheckPassWord(user.PasswordHash,model.Password))
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

        try
        {
            var token = _tokenService.GenerateToken(user);
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

    //[Authorize(Roles = "admin")]
    [HttpPost("v1/accounts/")]
    public async Task<IActionResult> Post(
    [FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
        try
        {
            var passWord = await _userRepository.AddUserAsync(model);
                return Ok(new ResultViewModel<dynamic>(new
                {
                    user = model.Email,
                    passWord = passWord

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
     [FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = await _userRepository.GetUserByIdAsync(IdUser);
        if (user == null)
            return BadRequest(new ResultViewModel<User>(ModelState.GetErrors()));

        user.Name = model.Name;
        user.Email = model.Email;
        user.Image = model.Image??user.Image;
        try
        {
           await _userRepository.UpdateUserAsync(user);
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
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {

        try
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return BadRequest(new ResultViewModel<User>("Conteúdo não encontrado."));

            await _userRepository.DeleteUserAsync(user);

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
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }
        catch 
        {
            return StatusCode(500, "05X04 - Falha interna no servidor");
        }
    }

    [Authorize(Roles = "admin")]
    [HttpGet("v1/accounts/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                NotFound(("Couteúdo não encontrado."));
            return Ok(user);
        }
        catch
        {
            return StatusCode(500, "05X04 - Falha interna no servidor");
        }
    }


    //[AllowAnonymous]
    //[HttpPost("v1/login")] // DESATIVADO 
    //public IActionResult Login()
    //{

    //    var token = _tokenService.GenerateToken(null);

    //    return Ok(token);
    //}

}

