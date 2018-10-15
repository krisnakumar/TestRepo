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

const logo = `${process.env.PUBLIC_URL}/img/content_logo.png`;


class Topbar extends PureComponent {
  render() {
    const { changeMobileSidebarVisibility, changeSidebarVisibility } = this.props;
    return (
      <div className="topbar">
        <div className="topbar__wrapper">
          <div className="topbar__left">
            <p className="topbar__left-logo">
              <img src={logo} alt="" />
            </p>
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
