using System.Net.Http.Headers;
using System.Text.Json;
using System.Linq;
using System.Diagnostics;
using HttpClient client = new();



client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("text/plain"));

for (int i = 0; i < 50; i++)
{
    Stopwatch timer = new Stopwatch();

    timer.Start();

    //Console.WriteLine("Program Start - " + timer.Elapsed.ToString());

    var carAdvs = await GetAdvantagesAsync(client);

    Console.WriteLine("Process finished - " + timer.Elapsed.ToString() + " Milliseconds = " + timer.Elapsed.TotalMilliseconds.ToString() + "Total advs: "+ carAdvs.Count.ToString());

    timer.Stop();

    /* Console.WriteLine("======== Best features ==========");

    foreach (var adv in carAdvs)
    {
        Console.WriteLine($"{adv}");
    } */
   
}



static async Task<List<string>> GetAdvantagesAsync(HttpClient client)
{
    await using Stream stream =
        await client.GetStreamAsync("http://localhost:7131/CarSpecs");


    var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

    var carSpecs =
        await JsonSerializer.DeserializeAsync<List<CarSpec>>(stream, options);

    // get base car specs to be compared with
    CarSpec baseCar = carSpecs[0];
    carSpecs.RemoveAt(0);

     // compare with competitors

    return CarSpecProcessor.CompareCarFeatures(baseCar, carSpecs);
 
  
}