/* eslint-disable */
import React, { Component } from 'react';
import { hot } from 'react-hot-loader';
import 'bootstrap/dist/css/bootstrap.css';
import '../../scss/app.scss';
import Router from './Router';
import { withCookies, Cookies } from 'react-cookie';

/**
 * App Class defines the React component to render
 * the inner container layout
 * extending the Router, React and Component module.
 */
class App extends Component {
  constructor() {
    super();
    this.state = {
      loading: true,
      loaded: false,
      isValid: false
    };
  }

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
  }

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
    
    // Checkin that the Identity token is available and it is not app base path
    if(!isTokenAvailable && !isBasePath){
      // If there is no Identity token from browser cookie on loading app other than base domain setting state 'isValid' to false and doing re-direct
      this.setState({ isValid: false });
      window.location = window.location.origin;
    } else {
      this.setState({ isValid: true });
    }
  }

  render() {
    const { loaded, loading, isValid } = this.state;
    return (
      <div>
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
          { isValid && <Router /> }
        </div>
      </div>
    );
  }
}

export default hot(module)(withCookies(App));
