import React, { PureComponent } from 'react';
import DownIcon from 'mdi-react/ChevronDownIcon';
import { Collapse } from 'reactstrap';
import TopbarMenuLink from './TopbarMenuLink';

const Ava = `${process.env.PUBLIC_URL}/img/ava.png`;

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
          <DownIcon className="topbar__icon" />
        </button>
        {this.state.collapse && <button className="topbar__back" onClick={this.toggle} />}
        <Collapse isOpen={this.state.collapse} className="topbar__menu-wrap">
          <div className="topbar__menu">
            <TopbarMenuLink title="User Profile" icon="user" path="/account/profile" />
            <TopbarMenuLink title="Dashboard" icon="calendar-full" path="/default_pages/calendar" />
            <TopbarMenuLink title="Events" icon="list" path="/default_pages/calendar" />
            <TopbarMenuLink title="Help" icon="inbox" path="/mail" />
            <TopbarMenuLink title="Sign Out" icon="exit" path="/log_in" />
          </div>
        </Collapse>
      </div>
    );
  }
}
