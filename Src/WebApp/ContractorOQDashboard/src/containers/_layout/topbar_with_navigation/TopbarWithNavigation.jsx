/* eslint-disable */
import React, { PureComponent } from 'react';
import { DropdownItem, DropdownToggle, DropdownMenu, UncontrolledDropdown, Dropdown } from 'reactstrap';

export default class TopbarWithNavigation extends PureComponent {
  constructor(props) {
    super(props);

    this.onMouseEnter = this.onMouseEnter.bind(this);
    this.onMouseLeave = this.onMouseLeave.bind(this);
    this.state = {
      isHover: [],
      dropdownOpen: false
    };
  }

  // componentWillMount(){
  //   var nav_menus = JSON.parse(localStorage.getItem('menus')),
  //       _self = this;
  //   if(nav_menus !== null) {
  //       nav_menus.map(function(mainMenu, index) {  
  //         _self.state.isHover[index] =  false
  //       });
  //     }
  // }

  onMouseEnter(i) {
    return () => {
      if (this.state.isHover[i] === true) {
        return this.state;
      }
      let isHover = [...this.state.isHover]
      isHover[i] = true;
      this.setState({ ...this.state, isHover });
    }
  }

  onMouseLeave(i) {
    return () => {
      if (this.state.isHover[i] === false) {
        return this.state;
      }
      let isHover = [...this.state.isHover]
      isHover[i] = false;
      this.setState({ ...this.state, isHover });
    }
  }

  render() {
    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails || '{}');
    let nav_menus = contractorManagementDetails.Menu || {},
    _self = this;
    return (
      <div className="topbar topbar--navigation menu-navigation">
        <div className="topbar__wrapper">
          <div className="topbar__right">   
            <nav className="topbar__nav">
                    {                      
                       nav_menus &&
                        Object.values(nav_menus).map(function(mainMenu, index) {             
                          let menuName = mainMenu.Title;
                          let basePath = window.location.origin;
                          return (
                            <UncontrolledDropdown key={index} 
                                      className="topbar__nav-dropdown" 
                                      onMouseOver={_self.onMouseEnter(index)} 
                                      onMouseLeave={_self.onMouseLeave(index)}
                                      isOpen={_self.state.isHover[index]} >
                              <DropdownToggle className="topbar__nav-dropdown-toggle" href= {basePath + mainMenu.Url}>
                                <img alt={menuName} src={mainMenu.Icon}/> 
                                {menuName}
                              </DropdownToggle>
                              <DropdownMenu className="topbar__nav-dropdown-menu dropdown__menu">
                              {
                                mainMenu.Items.map((menu, index) => (
                                <div key={menu.Title+index}>
                                  <a className= "topbar__link" title={menu.Title} href={menu.isRedirect == false ? menu.Url : basePath + menu.Url}>
                                      <p className="topbar__link-title">{menu.Title}</p>
                                  </a> 
                                </div>
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
