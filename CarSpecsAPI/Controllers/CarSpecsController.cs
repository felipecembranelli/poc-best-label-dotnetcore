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

        const int NUM_TOTAL_FEATURES = 100;
        const int NUM_CARS = 5;

        List<CarSpec> carSpecList = new List<CarSpec>();

        for (int index = 1; index < NUM_CARS; index++)
        {
            // generate new car specs
            var baseCar = new CarSpec {
                CarId = index.ToString(),
                CarName = "Car" + index.ToString(),
                SatelliteNavigation = GetRandomValue(),
                LeatherSeats = GetRandomValue(),
                HeatedFrontSeats = GetRandomValue(),
                BluetoothHandsfreeSystem = GetRandomValue(),
                CruiseControl = GetRandomValue(),
                AutomaticHeadlights = GetRandomValue()

            };
            
            // set values for dummy features
            for (int i = 1; i <= NUM_TOTAL_FEATURES; i++)
            {
                var featureName = "feature"+ i.ToString();

                // get base car feature value
                var nameOfProperty = featureName;
                var propertyInfo = baseCar.GetType().GetProperty(nameOfProperty);
                
                var baseCarFeatureValue = GetRandomValue();

                propertyInfo.SetValue(baseCar, baseCarFeatureValue);
            }

            carSpecList.Add(baseCar);
        }

        return carSpecList.ToArray();


    }

    static string GetRandomValue() 
    {

        List<string> list =  new List<string> {"Standard", "Optional", "Not Available"};

        Random rnd = new Random();

        int r = rnd.Next(list.Count);

        return (string)list[r];
    }
}



