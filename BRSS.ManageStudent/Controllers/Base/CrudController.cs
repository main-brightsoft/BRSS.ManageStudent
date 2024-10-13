using BRSS.ManageStudent.Application.Interface.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BRSS.ManageStudent.Controllers.Base;
[Route("api/[controller]")]
[ApiController]
public abstract class CrudController<TEntityDTO, TEntityCreateDTO, TEntityUpdateDTO, TKey>: ReadOnlyController<TEntityDTO, TKey> 
    where TEntityDTO : class where TEntityCreateDTO : class where TEntityUpdateDTO : class
{
    private readonly ICrudService<TEntityDTO, TEntityCreateDTO, TEntityUpdateDTO, TKey> _service;
    protected CrudController(ICrudService<TEntityDTO, TEntityCreateDTO, TEntityUpdateDTO, TKey> service) : base(service)
    {
        _service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TEntityCreateDTO createDTO)
    {
        var result = await _service.AddAsync(createDTO);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] TKey id,[FromBody] TEntityUpdateDTO updateDTO)
    {
        var result = await _service.UpdateAsync(id, updateDTO);   
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] TKey id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteMany([FromBody] List<TKey> ids)
    {
        await _service.DeleteManyAsync(ids);
        return Ok();
    }
}