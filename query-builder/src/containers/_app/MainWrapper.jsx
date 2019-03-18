import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import classNames from 'classnames';
import PropTypes from 'prop-types';
import { ThemeProps } from '../../shared/prop-types/ReducerProps';

class MainWrapper extends PureComponent {
  static propTypes = {
    theme: ThemeProps.isRequired,
    children: PropTypes.element.isRequired,
  };

  render() {
    const { theme } = this.props;

    const wrapperClass = classNames({
      wrapper: true,
    });

    return (
      <div className={theme.className}>
        <div className={wrapperClass}>
          {this.props.children}
        </div>
      </div>
    );
  }
}

export default connect(state => ({
  theme: state.theme,
}))(MainWrapper);
