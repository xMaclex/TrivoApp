using Microsoft.AspNetCore.Mvc;
using UserApi.DTOs;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Route ("api/[Controller]")]
public class UserController : ControllerBase
{
    private readonly UserServices _userService;

    public UserController(UserServices service)
    {
        _userService = service;
    }
    // GET Obtener informacion
    [HttpGet]
    public IActionResult GetUser()
    {
        return Ok(_userService.GetAll());
    }

    //POST Crear Informacion
    [HttpPost]
    public IActionResult CreateUser(CreateUserDto dto)
    {
        var user = _userService.Add(dto);
        return Created("", user);
    }

    //PUT Actualizar informacion
    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id,CreateUserDto dto)
    {
        var user = _userService.Update(id, dto);

        if(user == null)
            return NotFound();

        return Ok(user);
    }
    //DELETE Eliminar informacion 
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var deleted = _userService.Delete(id);
        if(!deleted)
            return NotFound();
        
        return NoContent();
    }
}
