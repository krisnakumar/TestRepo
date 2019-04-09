/* eslint-disable */
import React, { PureComponent } from 'react';
import TopbarProfile from './TopbarProfile';
// import TopbarNotification from './TopbarNotification';

const logo = `https://d2vkqsz7y0fh3j.cloudfront.net/img/content_logo.png?v=2`;

class Topbar extends PureComponent {
  render() {
    let companyName = "",
    companyLogo = "";

    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails || '{}');
    companyName = contractorManagementDetails.Company ? contractorManagementDetails.Company.Name : "",
    companyLogo = contractorManagementDetails.Company ? contractorManagementDetails.Company.Logo : "";

    return (
      <div className="topbar">
        <div className="topbar__wrapper">
          <div className="topbar__left">
            <a className="topbar__left-logo" href={window.location.origin}>
              <img src={logo} alt="" />
            </a>
            <div className="comp">
              <span id="ctl00_compname" ><img src={companyLogo} />{companyName}</span>
            </div>
          </div>
          <div className="topbar__right">
            {/* <TopbarNotification /> */}
            <TopbarProfile />
          </div>
        </div>
      </div>
    );
  }
}

export default Topbar;
