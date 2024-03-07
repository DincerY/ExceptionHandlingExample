using Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WepApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    [Route("add")]
    public Task<ActionResult> AddUser()
    {
        throw new BusinessException("User name is not contain integer");
    }

    [HttpPost]
    [Route("delete")]
    public Task<ActionResult> DeleteUser()
    {
        throw new ValidationException("User name is not valid");
    }

    [HttpGet]
    [Route("get")]
    public Task<ActionResult> GetUser()
    {
        throw new AuthorizationException("User is not authorized");
    }
}