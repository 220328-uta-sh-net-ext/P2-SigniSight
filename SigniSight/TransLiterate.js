/* This simple app uses the '/transliterate' resource to transliterate text from
one script to another. In this sample, Japanese is transliterated to use the
Latin alphabet. */

/* This template relies on the request module, a simplified and user friendly
way to make HTTP requests. */
const axios = require('axios').default;
const { v4: uuidv4 } = require('uuid');

var key = "C:/Revature/P2-SigniSight_Fork/SigniSight/Key.txt";
var endpoint = "C:/Revature/P2-SigniSight_Fork/SigniSight/Endpoint.txt";

// Add your location, also known as region. The default is global.
// This is required if using a Cognitive Services resource.
var location = "eastus";

axios({
    baseURL: endpoint,
    url: '/translate',
    method: 'post',
    headers: {
        'Ocp-Apim-Subscription-Key': key,
        'Ocp-Apim-Subscription-Region': location,
        'Content-type': 'application/json',
        'X-ClientTraceId': uuidv4().toString()
    },
    params: {
        'api-version': '3.0',
        'to': 'th',
        'toScript': 'latn'
    },
    data: [{
        'text': 'Hello'
    }],
    responseType: 'json'
}).then(function(response){
    console.log(JSON.stringify(response.data, null, 4));
})