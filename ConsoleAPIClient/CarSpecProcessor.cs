public static class CarSpecProcessor {

    const int NUM_TOTAL_FEATURES = 100;

    public static List<string> CompareCarFeatures(CarSpec baseCar, List<CarSpec> cars) 
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
            var featureName = "feature"+ i.ToString();

            // get base car feature value
            var nameOfProperty = featureName;
            var propertyInfo = baseCar.GetType().GetProperty(nameOfProperty);
            var value = propertyInfo.GetValue(baseCar, null);
            var baseCarFeatureValue = value;

            List<string> values = new List<string>();

            foreach (var c in cars)
            {
                var featureValue = GetPropValue(c, featureName);

                values.Add(featureValue.ToString());

            }   

            if (CompareFeature(featureName, baseCarFeatureValue.ToString(), values))
                listBestFeatures.Add(featureName);                     
        }

        return listBestFeatures;
    }

    static object GetPropValue(object src, string propName)
    {
            return src.GetType().GetProperty(propName).GetValue(src, null);
    }


    static bool CompareFeature(string featureName, string baseCarValue, List<string> competitorVaues) 
    {
        bool result = false;

        switch (featureName)
        {
            case "DummyFeature1" : 
                result = CompareFeatureByGreaterThanValue(baseCarValue, competitorVaues);
                break;
            case "DummyFeature2" : 
                 result = CompareFeatureByLessThanValue(baseCarValue, competitorVaues);
                break;
            case "SatelliteNavigation":
            case "LeatherSeats":
            case "HeatedFrontSeats":
            case "BluetoothHandsfreeSystem":
            case "CruiseControl":
            case "AutomaticHeadlights":
                result = CompareFeatureByAvailability(baseCarValue, competitorVaues);
                break;
            default:
                result = CompareFeatureByAvailability(baseCarValue, competitorVaues);
                break;
        }

        return result;
    }

    static bool CompareFeatureByAvailability(string baseCarValue, List<string> competitorValues) {

        bool result = true;

        // dummy rule (implement rule to validate the best in this feature)
        if ((baseCarValue=="Not Available") && (competitorValues.Any(v => v.Contains("Standard")))) return false;
        if ((baseCarValue=="Not Available") && (competitorValues.Any(v => v.Contains("Optional")))) return false;
        if ((baseCarValue=="Optional") && (competitorValues.Any(v => v.Contains("Standard")))) return false;
        if ((baseCarValue=="Standard") && (competitorValues.Any(v => v.Contains("Standard")))) return false;

        return result;
    }

    static bool CompareFeatureByGreaterThanValue(string baseCarValue, List<string> competitorValues) {

        bool result = false;

        // logic goes here

        return result;
    } 

    static bool CompareFeatureByLessThanValue(string baseCarValue, List<string> competitorValues) {

       bool result = false;

        // logic goes here

        return result;
    }

    static bool CompareFeatureByCustomRule(string baseCarValue, List<string> competitorValues) {

       bool result = false;

        // logic goes here

        return result;
    }

    struct CarFeature
    {
        public string CarId;
        public string FeatureValue;
    }

    
}