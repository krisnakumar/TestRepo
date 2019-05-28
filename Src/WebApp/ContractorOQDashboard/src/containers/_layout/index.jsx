/* eslint-disable */
import React, { Component } from 'react';
import Topbar from './topbar/Topbar';
import TopbarWithNavigation from './topbar_with_navigation/TopbarWithNavigation';

/**
 * Layout Class defines the React component to render
 * the Layout components App
 */
class Layout extends Component {

  render() {
    return (
      <div>        
          <Topbar />
          <TopbarWithNavigation />
      </div>
    );
  }
}

// export default withRouter(connect(state => ({
//   theme: state.theme,
// }))(Layout));
export default Layout;