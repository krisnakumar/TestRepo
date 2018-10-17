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
    var nav_menus = Menus;

    return (
      <div className="topbar topbar--navigation menu-navigation">
        <div className="topbar__wrapper">
          <div className="topbar__right">   
            <nav className="topbar__nav">
                    {
                       nav_menus.map(function(mainMenu, index) {
                        console.log(mainMenu)
                        return (
                          <UncontrolledDropdown key={index} className="topbar__nav-dropdown">
                            <DropdownToggle className="topbar__nav-dropdown-toggle">
                              <img alt={mainMenu.name} src={mainMenu.iconUrl}/> 
                              {mainMenu.name}
                            </DropdownToggle>
                            <DropdownMenu className="topbar__nav-dropdown-menu dropdown__menu">
                            {
                              mainMenu.subMenu.map((menu, index) => (
                                <DropdownItem key={menu.name}>
                                  <TopbarNavLink title={menu.name} route={"/"+menu.href} />
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
