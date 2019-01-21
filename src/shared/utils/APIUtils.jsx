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

import React, { PureComponent } from 'react';
import { BrowserRouter as Router, Route, Link, Redirect, withRouter } from "react-router-dom";
import 'whatwg-fetch'
import * as Constants from '../constants';

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
    document.getElementById("loader-layer").classList.remove("loader-hide");
    document.getElementById("loader-layer").classList.add("loader-show");

    let request = {},
        url = Constants.API_DOMAIN + Constants.API_STAGE_NAME + path;

    if(type == "POST"){
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

   return fetch(url, request).then(function(response) {
        if(response.status == 401){
            //LoginRefresh("", token, false)
            deleteAllCookies();
            window.location =window.location.origin;
        } else {
            document.getElementById("loader-layer").classList.remove("loader-show");
            document.getElementById("loader-layer").classList.add("loader-hide");
            return response.json();
        }       
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
    let url = Constants.API_DOMAIN + Constants.API_STAGE_NAME + "/login/refresh",
        userName = decodeURIComponent(getCookie("UserName")),
        refreshToken = getCookie("RefreshToken");
    
    return fetch(url, {
     method: "POST",
     headers: {
         'Accept': 'application/json',
         'Content-Type': 'application/json',
         "Authorization": token
       },
    body: JSON.stringify({
         "RefreshToken": refreshToken,
         "UserName": userName     
        })
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
 };


 function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift();
 };