/* eslint-disable */
import React, { PureComponent } from 'react';
import { Link } from 'react-router-dom';
import PropTypes from 'prop-types';
import TopbarSidebarButton from './TopbarSidebarButton';
import TopbarProfile from './TopbarProfile';
import TopbarMail from './TopbarMail';
import TopbarLanguage from './TopbarLanguage';
import TopbarNotification from './TopbarNotification';
import TopbarNav from './tobar_nav/TopbarNav';

export default class TopbarWithNavigation extends PureComponent {
  static propTypes = {
    changeMobileSidebarVisibility: PropTypes.func.isRequired,
  };

  render() {
    const { changeMobileSidebarVisibility } = this.props;

    return (
      <div className="topbar topbar--navigation">
        <div className="topbar__wrapper">
          <TopbarNav />
          <div className="topbar__right">
            <TopbarNotification />
            <TopbarProfile />
          </div>
        </div>
      </div>
    );
  }
}
