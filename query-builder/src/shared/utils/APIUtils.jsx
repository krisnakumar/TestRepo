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
import * as Constants from '../constants';

const apoolData = {
    UserPoolId: 'XXX_XXX', // Your user pool id here
    ClientId: '4efougb8nqj7f72ku183rudmqm', // Your client id here
    Region: 'us-west-2'
};


/**
* @method
* @name - ProcessAPI
* This method will call REST API according to the input from user
* @param path
* @param requestPayload
* @param token
* @param isLogin
* @param type
* @param isLoader
* @returns json
*/
export async function ProcessAPI(path, requestPayload, token, isLogin, type, isLoader) {
    let _self = this;
    let API_URL = Constants.API_CONFIG.API_URL || "";
    if (API_URL == "") {
        API_URL = await Constants.getAPIEndpoint();
        Constants.API_CONFIG.API_URL = API_URL;
    }

    if (document.getElementById("loader-layer")) {
        document.getElementById("loader-layer").classList.remove("loader-hide");
        document.getElementById("loader-layer").classList.add("loader-show");
    }
    let request = {},
        // url = Constants.API_DOMAIN + Constants.API_STAGE_NAME + path;
        url = API_URL + path;

    if (type == "POST") {
        request = {
            method: type,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                "Authorization": token
            },
            body: JSON.stringify(requestPayload)
        };
    } else {
        request = {
            method: type,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                "Authorization": token
            }
        };
    }

    return fetch(url, request).then(function (response) {
        if (response.status == 401) {
            // LoginRefresh("", token, false)
            // deleteAllCookies();
            // window.location = window.location.origin;
        } else {
            if (document.getElementById("loader-layer")) {
                document.getElementById("loader-layer").classList.remove("loader-show");
                document.getElementById("loader-layer").classList.add("loader-hide");
            }
            return response.json();
        }
    }).then(function (json) {
        if (document.getElementById("loader-layer")) {
            document.getElementById("loader-layer").classList.remove("loader-show");
            document.getElementById("loader-layer").classList.add("loader-hide");
        }
        // let responseObject = Object.values(json)[0];
        // return responseObject;
        let responseObject = Object.keys(json);
        console.log(json[responseObject])
        return json[responseObject];
    }).catch(function (ex) {
        if (document.getElementById("loader-layer")) {
            document.getElementById("loader-layer").classList.remove("loader-show");
            document.getElementById("loader-layer").classList.add("loader-hide");
        }
        // Handle API Exception here
        console.log('parsing failed', ex);
    });
}

/**
* @method
* @name - deleteAllCookies
* This method will used to clear all available domain cookie if session is expired 
* @param none
* @returns none
*/
function deleteAllCookies() {
    let cookies = document.cookie.split(";");

    for (let i = 0; i < cookies.length; i++) {
        let cookie = cookies[i];
        let eqPos = cookie.indexOf("=");
        let name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
        let expires = new Date();
        let duration = 1;
        expires.setTime(expires.getTime() - (duration));
        document.cookie = name + '=;expires=' + expires.toUTCString() + ';path=/;';
    }
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
    let _self = this;
    // let API_URL = Constants.API_CONFIG.API_URL || "";
    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let refreshToken = dashboardAPIToken.dashboardAPIToken.RefreshToken || "";
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

    return fetch("https://cognito-idp." + apoolData.Region + ".amazonaws.com/" + apoolData.UserPoolId, {
        headers: {
            "X-Amz-Target": "AWSCognitoIdentityProviderService.InitiateAuth",
            "Content-Type": "application/x-amz-json-1.1",
        },
        mode: 'cors',
        cache: 'no-cache',
        method: 'POST',
        body: JSON.stringify({
            ClientId: apoolData.ClientId,
            AuthFlow: 'REFRESH_TOKEN_AUTH',
            AuthParameters: {
                REFRESH_TOKEN: refreshToken
            }
        }),
    }).then((response) => {
        return response.json(); // this will give jwt id and access tokens
    }).then(function (json) {
        let authenticationResult = json.AuthenticationResult;
        if (authenticationResult.AccessToken && authenticationResult.IdToken) {
            dashboardAPIToken.dashboardAPIToken.AccessToken = authenticationResult.AccessToken || "";
            dashboardAPIToken.dashboardAPIToken.IdToken = authenticationResult.IdToken || "";
            dashboardAPIToken.dashboardAPIToken.IsUpdated = true;
            sessionStorage.dashboardAPIToken = JSON.stringify(dashboardAPIToken);
        }
        return authenticationResult
    }).catch(function (ex) {
        // Handle API Exception here       
        console.log('API ERROR', ex);
    });
};

function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift();
};
