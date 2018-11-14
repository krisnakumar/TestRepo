/* eslint-disable */
import React from 'react';
import { Link } from 'react-router-dom';
import TopbarNavDashboards from './TopbarNavDashboards';
import TopbarNavUIElements from './TopbarNavUIElements';
import TopbarNavOtherPages from './TopbarNavOtherPages';
import { DropdownItem, DropdownToggle, DropdownMenu, UncontrolledDropdown } from 'reactstrap';
import TopbarNavLink from './tobar_nav/TopbarNavLink';
import MenuJSON from '.../Menus.json';

const TopbarNavCustom = () => (
  <nav className="topbar__nav">
   { 
              Object.keys(MenuJSON.Menus).forEach(function(key) {
                return (<UncontrolledDropdown className="topbar__nav-dropdown">
                  <DropdownToggle className="topbar__nav-dropdown-toggle">
                       {key}
                  </DropdownToggle>
                  <DropdownMenu className="topbar__nav-dropdown-menu dropdown__menu">
                    {
                      // menus[key].map(item => 
                      //   <DropdownItem><TopbarNavLink title={item} icon="home" route={item}/></DropdownItem>
                      // )
                    }
                  </DropdownMenu>
                </UncontrolledDropdown>)
              })
            }
  </nav>
);

export default TopbarNavCustom;
