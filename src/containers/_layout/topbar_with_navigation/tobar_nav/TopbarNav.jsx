import React from 'react';
// import { Link } from 'react-router-dom';

import { DropdownItem, DropdownToggle, DropdownMenu, UncontrolledDropdown } from 'reactstrap';
import TopbarNavDashboards from './TopbarNavDashboards';
import TopbarNavUIElements from './TopbarNavUIElements';
import TopbarNavOtherPages from './TopbarNavOtherPages';
import TopbarNavLink from './TopbarNavLink';

const TopbarNav = () => (
  <nav className="topbar__nav">
    <TopbarNavDashboards />
    <TopbarNavUIElements />
    <TopbarNavOtherPages />
    {/* <Link className="topbar__nav-link" to="/documentation/introduction">Documentation</Link> */}
    <UncontrolledDropdown className="topbar__nav-dropdown">
      <DropdownToggle className="topbar__nav-dropdown-toggle">
            Dashboards
        {/* <DownIcon /> */}
      </DropdownToggle>
      <DropdownMenu className="topbar__nav-dropdown-menu dropdown__menu">
        <DropdownItem>
          <TopbarNavLink title="Dashboard Default" icon="home" route="/dashboard_default" />
        </DropdownItem>
        <DropdownItem>
          <TopbarNavLink title="Dashboard E-commerce" icon="store" route="/dashboard_e_commerce" />
        </DropdownItem>
        <DropdownItem>
          <TopbarNavLink title="Dashboard Fitness" icon="heart-pulse" newLink route="/dashboard_fitness" />
        </DropdownItem>
        <DropdownItem>
          <TopbarNavLink title="Dashboard Crypto" icon="rocket" newLink route="/dashboard_crypto" />
        </DropdownItem>
      </DropdownMenu>
    </UncontrolledDropdown>
  </nav>
);

export default TopbarNav;
