/* eslint-disable */
import React, { PureComponent } from 'react';
import { Collapse } from 'reactstrap';
import TopbarMenuLink from './TopbarMenuLink';

const Ava = "img/ava.png";

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
    let { contractorManagementDetails, userPhoto } = sessionStorage || '{}',
        userName = "",
        userProfileLink = "/userProfile.aspx";
    if(contractorManagementDetails){
      contractorManagementDetails = JSON.parse(contractorManagementDetails);
      userName = contractorManagementDetails.User.FullName || "",
      userProfileLink = contractorManagementDetails.User.Url || "/userProfile.aspx";
    }

    let userDisplayPicture = userPhoto ? userPhoto : Ava;
 
    return (
      <div className="topbar__profile">
        <button className="topbar__avatar" onClick={this.toggle}>
          <p className="topbar__avatar-name">{userName}</p>
          <img className="topbar__avatar-img" src={userDisplayPicture} alt="avatar" />
          <i id="downArrow" className="fa fa-caret-down" title="Down Arrow" aria-hidden="true" />
        </button>
        {this.state.collapse && <button className="topbar__back" onClick={this.toggle} />}
        <Collapse isOpen={this.state.collapse} className="topbar__menu-wrap">
          <div className="topbar__menu user-menu">
            <TopbarMenuLink title="User Profile" icon="fa fa-user-circle-o fa-lg" path={userProfileLink} />
            <TopbarMenuLink title="Dashboard" icon="fa fa-tachometer fa-lg" path="/DashBoard.aspx" />
            <TopbarMenuLink title="Events" icon="fa fa-calendar fa-lg" path="/events.aspx" />
            <TopbarMenuLink title="Help" icon="fa fa-info-circle fa-lg" path="/help.aspx" />
            <TopbarMenuLink title="Sign Out" icon="fa fa-sign-out fa-lg" path="/logout.aspx" />
          </div>
        </Collapse>
      </div>
    );
  }
}
