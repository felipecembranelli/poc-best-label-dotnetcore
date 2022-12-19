const carApi = require('./carApi')

const asyncApiCall = async () => {
    const response = await carApi.httpGetCars()

    //console.log(response.data)
    // var carSpecs = JSON.parse(response.data);
    let carSpecs = response.data;

    const bestFeatures = [];

    for (let index = 0; index < 50; index++) {

        const featureValues = [];

        carSpecs.forEach(function(table) {
            var featureValue = table['feature' + index];
            //console.log(featureValue);

            featureValues.push(featureValue);
            
        });

        //console.log(featureValues);

        // apply logic to determine the best
        if (CompareByAvailability(featureValues)) {
            //console.log("adding best" + index);
            bestFeatures.push('feature' + index);
        }
    }
    
    //console.log(bestFeatures);

    return bestFeatures;
    
}



//return ret

var http = require('http');
const { readSync } = require('fs');

var app = http.createServer(function(req,res){
    res.setHeader('Content-Type', 'application/json');
    console.log('calling...')
    let bestFeatures;
    let ret = asyncApiCall().then(function(result) { 
        bestFeatures = result;
        console.log(bestFeatures);
        res.end(JSON.stringify({bestFeatures}));
    });

    //console.log(d);
    
    //res.end(JSON.stringify({ a: 1 }));
});
app.listen(3002);

//var app = http.createServer(function(req,res){
    //res.setHeader('Content-Type', 'application/json');
    //console.log('calling...')
    //let ret = asyncApiCall();
    //res.end(JSON.stringify({ a: 1 }));
    //res.end(JSON.stringify({ a: 1 }));
//});

/* app.get('/cars', function (req, res) {   
    res.contentType('application/json');
    res.setHeader('Content-Type', 'application/json');
    console.log('calling...')
    let ret = asyncApiCall();
    res.end(JSON.stringify(ret));
}); */

//app.listen(3000)

/* const requestListener = function(req, res) {
    res.setHeader("Content-Type", "application/json");
    res.writeHead(200);
    res.end(JSON.stringify(data, null, 3));
} */

function CompareByAvailability(values) {

	let result = true;
	let baseCarValue = values.shift();

	//console.log("basecarvalue:"+ baseCarValue);

	// dummy rule (implement rule to validate the best in this feature)
	if ((baseCarValue=="Not Available") && (values.includes("Standard"))) return false;
	if ((baseCarValue=="Not Available") && (values.includes("Optional"))) return false;
	if ((baseCarValue=="Optional") && (values.includes("Standard"))) return false;
	if ((baseCarValue=="Standard") && (values.includes("Standard"))) return false;

	return result;

}