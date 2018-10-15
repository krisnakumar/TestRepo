/* eslint-disable */
import React, { PureComponent } from 'react';
import { Link } from 'react-router-dom';
import PropTypes from 'prop-types';
import TopbarSidebarButton from './TopbarSidebarButton';
import TopbarProfile from './TopbarProfile';
import TopbarMail from './TopbarMail';
import TopbarLanguage from './TopbarLanguage';
import TopbarNotification from './TopbarNotification';
import TopbarNav from './tobar_nav/TopbarNav';
import Menus from '../Menus.json';
import { DropdownItem, DropdownToggle, DropdownMenu, UncontrolledDropdown } from 'reactstrap';
import TopbarNavLink from './tobar_nav/TopbarNavLink';

export default class TopbarWithNavigation extends PureComponent {

  componentDidMount(){
    console.log(Menus);
  };

  render() {
    var nav_menus = Menus.Menu;

    return (
      <div className="topbar topbar--navigation menu-navigation">
        <div className="topbar__wrapper">
          <div className="topbar__right">   
            <nav className="topbar__nav">
                    {
                      Object.keys(nav_menus).map(function(key) {
                        return (
                          <UncontrolledDropdown key={key} className="topbar__nav-dropdown">
                            <DropdownToggle className="topbar__nav-dropdown-toggle">
                              {key}
                            </DropdownToggle>
                            <DropdownMenu className="topbar__nav-dropdown-menu dropdown__menu">
                            {
                              nav_menus[key].map((menu, index) => (
                                <DropdownItem key={menu+index}>
                                  <TopbarNavLink title={menu} route={"/"+menu.trim()} />
                                </DropdownItem>
                              ))
                            }
                          </DropdownMenu>
                          </UncontrolledDropdown>
                        )
                      })
                    }                  
            </nav>
          </div>
        </div>
      </div>
    );
  }
}
