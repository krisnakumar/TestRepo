/* eslint-disable */
import React from 'react';
import { Link } from 'react-router-dom';
import FacebookIcon from 'mdi-react/FacebookIcon';
import GooglePlusIcon from 'mdi-react/GooglePlusIcon';
import LogInForm from './components/LogInForm';

const LogIn = () => (
  <div className="loginLogo">
    <img src="https://d2vkqsz7y0fh3j.cloudfront.net/img/logo_big.png?v=2" title="OnBoard Learning Management System" alt="logo" id="logo_top"></img>
    <div className="account">
      <div className="account__wrapper">
        <div className="account__card">
          <LogInForm />
        </div>
      </div>
    </div>
  </div>
);

export default LogIn;
