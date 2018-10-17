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
import { DropdownItem, DropdownToggle, DropdownMenu, UncontrolledDropdown, Dropdown } from 'reactstrap';
import TopbarNavLink from './tobar_nav/TopbarNavLink';

export default class TopbarWithNavigation extends PureComponent {
  constructor(props) {
    super(props);

    this.toggle = this.toggle.bind(this);
    this.onMouseEnter = this.onMouseEnter.bind(this);
    this.onMouseLeave = this.onMouseLeave.bind(this);
    this.state = {
      menuStatus: {},
      dropdownOpen: false
    };
  }

  toggle(mainMenu) {
    this.setState(prevState => ({
      dropdownOpen: !prevState.dropdownOpen
    }));
  }

  onMouseEnter(mainMenu, event) {
    this.state[mainMenu] = true;
    this.setState({ dropdownOpen: true });   
  }

  onMouseLeave(mainMenu, event) {
    this.state[mainMenu] = false;
    this.setState({ dropdownOpen: false });
  }

  render() {
    var nav_menus = Menus,
        _self = this;
    return (
      <div className="topbar topbar--navigation menu-navigation">
        <div className="topbar__wrapper">
          <div className="topbar__right">   
            <nav className="topbar__nav">
                    {
                       nav_menus.map(function(mainMenu, index) {                         
                        let menuName = mainMenu.name;
                        _self.state[menuName] = false;
                        return (
                          <UncontrolledDropdown key={index} 
                                    className="topbar__nav-dropdown" 
                                    onMouseOver={_self.onMouseEnter.bind(event, menuName)} 
                                    onMouseLeave={_self.onMouseLeave.bind(event, menuName)} 
                                    isOpen={_self.state[mainMenu.name]} >
                            <DropdownToggle className="topbar__nav-dropdown-toggle">
                              <img alt={mainMenu.name} src={mainMenu.iconUrl}/> 
                              {mainMenu.name}
                            </DropdownToggle>
                            <DropdownMenu className="topbar__nav-dropdown-menu dropdown__menu">
                            {
                              mainMenu.subMenu.map((menu, index) => (
                                <DropdownItem key={menu.name+index}>
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
