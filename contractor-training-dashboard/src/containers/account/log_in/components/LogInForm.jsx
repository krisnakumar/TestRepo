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
import { reduxForm } from 'redux-form';
import { BrowserRouter as Redirect } from "react-router-dom";
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import * as Constants from '../../../../shared/constants';

import CTDashboard from '../../../contractor_training_dashboard/index';


class LogInForm extends PureComponent {
  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor(props) {
    super(props);
    const { cookies } = props;
    this.state = {
      showPassword: false,
      toDashboard: false,
      hasSessionCookie: false,
      isReloadWindow: false,
      username: "",
      password: ""
    };

    this.showPassword = this.showPassword.bind(this);
    this.loginSubmit = this.loginSubmit.bind(this);
    this.handleUserInput = this.handleUserInput.bind(this);
    this.authenticate = this.authenticate.bind(this);
  };

  componentWillMount() {
    const { cookies } = this.props;
    let { dashboardAPIToken } = sessionStorage,
      idToken = '';
    if (dashboardAPIToken) {
      dashboardAPIToken = JSON.parse(dashboardAPIToken);
      idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";
    }
    if (idToken) {
      this.setState({ toDashboard: true, hasSessionCookie: true, isReloadWindow: false });
    } else {
      let readSessionCount = localStorage.getItem('readSessionCount');
      if (readSessionCount) {
        // Do nothing
      } else {
        localStorage.setItem('readSessionCount', '1');
      }
      this.setState({ toDashboard: false, hasSessionCookie: false, isReloadWindow: true });
    }
  };

  componentDidCatch(error, info) {
    // Display fallback UI
    // this.setState({ hasError: true });
    // You can also log the error to an error reporting service
    console.log(error, info);
  };

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
  };

  loginSubmit(event) {
    event.preventDefault();
    let userName = this.state.username;
    let password = this.state.password;
    if (userName && password) {
      this.authenticate(userName, password);
    }
  };

  authenticate(userName, password) {
    let _self = this,
      url = Constants.API_DOMAIN + Constants.API_STAGE_NAME + "/login",
      postData = {
        "UserName": userName,
        "Password": password
      };
    fetch(url,
      {
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        method: "POST",
        body: JSON.stringify(postData)
      }).then(function (response) {
        return response.json()
      }).then(function (json) {
        if (json.AccessToken && json.IdentityToken) {
          const { cookies } = _self.props;
          cookies.set('UserName', _self.state.username, { path: '/' });
          cookies.set('AccessToken', json.AccessToken, { path: '/' });
          cookies.set('IdentityToken', json.IdentityToken, { path: '/' });
          cookies.set('RefreshToken', json.RefreshToken, { path: '/' });
          cookies.set('UserId', json.UserId, { path: '/' });
          cookies.set('CompanyId', json.CompanyId, { path: '/' });
          cookies.set('UserName', json.UserName, { path: '/' });
          _self.setState({ toDashboard: true, hasSessionCookie: true });
        } else {
          _self.setState({ toDashboard: false, hasSessionCookie: false });
        }
        return json
      }).catch(function (ex) {
        console.log('parsing failed', ex)
      });
  }

  reloadWindow() {
    let readSessionCount = localStorage.getItem('readSessionCount');
    if (readSessionCount <= 2) {
      readSessionCount = parseInt(readSessionCount) + 1;
      localStorage.setItem('readSessionCount', readSessionCount);
      location.reload();
    } else {
      localStorage.removeItem('readSessionCount');
      window.location = window.location.origin;
    }
  };

  render() {
    const { isReloadWindow } = this.state;

    if (this.state.hasSessionCookie) {
      return <CTDashboard />;
    } else {
      return (
        isReloadWindow && <Modal backdrop={"static"} isOpen={this.state.isReloadWindow} toggle={this.toggle} fade={false} centered={true} className="auto-logout-modal">
          <ModalHeader> Alert</ModalHeader>
          <ModalBody>{Constants.NO_SESSION_MESSAGE}</ModalBody>
          <ModalFooter>
            <button color="primary" onClick={this.reloadWindow}>Refresh</button>{' '}
          </ModalFooter>
        </Modal>

        ||

        <div className={`load`}>
          <div className="load__icon-wrap">
            <svg className="load__icon">
              <path fill="#4ce1b6" d="M12,4V2A10,10 0 0,0 2,12H4A8,8 0 0,1 12,4Z" />
            </svg>
          </div>
        </div>
      );
    }
  }
}

export default reduxForm({
  form: 'log_in_form', // a unique identifier for this form
})(withCookies(LogInForm));
