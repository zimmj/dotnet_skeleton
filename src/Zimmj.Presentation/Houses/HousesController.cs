using Microsoft.AspNetCore.Mvc;
using Zimmj.Presentation.Houses.Dto;

namespace Zimmj.Presentation.Houses;

[ApiController]
[Route("api/[controller]")]
public class HousesController : ControllerBase
{
    private readonly ILogger<HousesController> _logger;

    public HousesController(ILogger<HousesController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet( Name = "GetHouses")]
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
}