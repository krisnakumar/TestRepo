/* eslint-disable */
import React, { PureComponent } from 'react';
import { Link } from 'react-router-dom';
import PropTypes from 'prop-types';
import TopbarSidebarButton from './TopbarSidebarButton';
import TopbarProfile from './TopbarProfile';
import TopbarMail from './TopbarMail';
import TopbarNotification from './TopbarNotification';
import TopbarSearch from './TopbarSearch';
import TopbarLanguage from './TopbarLanguage';

const logo = `https://d2vkqsz7y0fh3j.cloudfront.net/img/content_logo.png?v=2`;


class Topbar extends PureComponent {
  render() {
    const { changeMobileSidebarVisibility, changeSidebarVisibility } = this.props;
    return (
      <div className="topbar">
        <div className="topbar__wrapper">
          <div className="topbar__left">
            <a className="topbar__left-logo" href= "https://dev.its-training.com/Login.aspx">
              <img src={logo} alt="" />
            </a>
          </div>
          <div className="topbar__right">
            <TopbarNotification />
            <TopbarProfile />
          </div>
        </div>
      </div>
    );
  }
}

export default Topbar;
