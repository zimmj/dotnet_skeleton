using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zimmj.Application.Houses.Commands.Add;
using Zimmj.Application.Houses.Commands.Get;
using Zimmj.Presentation.CrossCutting.Logging;
using Zimmj.Presentation.CrossCutting.ResultExtensions;
using Zimmj.Presentation.Houses.Dto;

namespace Zimmj.Presentation.Houses;

[ApiController]
[Route("api/[controller]")]
public class HousesController : ControllerBase
{
    private readonly ILogger<HousesController> _logger;
    private readonly IMediator _mediator;

    public HousesController(
        ILogger<HousesController> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet(Name = "GetHouses")]
    public ActionResult<IEnumerable<SimpleHouse>> Get()
    {
        return Ok(new List<SimpleHouse>
        {
            new SimpleHouse
            {
                Name = "House 1",
                Price = 100000
            },
            new SimpleHouse
            {
                Name = "House 2",
                Price = 200000
            }
        });
    }

    [HttpGet("{name}", Name = "GetHouseByName")]
    public async Task<ActionResult<SimpleHouse>> GetHouseByName(string name)
    {
        var timer = Stopwatch.StartNew();
        var result = await _mediator.Send(new GetHouseByNameCommand(name);
        timer.Stop();
        var eventLog = new HttpEventBuilder(Request).Ok(timer.ElapsedMilliseconds);
        _logger.LogInformation("{@Event}", eventLog);
        return result.Map(SimpleHouse.FromEntity).ToActionResult();
    }

    [HttpPost(Name = nameof(AddHouse))]
    [ProducesResponseType(typeof(EmptyResult), StatusCodes.Status201Created)]
    public async Task<ActionResult> AddHouse([FromBody] AddHouse addHouse)
    {
        var timer = Stopwatch.StartNew();
        var result = await _mediator.Send(new AddHouseCommand(addHouse.ToEntity()));
        timer.Stop();
        var eventLog = new HttpEventBuilder(Request).Created(timer.ElapsedMilliseconds);
        _logger.LogInformation("{@Event}", eventLog);
        return result.ToActionResult(nameof(GetHouseByName), "Houses", new
        {
            name = addHouse.Name
        });
    }
}