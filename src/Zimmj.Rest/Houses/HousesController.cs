using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zimmj.Application.Houses.Commands.Add;
using Zimmj.Application.Houses.Commands.Get;
using Zimmj.Core.CrossCutting.Search;
using Zimmj.Core.Houses;
using Zimmj.Rest.CrossCutting.ResultExtensions;
using Zimmj.Rest.CrossCutting.Dto;
using Zimmj.Rest.CrossCutting.Enum;
using Zimmj.Rest.CrossCutting.Logging;
using Zimmj.Rest.Houses.Dto;
using Zimmj.Rest.Houses.Enum;
using Zimmj.Rest.Houses.Mappings;

namespace Zimmj.Rest.Houses;

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

    [HttpGet(Name = nameof(FilterHouses))]
    public async Task<ActionResult<SearchAnswerDto<SimpleHouse>>> FilterHouses([FromQuery] int? upperPrice,
        [FromQuery] int? lowerPrice,
        [FromQuery] SortDirectionDto sortDirection = SortDirectionDto.DESC,
        [FromQuery] SortHouseByDto sortBy = SortHouseByDto.Price,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10)
    {
        var timer = Stopwatch.StartNew();
        var result = await _mediator.Send(new FilterHousesCommand(
            new HouseQuery(upperPrice, lowerPrice), new Paginator(skip, take),
            new SortBy<SortHouseBy>(sortBy.ToSortHouseBy(),
                sortDirection.ToSortDirection())));
        timer.Stop();
        var eventLog = new HttpEventBuilder(Request).Ok(timer.ElapsedMilliseconds);
        _logger.LogInformation("{@Event}", eventLog);
        return result.Map(SearchAnswerHouseMapper.FromEntity).ToActionResult();
    }

    [HttpGet("{name}", Name = "GetHouseByName")]
    public async Task<ActionResult<SimpleHouse>> GetHouseByName(string name)
    {
        var timer = Stopwatch.StartNew();
        var result = await _mediator.Send(new GetHouseByNameCommand(name));
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