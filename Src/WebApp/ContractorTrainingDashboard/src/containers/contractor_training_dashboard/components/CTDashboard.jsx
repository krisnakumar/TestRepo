/* eslint-disable */
/*
* CTDashboard.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will be used to render Contractor Training Dashboard
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------
createRows()
handleGridRowsUpdated(fromRow, toRow, updated)
handleGridSort(sortColumn, sortDirection)
*/
import React, { PureComponent } from 'react';
import { Collapse, Button, CardBody, Card, Row, Col, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import 'whatwg-fetch'
import update from 'immutability-helper';
import { WithContext as ReactTags } from 'react-tag-input';
import * as API from '../../../shared/utils/APIUtils';
import ContractorCompanyDetail from './ContractorCompanyDetail';
import FilterModal from './FilterModal';
import CompanyFilterModal from './CompanyFilterModal';
import Export from './CTDashboardExport';
import SessionPopup from './SessionPopup';
import * as Constants from '../../../shared/constants';

// Import React Table
import ReactTable from "react-table";
import "react-table/react-table.css";

/**
 * DataTableEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-table module.
 */

class CTDashboard extends PureComponent {
  constructor() {
    super();
    this.heads = [
      {
        key: 'role',
        name: 'Role',
        sortable: true,
        editable: false,
        width: 700,
        cellClass: "text-left"
      },
      {
        key: 'incompleteCompanies',
        name: 'Incomplete Companies',
        sortable: true,
        editable: false,
        cellClass: "text-center"
      },
      {
        key: 'completedCompanies',
        name: 'Completed Companies',
        sortable: true,
        editable: false,
        cellClass: "text-center last-column padding-rt-2p"
      }
    ];
    this.roleFilter = React.createRef();
    this.companyFilter = React.createRef();
    this.state = {
      rows: this.createRows([]),
      isInitial: false,
      selectedRole: "",
      companyDetails: {},
      isCompanyDetailsModal: false,
      collapse: false,
      collapseText: "More Options",
      isFilterModal: false,
      filterModalTitle: "Roles",
      filteredRoles: [],
      filterOptionsRoles: [],
      isCompanyFilterModal: false,
      companyFilterTitle: "Companies",
      filteredCompanies: [],
      filterOptionsCompanies: [],
      isReloadWindow: false,
      isSessionPopup: false,
      sessionPopupType: "API"
    };
    this.toggle = this.toggle.bind(this);
    this.toggleFilter = this.toggleFilter.bind(this);
    this.handleRoleDelete = this.handleRoleDelete.bind(this);
    this.toggleCompanyFilter = this.toggleCompanyFilter.bind(this);
    this.handleCompanyDelete = this.handleCompanyDelete.bind(this);
    this.customCell = this.customCell.bind(this);
  }

  /**
  * @method
  * @name - cellFormatter
  * This method will format the cell column other than workbooks Data Grid
  * @param props
  * @returns none
  */
  cellFormatter = (props) => {
    return (
      <span>{props.value}</span>
    );
    ;
  }

  /**
   * @method
   * @name - roleDetailsFormatter
   * This method will format the cell column other than CT Data Grid
   * @param type
   * @param props
   * @returns react element
  */
  roleDetailsFormatter = (type, props) => {
    if (props.dependentValues[type] <= 0) {
      return (
        <span>{props.value}</span>
      );
    } else {
      return (
        <span onClick={e => { e.preventDefault(); this.handleCellClick(type, props.dependentValues); }} className={"text-clickable"}>
          {props.value}
        </span>
      );
    }
  };

  /**
   * @method
   * @name - componentDidCatch
   * This method will catch all the exceptions in this class
   * @param error
   * @param info
   * @returns none
   */
  componentDidCatch(error, info) {
    // Display fallback UI
    // this.setState({ hasError: true });
    // You can also log the error to an error reporting service
    console.log(error, info);
  }

  /**
   * @method
   * @name - componentDidMount
   * This method will invoked whenever the component is mounted
   *  is update to this component class
   * @param none
   * @returns none
  */
  async componentDidMount() {
    let roles = [],
      companies = [];

    let { dashboardAPIToken } = sessionStorage,
      idToken = '';

    if (dashboardAPIToken) {
      dashboardAPIToken = JSON.parse(dashboardAPIToken);
      idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";
    }
    if (idToken) {
      this.setState({ isReloadWindow: false });
      // await this.getCompanyFilterOptions();
      await this.getFilterOptions();
      this.getRoles(roles, companies);
    } else {
      this.setState({ isReloadWindow: true });
    }

  };

  /**
   * @method
   * @name - updateModalState
   * This method will update the modal window state of parent
   * @param modelName
   * @returns none
   */
  updateModalState = (modelName) => {
    let value = !this.state[modelName];
    this.setState({
      [modelName]: value,
      selectedRole: ""
    });
  };

  /**
  * @method
  * @name - getEmployees
  * This method will used to get Employees details
  * @param roles
  * @returns none
  */
  async getRoles(roles, companies) {
    const { cookies } = this.props;
    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let adminId = parseInt(contractorManagementDetails.User.Id) || 0;
    let rolesLength = roles.length,
      companiesLength = companies.length,
      fields = [{ "Name": "IS_SHARED", "Value": 1, "Operator": "=" }, { "Name": "ADMIN_ID", "Value": adminId, "Operator": "=", "Bitwise": "and" }];
    if (rolesLength > 0) {
      let roleIds = roles.join();
      let roleField = { "Name": "ROLES", "Value": roleIds, "Operator": "=", "Bitwise": "AND" };
      fields.push(roleField);
    }
    if (companiesLength > 0) {
      let companyIds = companies.join();
      let companyField = { "Name": "COMPANIES", "Value": companyIds, "Operator": "=", "Bitwise": "AND" };
      fields.push(companyField);
    }
    const postData = {
      "Fields": fields,
      "ColumnList": ['COMPLETED_ROLE_QUALIFICATION', 'NOT_COMPLETED_ROLE_QUALIFICATION', 'ROLE_ID', 'TRAINING_ROLE'],
      "AppType": "TRAINING_DASHBOARD"
    };
    let { dashboardAPIToken } = sessionStorage || {};
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

    let companyId = contractorManagementDetails.Company.Id || 0;

    let token = idToken,
      url = "/company/" + companyId + "/tasks",
      response = await API.ProcessAPI(url, postData, token, false, "POST", true),
      rows = this.createRows(response),
      isInitial = true;

    if (response == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if (response == 'API_ERROR') {
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {
      this.setState({ rows: rows, isInitial: isInitial });
    }

  };

  /**
  * @method
  * @name - getFilterOptions
  * This method will used to get Filter Options
  * @param none
  * @returns none
  */
  async getFilterOptions() {
    const { cookies } = this.props;
    let { dashboardAPIToken } = sessionStorage || {};
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let companyId = contractorManagementDetails.Company.Id || 0;
    const postData = {
      "AppType": "TRAINING_DASHBOARD"
    };
    let token = idToken,
      url = "/company/" + companyId + "/roles",
      response = await API.ProcessAPI(url, postData, token, false, "POST", true);

    if (response == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if (response == 'API_ERROR') {
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {
      response = JSON.parse(JSON.stringify(response).split('"Role":').join('"text":'));
      response = JSON.parse(JSON.stringify(response).split('"RoleId":').join('"id":'));
      Object.keys(response).map(function (i) { response[i].id ? response[i].id = response[i].id.toString() : "" });
      this.setState({ filterOptionsRoles: response });
    }

  };

  /**
  * @method
  * @name - getCompanyFilterOptions
  * This method will used to get Filter Options
  * @param none
  * @returns none
  */
  async getCompanyFilterOptions() {
    const { cookies } = this.props;
    let { dashboardAPIToken } = sessionStorage || {};
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";
    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let companyId = contractorManagementDetails.Company.Id || 0;
    let token = idToken,
      url = "/companies",
      response = await API.ProcessAPI(url, "", token, false, "GET", true);

    if (response == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if (response == 'API_ERROR') {
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {
      response = JSON.parse(JSON.stringify(response).split('"CompanyName":').join('"text":'));
      response = JSON.parse(JSON.stringify(response).split('"CompanyId":').join('"id":'));
      Object.keys(response).map(function (i) { response[i].id ? response[i].id = response[i].id.toString() : "" });
      this.setState({ filterOptionsCompanies: response });
    }

  };

  /**
 * @method
 * @name - getInCompleteCompanies
 * This method will used to get InComplete Companies details
 * @param userId
 * @returns none
 */
  async getCompanyDetails(role, roleId, isCompleted) {
    const { cookies } = this.props;
    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    // get the company Id from the session storage 
    let adminId = parseInt(contractorManagementDetails.User.Id) || 0;
    let companyId = contractorManagementDetails.Company.Id || 0,
      fields = [{ "Name": "ROLE_ID", "Value": roleId, "Operator": "=" }, { "Name": "ADMIN_ID", "Value": adminId, "Operator": "=", "Bitwise": "and" }];

    if (isCompleted) {
      fields.push({ "Name": "STATUS", "Value": "COMPLETED_COMPANY_USERS", "Operator": "=", "Bitwise": "and" });
    } else {
      fields.push({ "Name": "STATUS", "Value": "NOT_COMPLETED_COMPANY_USERS", "Operator": "=", "Bitwise": "and" });
    }
    let isCompanyDetailsModal = this.state.isCompanyDetailsModal,
      companyDetails = {},
      selectedRole = role;
    isCompanyDetailsModal = true;
    this.setState({ isCompanyDetailsModal, companyDetails, selectedRole });
    let { dashboardAPIToken } = sessionStorage || {};
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";
    const postData = {
      "Fields": fields,
      "ColumnList": ['NOT_COMPLETED_COMPANY_USERS', 'COMPLETED_COMPANY_USERS', 'TOTAL_COMPLETED_COMPANY_USERS', 'COMPANY_NAME', 'COMPANY_ID', 'ROLE_ID'],
      "AppType": "TRAINING_DASHBOARD"
    };
    let token = idToken,
      url = "/company/" + companyId + "/tasks",
      response = await API.ProcessAPI(url, postData, token, false, "POST", true);

    if (response == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if (response == 'API_ERROR') {
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {
      companyDetails = response;
      isCompanyDetailsModal = true;
      this.setState({ ...this.state, isCompanyDetailsModal, companyDetails });
    }
  };

  /**
  * @method
  * @name - createRows
  * This method will format the input data
  * for Data Grid
  * @param roleDetails
  * @returns rows
  */
  createRows = (roleDetails) => {
    const rows = [],
      length = roleDetails ? roleDetails.length : 0;
    for (let i = 0; i < length; i++) {
      rows.push({
        roleId: parseInt(roleDetails[i].RoleId || 0),
        role: roleDetails[i].Role || "",
        incompleteCompanies: parseInt(roleDetails[i].InCompletedRoleQualification || 0),
        completedCompanies: parseInt(roleDetails[i].CompletedRoleQualification || 0)
      });
    }

    return rows;
  };

  /**
  * @method
  * @name - handleCellClick
  * This method will trigger the event of API's respective to cell clicked Data Grid
  * @param type
  * @param args
  * @returns none
  */
  handleCellClick = (type, args) => {
    let roleId = args.roleId || 0,
      role = args.role || "",
      title = (type == "incompleteCompanies" ? role + " - Incomplete" : role + " - Completed"),
      isCompleted = type == "completedCompanies";
    switch (type) {
      case "incompleteCompanies":
      case "completedCompanies":
        this.getCompanyDetails(title, roleId, isCompleted);
        break;
      default:
        break;
    }
    // this.refs.contractorDashboardDataGrid.deselect();
  };

  /**
  * @method
  * @name - toggle
  * This method will used show or hide the modal popup
  * @param none
  * @returns none
  */
  toggle() {
    let collapseText = this.state.collapse ? "More Options" : "Less Options";
    this.setState(state => ({ collapse: !state.collapse, collapseText: collapseText }));
  }

  /**
  * @method
  * @name - toggleFilter toggleCompanyFilter
  * This method will used show or hide the filter modal popup
  * @param none
  * @returns none
  */
  toggleFilter() {
    this.setState(state => ({ isFilterModal: true }));
  }


  /**
  * @method
  * @name - toggleCompanyFilter 
  * This method will used show or hide the filter modal popup
  * @param none
  * @returns none
  */
  toggleCompanyFilter() {
    this.setState(state => ({ isCompanyFilterModal: true }));
  }

  /**
   * @method
   * @name - updateSelectedData
   * This method will update the selected role on state
   * @param selectedData
   * @returns none
  */
  updateSelectedData = (selectedData) => {
    this.setState({
      filteredRoles: selectedData
    });
  };

  /**
   * @method
   * @name - updateCompanySelectedData
   * This method will update the selected role on state
   * @param selectedData
   * @returns none
  */
  updateCompanySelectedData = (selectedData) => {
    this.setState({
      filteredCompanies: selectedData
    });
  };

  /**
  * @method
  * @name - handleRoleDelete
  * This method will delete the selected roles and update it on state
  * @param i
  * @returns none
  */
  handleRoleDelete(i) {
    let { filteredRoles } = this.state;
    filteredRoles = filteredRoles.filter((tag, index) => index !== i)
    this.setState({
      filteredRoles: filteredRoles,
    });
    if(this.roleFilter.current){
      this.roleFilter.current.selectMultipleOption(true, this, filteredRoles);
    }
  }

  /**
  * @method
  * @name - handleCompanyDelete
  * This method will delete the selected company and update it on state
  * @param i
  * @returns none
  */
  handleCompanyDelete(i) {
    let { filteredCompanies } = this.state;
    filteredCompanies = filteredCompanies.filter((tag, index) => index !== i)
    this.setState({
      filteredCompanies: filteredCompanies,
    });
    if(this.companyFilter.current){
      this.companyFilter.current.selectMultipleOption(true, this, filteredCompanies);
    }
  }

  /** handleCompanyDelete
  * @method
  * @name - filterGoAction
  * This method will update the selected role on state
  * @param none
  * @returns none
 */
  filterGoAction = () => {
    let filteredRoles = this.state.filteredRoles,
      filteredCompanies = this.state.filteredCompanies,
      roles = [],
      companies = [];
    Object.keys(filteredRoles).map(function (i) { roles.push(filteredRoles[i].id) });
    Object.keys(filteredCompanies).map(function (i) { companies.push(filteredCompanies[i].id) });
    this.getRoles(roles, companies);
  };

  customCell(props) {
    let self = this;
    return (
      props.value && <span onClick={e => { e.preventDefault(); self.handleCellClick(props.column.id, props.original); }} className={"text-clickable"}>
        {props.value}
      </span> || <span>{props.value}</span>
    );
  }

  autoLogout() {
    window.location = window.location.origin + "/Logout.aspx";
  };

  render() {
    const { rows, collapseText, collapse, filteredRoles, filteredCompanies } = this.state;
    let collapseClassName = (collapse ? "show" : "hide"),
      filteredRolesLength = filteredRoles.length,
      filteredCompaniesLength = filteredCompanies.length;
    let basePath = window.location.origin || "";
    let pgSize = (rows.length > 10) ? rows.length : rows.length;
    return (
      <CardBody>
        <SessionPopup
          backdropClassName={"backdrop"}
          modal={this.state.isSessionPopup}
          sessionPopupType={this.state.sessionPopupType}
        />
        <Modal backdrop={"static"} isOpen={this.state.isReloadWindow} toggle={this.toggle} fade={false} centered={true} className="auto-logout-modal">
          <ModalHeader> Alert</ModalHeader>
          <ModalBody>Your session has expired. Please login again</ModalBody>
          <ModalFooter>
            <button color="primary" onClick={this.autoLogout}>Go to Login</button>{' '}
          </ModalFooter>
        </Modal>
        <ContractorCompanyDetail
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isCompanyDetailsModal}
          companyDetails={this.state.companyDetails}
          title={this.state.selectedRole}
        />
        <FilterModal
          ref={this.roleFilter}
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          updateSelectedData={this.updateSelectedData.bind(this)}
          modal={this.state.isFilterModal}
          title={this.state.filterModalTitle}
          filterOptionsRoles={this.state.filterOptionsRoles}
        />
        <CompanyFilterModal
          ref={this.companyFilter}
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          updateCompanySelectedData={this.updateCompanySelectedData.bind(this)}
          modal={this.state.isCompanyFilterModal}
          title={this.state.companyFilterTitle}
          filterOptionsCompanies={this.state.filterOptionsCompanies}
        />
        <div className="card__title">
          <div className="breadcrumbs noprint"><a href={basePath + "/default.aspx"}>Home</a>&gt;<a href={basePath + "/ReportsLanding.aspx"}>Reports</a></div>
          <Export
            data={this.state.rows}
            heads={this.heads}
            sheetName={"OnBoard LMS Training Dashboard"}
          />
          <div className="pageheader">
            <img src="https://d2tqbrn06t95pa.cloudfront.net/img/topnav_reports.png?v=2" /> Contractor Training Dashboard
            </div>
          <p className="card__description">Displays contractors' training progress required by role (job function). This report displays current records for the contractor employee's task profile (if a training task is not in the employee task profile it is not counted on this report).</p>
        </div>
        <div className="grid-filter-section">
          <div className={"grid-filter-collapse " + collapseClassName}>Filters: <button className="btn-as-text" onClick={this.toggle} >{collapseText}</button></div>
          <Collapse className="grid-filter-collapse-body" isOpen={collapse}>
            <Row className="collapse-body-row">
              <Col xs="1"><label>Role:</label></Col>
              <Col xs="auto">
                {
                  filteredRolesLength <= 0 && <input value="ALL" disabled className="text-center" />

                  ||

                  <ReactTags
                    tags={filteredRoles}
                    handleDelete={this.handleRoleDelete}
                    handleDrag={console.log()}
                  />
                }
              </Col>
              <Col xs="1"><button className="btn-as-text" onClick={this.toggleFilter} >Change</button></Col>
            </Row>
            <Row className="collapse-body-row">
              <Col xs="1"><label></label></Col>
              <Col xs="auto"><button className="grid-filter-go-btn" size="sm" onClick={this.filterGoAction} >Go</button></Col>
              <Col xs="1"></Col>
            </Row>
          </Collapse>
        </div>
        <div className="grid-container">
          <div className="section-info-view">
            <div className="section-info-title">
              <div className="section-info-pageheader">Progress by Role</div>
              <p className="section-info-description">Complete =  Number of contractor companies that have users in a role, who have completed all the training tasks in the role complete.</p>
            </div>
            <div className="table has-section-view is-table-page-view">
              <ReactTable
                minRows={1}
                data={rows}
                columns={[
                  {
                    Header: "Role",
                    accessor: "role",
                    headerClassName: 'header-wordwrap',
                    // width: 200
                    minWidth: 400,
                    className: 'text-left'
                  },
                  {
                    Header: "Incomplete Companies",
                    id: "incompleteCompanies",
                    accessor: "incompleteCompanies",
                    headerClassName: 'header-wordwrap',
                    minWidth: 200,
                    maxWidth: 300,
                    className: 'text-center',
                    Cell: this.customCell
                  },
                  {
                    Header: "Completed Companies",
                    id: "completedCompanies",
                    accessor: "completedCompanies",
                    headerClassName: 'header-wordwrap',
                    minWidth: 200,
                    maxWidth: 300,
                    className: 'text-center',
                    Cell: this.customCell
                  }
                ]
                }
                resizable={false}
                className="-striped -highlight"
                showPagination={false}
                showPaginationTop={false}
                showPaginationBottom={false}
                showPageSizeOptions={false}
                pageSizeOptions={[5, 10, 20, 25, 50, 100]}
                pageSize={!this.state.isInitial ? 5 : pgSize}
                loading={!this.state.isInitial}
                loadingText={''}
                noDataText={!this.state.isInitial ? '' : 'Sorry, no records'}
                defaultSorted={[
                  {
                    id: "role",
                    desc: false
                  }
                ]}
              />
            </div>
          </div>
        </div>
      </CardBody>
    );
  }
}

export default CTDashboard;
