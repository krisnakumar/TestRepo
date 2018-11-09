/* eslint-disable */
/*
* LoginForm.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript library will used render Login Form and perform authentication on submit
* Template: React PureComponent
* Prerequisites: React and babel

METHODS
--------
showPassword( EVENT )
loginSubmit(EVENT)
handleUserInput( EVENT )
authenticate()
*/

import React, { PureComponent } from 'react';
import { Field, reduxForm } from 'redux-form';
import { BrowserRouter as Router, Route, Link, Redirect, withRouter } from "react-router-dom";
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import Menus from '../../../_layout/Menus.json';

class LogInForm extends PureComponent {
  static propTypes = {
    handleSubmit: PropTypes.func.isRequired,
    cookies: instanceOf(Cookies).isRequired
  };

  constructor(props) {
    super(props);
    const { cookies } = props;
    this.state = {
      showPassword: false,
      toDashboard: false,
      username: "",
      password: ""
    };

    this.showPassword = this.showPassword.bind(this);
    this.loginSubmit = this.loginSubmit.bind(this);
    this.handleUserInput = this.handleUserInput.bind(this);
    this.authenticate = this.authenticate.bind(this);
  }

  componentDidCatch(error, info) {
    // Display fallback UI
    // this.setState({ hasError: true });
    // You can also log the error to an error reporting service
    console.log(error, info);
  }

  showPassword(e) {
    e.preventDefault();
    this.setState({
      showPassword: !this.state.showPassword,
    });
  }

  handleUserInput(event) {
    let name = event.target.name;
    let value = event.target.value;
    this.setState({ [name]: value });
  }

  loginSubmit(event){
    event.preventDefault();
    let userName = this.state.username;
    let password = this.state.password;
    if(userName && password){      
      this.authenticate();
    }
  }

  authenticate(){
    let _self = this,
        url = "https://omwlc1qx62.execute-api.us-west-2.amazonaws.com/dev/login",
        postData = {
          "UserName": _self.state.username,
          "Password": _self.state.password
          };
    fetch(url, 
      {
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      method: "POST",
      body: JSON.stringify(postData)
    }).then(function(response) {
      return response.json()
    }).then(function(json) { 
        if(json.AccessToken && json.IdentityToken){
          const { cookies } = _self.props;
          cookies.set('AccessToken', json.AccessToken, { path: '/' });
          cookies.set('IdentityToken', json.IdentityToken, { path: '/' });
          cookies.set('RefreshToken', json.RefreshToken, { path: '/' });
          // set the localstorage for menu's
          localStorage.setItem('menus', JSON.stringify(Menus));
          _self.setState({ toDashboard: true });
        } else {
          _self.setState({ toDashboard: false });
        }
        return json
    }).catch(function(ex) {
      console.log('parsing failed', ex)
    });
  }

  render() {
    const { handleSubmit } = this.props;

    if (this.state.toDashboard === true) {
      return <Redirect to='/employeereports' />
    }

    return (
      <form className="form" onSubmit={this.loginSubmit}>
        <div className="form__form-group">
          <label className="form__form-group-label  login-label" htmlFor="name">Username</label>
          <div className="form__form-group-field">
            <Field
              name="username"
              id="userName"
              component="input"
              type="text"
              placeholder=""
              onChange={event => this.handleUserInput(event)}
            />
          </div>
        </div>
        <div className="form__form-group">
          <label className="form__form-group-label login-label" htmlFor="password">Password</label>
          <div className="form__form-group-field">
            <Field
              name="password"
              component="input"
              id="password"
              type={this.state.showPassword ? 'text' : 'password'}
              placeholder=""
              onChange={event => this.handleUserInput(event)}
            />
          </div>
        </div>
        <div className="account__btns">
          <button
              type="submit"
              className="btn btn-primary account__btn">
                Login
            </button>
        </div>
        <div className="version-number">
          <p>V0.8.181102.001</p> 
        </div>
      </form>
    );
  }
}

export default reduxForm({
  form: 'log_in_form', // a unique identifier for this form
})(withCookies(LogInForm));
