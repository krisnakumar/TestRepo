/* eslint-disable */
import React, { Component } from 'react';
import { hot } from 'react-hot-loader';
import 'bootstrap/dist/css/bootstrap.css';
import '../../scss/app.scss';
import Router from './Router';
import IdleTimer from 'react-idle-timer'
import { withCookies, Cookies } from 'react-cookie';
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import * as Constants from '../../shared/constants';
import * as API from '../../shared/utils/APIUtils';
import JWT from 'jsonwebtoken';
import Schedule from 'node-schedule';

/**
 * App Class defines the React component to render
 * the inner container layout
 * extending the Router, React and Component module.
 */
class App extends Component {
  constructor() {
    super();
    this.timer = null;
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

  /**
   * @method
   * @name - _onAction
   * This method will invoked whenever user is makes an action on site
   * @param event
   * @returns none
  */
  _onAction(event) {
    // Write code to bind after any user interaction
  };

  /**
   * @method
   * @name - _onActive
   * This method will invoked whenever user is makes an activity on site
   * @param event
   * @returns none
  */
  _onActive(event) {
    // Write code to bind after any user in active state
  };

  /**
   * @method
   * @name - event
   * This method will invoked whenever user is inactive for INACTIVITY_TIME_LIMIT
   * @param none
   * @returns none
  */
  _onIdle(event) {
    this.setState({ modal: true });
    this.timer = setTimeout(() => this.autoLogout(), 1000 * 60 * 1);
  };

  autoLogout() {
    window.location = window.location.origin + "/Login.aspx"; //Need to be window.location.origin after integrating with LMS Site
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
    this.idleTimer.reset();
    clearTimeout(this.timer);
    this.setState({
      modal: false
    });
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
  async componentWillMount() {
    const { cookies } = this.props;
    let { dashboardAPIToken } = sessionStorage,
      idToken = '';
    if (dashboardAPIToken) {
      dashboardAPIToken = JSON.parse(dashboardAPIToken);
      idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";
    }                     // Get Identity token from browser cookie
    let isTokenAvailable = idToken ? true : false,                      // Checking Identity token is available or not
      isBasePath = window.location.pathname == '/' ? true : false,
      isMockBasePath = true;  // Checking it is base path or not

    // Checking that the Identity token is available and it is not app base path
    if (!isTokenAvailable && !isMockBasePath) {
      // If there is no Identity token from browser cookie on loading app other than base domain setting state 'isValid' to false and doing re-direct
      this.setState({ isValid: false });
      window.location = window.location.origin;
    } else {
      // let refreshSession = await API.LoginRefresh();
      // this.updateSessionTokens(refreshSession);
      this.setState({ isValid: true });
    }
  };

  async updateSessionTokens(refreshSession) {
    let idToken = refreshSession.IdentityToken || "",
        accessToken = refreshSession.AccessToken || "",
        _self = this;

    // get the decoded payload and header
    let decoded = JWT.decode(idToken, { complete: true });

    let exp = new Date(decoded.payload.exp * 1000),
      tokenExp = exp.toLocaleString(),
      refreshTrigger = new Date(exp),
      durationInMinutes = 5;

    refreshTrigger.setMinutes(exp.getMinutes() - durationInMinutes);
    let refreshYear = refreshTrigger.getFullYear(),
      refreshMonth = refreshTrigger.getMonth(),
      refreshDay = refreshTrigger.getDate(),
      refreshHour = refreshTrigger.getHours(),
      refreshMinute = refreshTrigger.getMinutes(),
      refreshSecond = refreshTrigger.getSeconds();

    var date = new Date(refreshYear, refreshMonth, refreshDay, refreshHour, refreshMinute, refreshSecond);
    let scheduleTask = Schedule.scheduleJob(date, async function () {
      let refreshSession = await API.LoginRefresh();
      _self.updateSessionTokens(refreshSession);
    });
  }

  render() {
    const { loaded, loading, isValid } = this.state,
      autoTimeout = 1000 * 60 * Constants.AUTO_LOGOUT_IDLE_TIME,
      isBasePath = window.location.pathname == '/' ? true : false;
    return (
      <div>
        <Modal backdrop={"static"} isOpen={this.state.modal} toggle={this.toggle} fade={false} centered={true} className="auto-logout-modal">
          <ModalHeader> Auto logout</ModalHeader>
          <ModalBody>{Constants.AUTO_LOGOUT_MESSAGE}</ModalBody>
          <ModalFooter>
            <button color="primary" onClick={this.cancelAutoLogout}>Cancel</button>{' '}
          </ModalFooter>
        </Modal>
        { 
          !isBasePath && <IdleTimer
            ref={ref => { this.idleTimer = ref }}
            element={document}
            onActive={this.onActive}
            onIdle={this.onIdle}
            onAction={this.onAction}
            debounce={250}
            timeout={autoTimeout} />
        }
        {
          !loaded &&
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
