/* eslint-disable */
/*
* log_in\index.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript library will used render Login page
* Template: React PureComponent
* Prerequisites: React and babel
*/


import React from 'react';
import LogInForm from './components/LogInForm';

const LogIn = () => (
  <div className="loginLogo">
    {/* <img src="https://d2vkqsz7y0fh3j.cloudfront.net/img/logo_big.png?v=2" title="OnBoard Learning Management System" alt="logo" id="logo_top"></img>
    <div className="account">
      <div className="account__wrapper">
        <div className="account__card">
          <LogInForm />
        </div>
      </div>
    </div> */}
    <LogInForm />
  </div>
);

export default LogIn;
