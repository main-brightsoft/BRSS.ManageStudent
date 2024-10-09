using BRSS.ManageStudent.Application.Interface.Base;
using Microsoft.AspNetCore.Mvc;

namespace BRSS.ManageStudent.Controllers.Base;

[Route("api/[controller]")]
[ApiController]
public abstract class ReadOnlyController<TEntityDTO, TKey> : ControllerBase where TEntityDTO : class
{
    private readonly IReadOnlyService<TEntityDTO, TKey> _service;

    protected ReadOnlyController(IReadOnlyService<TEntityDTO, TKey> service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(TKey id)
    {
        var result = await _service.GetAsync(id);
        return Ok(result);
    }
}