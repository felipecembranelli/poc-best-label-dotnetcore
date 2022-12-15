using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace CarAPIClient.Controllers;

[ApiController]
[Route("[controller]")]
public class CarCompareController : ControllerBase
{
    private readonly IConfiguration _configuration;

    HttpClient client = new();

    private readonly ILogger<CarCompareController> _logger;

    public CarCompareController(ILogger<CarCompareController> logger, IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = logger;
    }

   
    [HttpGet(Name = "GetCarCompare")]
    public async Task<IEnumerable<string>> Compare() 
    {

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

        var carAdvs = await GetAdvantagesAsync(client);

        return carAdvs.ToArray();

    }

    async Task<List<string>> GetAdvantagesAsync(HttpClient client)
    {
        var apiURL = _configuration["API_URL"];

        await using Stream stream = await client.GetStreamAsync(apiURL);


        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};

        var carSpecs = await JsonSerializer.DeserializeAsync<List<CarSpec>>(stream, options);

        // get base car specs to be compared with
        CarSpec baseCar = carSpecs[0];
        carSpecs.RemoveAt(0);

        // compare with competitors

        return CarSpecProcessor.CompareCarFeatures(baseCar, carSpecs);
    
    
    }
}
