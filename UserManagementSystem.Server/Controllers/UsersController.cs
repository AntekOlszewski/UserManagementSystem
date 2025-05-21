using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Threading.Tasks;
using UserManagementSystem.Server.CQRS.Commands;
using UserManagementSystem.Server.CQRS.Queries;
using UserManagementSystem.Server.DTOs;
using UserManagementSystem.Server.Exceptions;

namespace UserManagementSystem.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator, IOutputCacheStore outputCache) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IOutputCacheStore _outputCache = outputCache;

    [HttpGet]
    [OutputCache(Duration = 10, Tags = new[] { "UserList" })]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));
            return Ok(result);
        }
        catch(UserNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDTO createUserDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createCommand = new CreateUserCommand(createUserDTO);
            var createdUser = await _mediator.Send(createCommand);

            await _outputCache.EvictByTagAsync("UserList", default);

            return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
        }
        catch (EmailAlreadyExistsException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteUserByIdCommand(id));

        await _outputCache.EvictByTagAsync("UserList", default);

        return Ok();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDTO updateUserDTO)
    {
        var command = new UpdateUserCommand(id, updateUserDTO);

        try
        {
            var result = await _mediator.Send(command);

            await _outputCache.EvictByTagAsync("UserList", default);

            return Ok(result);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (EmailAlreadyExistsException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}
