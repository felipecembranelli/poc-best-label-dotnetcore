using Microsoft.AspNetCore.Mvc;

namespace CarSpecs.Controllers;

[ApiController]
[Route("[controller]")]
public class CarSpecsController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<CarSpecsController> _logger;

    public CarSpecsController(ILogger<CarSpecsController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetCarSpecs")]
    public IEnumerable<CarSpec> Get()
    {
        return Enumerable.Range(1, 4).Select(index => new CarSpec
        {
            CarId = index.ToString(),
            CarName = "Car"+ index.ToString(),
            SatelliteNavigation = "SatelliteNavigation" + index.ToString(),
            LeatherSeats = "LeatherSeats" + index.ToString(),
            HeatedFrontSeats = "HeatedFrontSeats" + index.ToString(),
            BluetoothHandsfreeSystem = "BluetoothHandsfreeSystem" + index.ToString(),
            CruiseControl = "CruiseControl" + index.ToString(),
            AutomaticHeadlights = "AutomaticHeadlights" + index.ToString()
        })
        .ToArray();
    }
}
