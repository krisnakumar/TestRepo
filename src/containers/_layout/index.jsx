/* eslint-disable */
import React, { Component } from 'react';
import { withRouter } from 'react-router-dom';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import Topbar from './topbar/Topbar';
import TopbarWithNavigation from './topbar_with_navigation/TopbarWithNavigation';
import { ThemeProps } from '../../shared/prop-types/ReducerProps';

/**
 * Layout Class defines the React component to render
 * the Layout components App
 */
class Layout extends Component {
  static propTypes = {
    dispatch: PropTypes.func.isRequired,
    theme: ThemeProps.isRequired,
  };

  render() {
    return (
      <div>        
          <Topbar
            changeMobileSidebarVisibility={this.changeMobileSidebarVisibility}
            changeSidebarVisibility={this.changeSidebarVisibility}
          />
          <TopbarWithNavigation />
      </div>
    );
  }
}

export default withRouter(connect(state => ({
  theme: state.theme,
}))(Layout));
