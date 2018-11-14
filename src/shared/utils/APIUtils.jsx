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

/**
* @method
* @name - ProcessAPI
* This method will call REST API according to the input from user
* @param url
* @param requestPayload
* @param token
* @param isLogin
* @param type
* @param isLoader
* @returns json
*/
export async function ProcessAPI(url, requestPayload, token, isLogin, type, isLoader) {
    document.getElementById("loader-layer").classList.remove("loader-hide");
    document.getElementById("loader-layer").classList.add("loader-show");
   return fetch(url, {
    method: type,
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        "Authorization": token
      }
    }).then(function(response) {        
        document.getElementById("loader-layer").classList.remove("loader-show");
        document.getElementById("loader-layer").classList.add("loader-hide");
        return response.json();        
    }).then(function(json) { 
        document.getElementById("loader-layer").classList.remove("loader-show");
        document.getElementById("loader-layer").classList.add("loader-hide");
        return json;
    }).catch(function(ex) {
        document.getElementById("loader-layer").classList.remove("loader-show");
        document.getElementById("loader-layer").classList.add("loader-hide");
        // Handle API Exception here       
        console.log('parsing failed', ex);
    });
}

// WIP ::::::::
/**
* @method
* @name - LoginRefresh
* This method will call Login Refresh API if the session is expired 
* @param requestPayload
* @param token
* @param isLoader
* @returns none
*/
export async function LoginRefresh(requestPayload, token, isLoader) {
    return fetch("https://klrg45ssob.execute-api.us-west-2.amazonaws.com/dev/login/refresh", {
     method: "POST",
     headers: {
         'Accept': 'application/json',
         'Content-Type': 'application/json',
         "Authorization": token
       },
    body: {
         "RefreshToken": "",
         "UserName": ""      
        }
     }).then(function(response) {
         console.log("response",response);
         return response.json();        
     }).then(function(json) { 
         console.log("json",json);
         return json;
     }).catch(function(ex) {
         // Handle API Exception here       
         console.log('parsing failed', ex);
     });
 }