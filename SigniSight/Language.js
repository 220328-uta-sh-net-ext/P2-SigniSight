/*[GET] request to the language resource to retrieve languages from Microsoft Translator*/

const request = require('request');
const uuidv4 = require('uuid/v4');
var endpoint_var = 'https://api.cognitive.microsofttranslator.com/';
if (!process.env[endpoint_var]) {
    throw new Error('Please set/export the following environment variable: ' + endpoint_var);
}
var endpoint = process.env[endpoint_var];
/* If you encounter any issues with the base_url or path, make sure that you are
using the latest endpoint: https://docs.microsoft.com/azure/cognitive-services/translator/reference/v3-0-languages */
function getLanguages(){
    let options = {
        method: 'GET',
        baseUrl: endpoint,
        url: 'languages',
        qs: {
          'api-version': '3.0',
        },
        headers: {
          'Content-type': 'application/json',
          'X-ClientTraceId': uuidv4().toString()
        },
        json: true,
    };

    request(options, function(err, res, body){
        console.log(JSON.stringify(body, null, 4));
    });
};

// Call the function to get a list of supported languages.
getLanguages();