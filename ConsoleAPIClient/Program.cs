using System.Net.Http.Headers;
using System.Text.Json;
using System.Linq;


using HttpClient client = new();

const int NUM_TOTAL_FEATURES = 50;

client.DefaultRequestHeaders.Accept.Clear();
//client.DefaultRequestHeaders.Add("accept", "text/plain");
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("text/plain"));
//client.DefaultRequestHeaders.Accept.Add(
//    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
//client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

//var repositories = await ProcessRepositoriesAsync(client);

var carSpecs = await ProcessCarSpecsAsync(client);

/* foreach (var repo in repositories)
{
    Console.WriteLine($"Name: {repo.Name}");
    Console.WriteLine($"Homepage: {repo.Homepage}");
    Console.WriteLine($"GitHub: {repo.GitHubHomeUrl}");
    Console.WriteLine($"Description: {repo.Description}");
    Console.WriteLine($"Watchers: {repo.Watchers:#,0}");
    Console.WriteLine($"{repo.LastPush}");
    Console.WriteLine();
} */

foreach (var item in carSpecs)
{
    Console.WriteLine($"Name: {item.CarName}");
}

var carAdvs = await GetAdvantagesAsync(client);

foreach (var adv in carAdvs)
{
    Console.WriteLine($"{adv}");
}

static async Task<List<Repository>> ProcessRepositoriesAsync(HttpClient client)
{
    await using Stream stream =
        await client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
    var repositories =
        await JsonSerializer.DeserializeAsync<List<Repository>>(stream);
    return repositories ?? new();
}

static async Task<List<CarSpec>> ProcessCarSpecsAsync(HttpClient client)
{
    await using Stream stream =
        await client.GetStreamAsync("http://localhost:7131/CarSpecs");


    var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

    var carSpecs =
        await JsonSerializer.DeserializeAsync<List<CarSpec>>(stream, options);


    // process to get advantages

  /*   CarSpec baseCar = new CarSpec(carSpecs[0].CarId, 
                    carSpecs[0].CarName,
                    carSpecs[0].SatelliteNavigation, 
                    "", "", "", "", "", "");

    carSpecs.RemoveAt(0);

    CompareCarFeatures(baseCar, carSpecs); */

    return carSpecs ?? new();
   
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


    // process to get advantages

    CarSpec baseCar = new CarSpec(carSpecs[0].CarId, 
                    carSpecs[0].CarName,
                    carSpecs[0].SatelliteNavigation, 
                    "", "", "", "", "", "TESTE");

    carSpecs.RemoveAt(0);

    return CompareCarFeatures(baseCar, carSpecs);
  
}


static List<string> CompareCarFeatures(CarSpec baseCar, List<CarSpec> cars) 
{
  
    List<string> listBestFeatures = new List<string>();

     if (CompareFeature("SatelliteNavigation", baseCar.SatelliteNavigation, (from f in cars select f.SatelliteNavigation).ToList()))
        listBestFeatures.Add("SatelliteNavigation");

    if (CompareFeature("LeatherSeats", baseCar.LeatherSeats, (from f in cars select f.LeatherSeats).ToList()))
        listBestFeatures.Add("LeatherSeats");

    if (CompareFeature("HeatedFrontSeats", baseCar.HeatedFrontSeats, (from f in cars select f.HeatedFrontSeats).ToList()))
        listBestFeatures.Add("HeatedFrontSeats");

    if (CompareFeature("BluetoothHandsfreeSystem", baseCar.BluetoothHandsfreeSystem, (from f in cars select f.BluetoothHandsfreeSystem).ToList()))
        listBestFeatures.Add("BluetoothHandsfreeSystem");
    
    if (CompareFeature("CruiseControl", baseCar.CruiseControl, (from f in cars select f.CruiseControl).ToList()))
        listBestFeatures.Add("CruiseControl");

    if (CompareFeature("AutomaticHeadlights", baseCar.AutomaticHeadlights, (from f in cars select f.AutomaticHeadlights).ToList()))
        listBestFeatures.Add("AutomaticHeadlights");

    for (int i = 1; i < NUM_TOTAL_FEATURES; i++)
    {
        var featureName = "Feature"+ i.ToString();

        // get base car feature value
        var nameOfProperty = featureName;
        var propertyInfo = baseCar.GetType().GetProperty(nameOfProperty);
        var value = propertyInfo.GetValue(baseCar, null);
        var baseCarFeatureValue = value;

        // compare with competitor values
        //var competitorValues = cars.Select(s => (s.GetType().GetProperty("CarSpec")
                                //.PropertyType.GetProperty("featureName")
                                //.GetValue(s.GetType().GetProperty("CarSpec").GetValue(s)).ToString()));

        //var result = cars.AsQueryable().Select(dynamicFields.ToDynamicSelector())
        //                                    .Cast<DynamicClass>();

        //foreach (var item in result)
        //{
        //    Console.WriteLine(item.GetValue<string>(list[0])); // outputs all user names
        //}

        /* var competitorValues = from s in cars 
                                select s.GetType().GetProperty("CarSpec")
                                .PropertyType.GetProperty("featureName")
                                .GetValue(s.GetType().GetProperty("CarSpec").GetValue(s)); */

        List<string> values = new List<string>();

        foreach (var c in cars)
        {
            var featureValue = GetPropValue(c, featureName);

            values.Add(featureValue.ToString());

        }                        


        //var r = competitorValues.ToList();

        //if (CompareFeature(featureName, baseCar.SatelliteNavigation, (from f in cars 
                                                                        //select f.GetType().GetProperty(featureName).GetValue(f.GetType().GetProperty(featureName).GetValue(f).ToString())).ToList()))
        //if (CompareFeature(featureName, baseCar.SatelliteNavigation, competitorValues.ToList() ))                                                                        
            //listBestFeatures.Add(featureName);
    }

   

    return listBestFeatures;
}

static object GetPropValue(object src, string propName)
{
        return src.GetType().GetProperty(propName).GetValue(src, null);
}

/* static T GetValue<T>(this DynamicClass dynamicObject, string propName)
{
    if (dynamicObject == null)
    {
        throw new ArgumentNullException("dynamicObject");
    }

    var type = dynamicObject.GetType();
    var props = type.GetProperties(BindingFlags.Public 
                                 | BindingFlags.Instance 
                                 | BindingFlags.FlattenHierarchy);
    var prop = props.FirstOrDefault(property => property.Name == propName);
    if (prop == null)
    {
        throw new InvalidOperationException("Specified property doesn't exist.");
    }

    return (T)prop.GetValue(dynamicObject, null);
}

static string ToDynamicSelector(this IList<string> propNames)
{
   if (!propNames.Any()) 
       throw new ArgumentException("You need supply at least one property");
   return string.Format("new({0})", string.Join(",", propNames));
} */

static bool CompareFeature(string featureName, string baseCarValue, List<string> competitorVaues) 
{
    bool result = false;

    switch (featureName)
    {
        case "SatelliteNavigation" : 
            result = CompareFeatureByAvailability(baseCarValue, competitorVaues);
            break;
        case "leatherSeats" : {
            //if (CompareFeatureByGreaterThanValue(baseCarValue, competitorVaues))
                //listBestFeatures.Add(featureName);
            return true;
            break;
        }
        case "HeatedFrontSeats" : 
            //if (CompareFeatureByAvailability(baseCarValue, competitorVaues))
                //listBestFeatures.Add(featureName);
            return true;
            break;
    }

    return result;

}

static bool CompareFeatureByAvailability(string baseCar, List<string> competitors) {

    bool result = false;

    // dummy rule (implement rule to validate the best in this feature)
    foreach (var c in competitors)
    {
        if (baseCar=="Not Available" || c=="Standard")
            result = false;
        else
            result = true;
    }

    return result;
}

/* static bool CompareFeatureByGreaterThanValue(CarFeature baseCar, List<CarFeature> competitors) {

    bool result = false;

    // dummy rule (implement rule to validate the best in this feature)
    foreach (var c in competitors)
    {
        if (int.Parse(baseCar.FeatureValue) >= int.Parse(c.FeatureValue))
            result = true;
        else
            result = false;
    }

    return result;
}

static bool CompareFeatureByLessThanValue(CarFeature baseCar, List<CarFeature> competitors) {

    bool result = false;

    // dummy rule (implement rule to validate the best in this feature)
    foreach (var c in competitors)
    {
        if (int.Parse(baseCar.FeatureValue) < int.Parse(c.FeatureValue))
            result = true;
        else
            result = false;
    }

    return result;
}

static bool CompareFeatureByCustomRule(CarFeature baseCar, List<CarFeature> competitors) {

    bool result = false;

    // dummy rule (implement rule to validate the best in this feature)
    foreach (var c in competitors)
    {
        result = true;
    }

    return result;
} */

struct CarFeature
{
    public string CarId;
    public string FeatureValue;
}
