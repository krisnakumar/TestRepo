/* eslint-disable */
import React, { Component } from 'react';
import { hot } from 'react-hot-loader';
import 'bootstrap/dist/css/bootstrap.css';
import '../../scss/app.scss';
import Router from './Router';
import IdleTimer from 'react-idle-timer'
import { withCookies, Cookies } from 'react-cookie';
import { Row, Col, Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import * as Constants from '../../shared/constants';

/**
 * App Class defines the React component to render
 * the inner container layout
 * extending the Router, React and Component module.
 */
class App extends Component {
  constructor() {
    super();
    this.idleTimer = null;
    this.onAction = this._onAction.bind(this);
    this.onActive = this._onActive.bind(this);
    this.onIdle = this._onIdle.bind(this);
    this.toggle = this.toggle.bind(this);
    this.cancelAutoLogout = this.cancelAutoLogout.bind(this);

    this.state = {
      loading: true,
      loaded: false,
      isValid: false,
      modal: false
    };
  };

  _onAction(e) {
    console.log('user did something', e)
  };

  _onActive(e) {
    console.log('user is active', e)
    console.log('time remaining', this.idleTimer.getRemainingTime());
  };

  _onIdle(e) {
    console.log('user is idle', e)
    console.log('last active', this.idleTimer.getLastActiveTime());
    this.setState({ modal: true });
    setTimeout(() => this.autoLogout(), 5000);
  };

  autoLogout() {
    console.log('auto logout');
    window.location = window.location.origin;
  };

  /**
   * @method
   * @name - toggle
   * This method used set state of modal to open and close
   * @param none
   * @returns none
  */
  toggle() {
    this.setState({
      modal: false
    });
  };

  /**
    * @method
    * @name - toggle
    * This method used set state of modal to open and close
    * @param none
    * @returns none
  */
  cancelAutoLogout() {
    console.log('time remaining BEFORE', this.idleTimer.getRemainingTime());
    this.idleTimer.reset();
    this.setState({
      modal: false
    });
    console.log('time remaining AFTER', this.idleTimer.getRemainingTime());
  };

  /**
   * @method
   * @name - componentDidMount
   * This method will invoked whenever the component is mounted
   *  is update to this component class
   * @param none
   * @returns none
  */
  componentDidMount() {
    window.addEventListener('load', () => {
      this.setState({ loading: false });
      setTimeout(() => this.setState({ loaded: true }), 500);
    });
  };

  /**
   * @method
   * @name - componentWillMount
   * This method will invoked whenever the component is to be mounted
   * @param none
   * @returns none
  */
  componentWillMount() {
    const { cookies } = this.props;
    let token = cookies.get('IdentityToken'),                         // Get Identity token from browser cookie
      isTokenAvailable = token ? true : false,                      // Checking Identity token is available or not
      isBasePath = window.location.pathname == '/' ? true : false;  // Checking it is base path or not

    // Checking that the Identity token is available and it is not app base path
    if (!isTokenAvailable && !isBasePath) {
      // If there is no Identity token from browser cookie on loading app other than base domain setting state 'isValid' to false and doing re-direct
      this.setState({ isValid: false });
      window.location = window.location.origin;
    } else {
      this.setState({ isValid: true });
    }
  };

  render() {
    const { loaded, loading, isValid } = this.state,
    autoTimeout = 1000 * 60 * Constants.AUTO_LOGOUT_IDLE_TIME;
    return (
      <div>
        <Modal backdrop={"static"} isOpen={this.state.modal} toggle={this.toggle} fade={false} centered={true} className="auto-logout-modal">
          <ModalHeader toggle={this.toggle}> Auto logout</ModalHeader>
          <ModalBody>{Constants.AUTO_LOGOUT_MESSAGE}</ModalBody>
          <ModalFooter>
            <button color="primary" onClick={this.cancelAutoLogout}>Cancel</button>{' '}
          </ModalFooter>
        </Modal>
        <IdleTimer
          ref={ref => { this.idleTimer = ref }}
          element={document}
          onActive={this.onActive}
          onIdle={this.onIdle}
          onAction={this.onAction}
          debounce={250}
          timeout={autoTimeout} />

        {!loaded &&
          <div className={`load${loading ? '' : ' loaded'}`}>
            <div className="load__icon-wrap">
              <svg className="load__icon">
                <path fill="#4ce1b6" d="M12,4V2A10,10 0 0,0 2,12H4A8,8 0 0,1 12,4Z" />
              </svg>
            </div>
          </div>
        }
        <div>
          {isValid && <Router />}
        </div>
      </div>
    );
  }
}

export default hot(module)(withCookies(App));
