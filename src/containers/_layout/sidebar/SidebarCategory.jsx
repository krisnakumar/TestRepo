import React, { Component } from 'react';
import { Collapse } from 'reactstrap';
import PropTypes from 'prop-types';

export default class SidebarCategory extends Component {
  static propTypes = {
    title: PropTypes.string.isRequired,
    icon: PropTypes.string,
    children: PropTypes.arrayOf(PropTypes.element).isRequired,
  };

  static defaultProps = {
    icon: '',
  };

  constructor() {
    super();
    this.state = {
      collapse: false,
    };
  }

  toggle = () => {
    this.setState({ collapse: !this.state.collapse });
  };

  render() {
    return (
      <div className={`sidebar__category-wrap${this.state.collapse ? ' sidebar__category-wrap--open' : ''}`}>
        <button className="sidebar__link sidebar__category" onClick={this.toggle}>
          {this.props.icon ? <span className={`sidebar__link-icon lnr lnr-${this.props.icon}`} /> : ''}
          <p className="sidebar__link-title">{this.props.title}</p>
          <span className="sidebar__category-icon lnr lnr-chevron-right" />
        </button>
        <Collapse isOpen={this.state.collapse} className="sidebar__submenu-wrap">
          <ul className="sidebar__submenu">
            <div>
              {this.props.children}
            </div>
          </ul>
        </Collapse>
      </div>
    );
  }
}
