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
import { withCookies, Cookies } from 'react-cookie';
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import * as Constants from '../../../../shared/constants';
import OQDashboard from '../../../oq_contractor_dashboard/index'


class LogInForm extends PureComponent {
  constructor(props) {
    super(props);
    this.state = {
      toDashboard: false,
      hasSessionCookie: false,   
      hasSessionCookie: false,
    };
  };

  componentWillMount() {
    let { dashboardAPIToken } = sessionStorage,
      idToken = '';
    if (dashboardAPIToken) {
      dashboardAPIToken = JSON.parse(dashboardAPIToken);
      idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";
    }
    if (idToken) {
      this.setState({ toDashboard: true, hasSessionCookie: true, isReloadWindow: false });
    } else {
      this.setState({ toDashboard: true, hasSessionCookie: true, isReloadWindow: false });
    }
  };

  componentDidCatch(error, info) {
    // Display fallback UI
    // this.setState({ hasError: true });
    // You can also log the error to an error reporting service
    console.log(error, info);
  };

  render() {
    const { isReloadWindow } = this.state;

    if (this.state.hasSessionCookie) {
      return <OQDashboard />;
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

// export default reduxForm({
//   form: 'log_in_form', // a unique identifier for this form
// })(withCookies(LogInForm));

export default LogInForm;
