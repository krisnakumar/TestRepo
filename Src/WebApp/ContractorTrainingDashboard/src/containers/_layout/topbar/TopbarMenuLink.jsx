/* eslint-disable */
import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';

export default class TopbarMenuLinks extends PureComponent {
  static propTypes = {
    title: PropTypes.string.isRequired,
    icon: PropTypes.string.isRequired,
    path: PropTypes.string.isRequired,
  };
  render() {
    const { title, icon, path } = this.props;
    return (
      <a className="topbar__link" href={path}>
        <span className={"topbar__link-icon lnr " + icon}></span>
        <p className="topbar__link-title">{title}</p>
      </a>
    );
  }
}
