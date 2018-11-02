/* eslint-disable */
/*
* APIUtils.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This JSX file will served as helper util class for all REST API calls
* Template: React Functions
* Prerequisites: whatwg-fetch

METHODS
--------
ProcessAPI(url, requestPayload, token, isLogin, type, isLoader) 

*/

import 'whatwg-fetch'

export async function ProcessAPI(url, requestPayload, token, isLogin, type, isLoader) {
   return fetch(url, {
    method: type,
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        "Authorization": token
      }
    }).then(function(response) {
        // if(response.status)
        return response.json();
    }).then(function(json) { 
        return json;
    }).catch(function(ex) {
        // Handle API Exception here       
        console.log('parsing failed', ex);
    });
}
