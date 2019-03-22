/* eslint-disable */
import React, { PureComponent } from 'react';
import { Collapse } from 'reactstrap';
import TopbarMenuLink from './TopbarMenuLink';

const Ava = "";

export default class TopbarProfile extends PureComponent {
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
      <div className="topbar__profile">
        <button className="topbar__avatar" onClick={this.toggle}>
          <p className="topbar__avatar-name">Tom Smith</p>
          <img className="topbar__avatar-img" src={Ava} alt="avatar" />
            <i id="downArrow" className="fa fa-caret-down" title="Down Arrow" aria-hidden="true" />
        </button>
        {this.state.collapse && <button className="topbar__back" onClick={this.toggle} />}
        <Collapse isOpen={this.state.collapse} className="topbar__menu-wrap">
          <div className="topbar__menu user-menu">
            <TopbarMenuLink title="User Profile" icon="fa fa-user-circle-o fa-lg" path="userprofile.aspx" />
            <TopbarMenuLink title="Dashboard" icon="fa fa-tachometer fa-lg" path="DashBoard.aspx" />
            <TopbarMenuLink title="Events" icon="fa fa-calendar fa-lg" path="events.aspx" />
            <TopbarMenuLink title="Help" icon="fa fa-info-circle fa-lg" path="help.aspx" />
            <TopbarMenuLink title="Sign Out" icon="fa fa-sign-out fa-lg" path="logout.aspx" />
          </div>
        </Collapse>
      </div>
    );
  }
}
