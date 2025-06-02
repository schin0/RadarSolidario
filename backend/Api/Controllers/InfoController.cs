using Core.DTOs;
using Core.Mappers;
using Infra.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InfoController(IInfoRepository infoRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<InfoDto>> Get()
    {
        var info = await infoRepository.GetAsync();

        if (info == null)
            return NotFound();

        return Ok(info.ToDto());
    }
}
