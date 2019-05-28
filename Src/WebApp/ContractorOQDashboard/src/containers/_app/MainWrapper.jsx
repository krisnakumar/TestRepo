/* eslint-disable */
import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import classNames from 'classnames';
import PropTypes from 'prop-types';
import { ThemeProps } from '../../shared/prop-types/ReducerProps';

class MainWrapper extends PureComponent {

  render() {
    return (
      <div className="theme-light">
        <div className="wrapper">
          {this.props.children}
        </div>
      </div>
    );
  }
}

// export default connect(state => ({
//   theme: state.theme,
// }))(MainWrapper);
export default MainWrapper;