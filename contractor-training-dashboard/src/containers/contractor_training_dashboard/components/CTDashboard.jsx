/* eslint-disable */
/*
* CTDashboard.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details to list the workbooks 
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------
createRows()
handleGridRowsUpdated(fromRow, toRow, updated)
handleGridSort(sortColumn, sortDirection)
*/
import React, { PureComponent } from 'react';
import { Collapse, Button, CardBody, Card, Row, Col } from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import { WithContext as ReactTags } from 'react-tag-input';
import * as API from '../../../shared/utils/APIUtils';
import ContractorCompanyDetail from './ContractorCompanyDetail';
import FilterModal from './FilterModal';
import Export from './CTDashboardExport';

/**
 * DataTableEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-table module.
 */
class DataTableEmptyRowsView extends React.Component {
  render() {
    return (<div className="no-records-found">Sorry, no records</div>)
  }
};
class CTDashboard extends PureComponent {

  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor() {
    super();
    this.heads = [
      {
        key: 'role',
        name: 'Role',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'incompleteCompanies',
        name: 'Incomplete Companies',
        sortable: true,
        width: 400,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.roleDetailsFormatter("incompleteCompanies", props),
        cellClass: "text-right"
      },
      {
        key: 'completedCompanies',
        name: 'Completed Companies',
        sortable: true,
        width: 400,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.roleDetailsFormatter("completedCompanies", props),
        cellClass: "text-right last-column padding-rt-2p"
      }
    ];
    this.roleFilter = React.createRef();
    this.state = {
      rows: this.createRows([]),
      isInitial: false,
      selectedRole: "",
      companyDetails: {},
      isCompanyDetailsModal: false,
      collapse: false,
      collapseText: "More Options",
      isFilterModal: false,
      filterModalTitle: "Role",
      filteredRoles: [],
      filterOptionsRoles: [],
    };
    this.toggle = this.toggle.bind(this);
    this.toggleFilter = this.toggleFilter.bind(this);
    this.handleRoleDelete = this.handleRoleDelete.bind(this);

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
    let roles = [];
    await this.getFilterOptions();
    this.getRoles(roles);
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
  async getRoles(roles) {
    const { cookies } = this.props;
    let rolesLength = roles.length,
      fields = [{ "Name": "IS_SHARED", "Value": 1, "Operator": "=" }];
    if (rolesLength > 0) {
      let roleIds = roles.join();
      let roleField = { "Name": "ROLES", "Value": roleIds, "Operator": "=", "Bitwise": "AND" };
      fields.push(roleField);
    }
    const postData = {
      "Fields": fields,
      "ColumnList": ['COMPLETED_ROLE_QUALIFICATION', 'NOT_COMPLETED_ROLE_QUALIFICATION', 'ROLE']
    };

    let token = cookies.get('IdentityToken'),
      companyId = cookies.get('CompanyId'),
      url = "/company/" + companyId + "/tasks",
      response = await API.ProcessAPI(url, postData, token, false, "POST", true),
      rows = this.createRows(response),
      isInitial = true;

    this.setState({ rows: rows, isInitial: isInitial });
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

    let token = cookies.get('IdentityToken'),
      companyId = cookies.get('CompanyId'),
      url = "/company/" + companyId + "/roles",
      response = await API.ProcessAPI(url, "", token, false, "GET", true);
    response = JSON.parse(JSON.stringify(response).split('"Role":').join('"text":'));
    response = JSON.parse(JSON.stringify(response).split('"RoleId":').join('"id":'));
    Object.keys(response).map(function (i) { response[i].id ? response[i].id = response[i].id.toString() : "" });
    this.setState({ filterOptionsRoles: response });
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
    let companyId = cookies.get('CompanyId'),
      fields = [{ "Name": "ROLE_ID", "Value": roleId, "Operator": "=" }];

    if (isCompleted) {
      fields.push({ "Name": "COMPLETED_COMPANY_USERS", "Value": "true", "Operator": "=", "Bitwise": "and" });
    } else {
      fields.push({ "Name": "NOT_COMPLETED_COMPANY_USERS", "Value": "true", "Operator": "=", "Bitwise": "and" });
    }

    const postData = {
      "Fields": fields,
      "ColumnList": ['NOT_COMPLETED_COMPANY_USERS', 'COMPLETED_COMPANY_USERS', 'TOTAL_COMPLETED_COMPANY_USERS', 'COMPANY_NAME', 'COMPANY_ID']
    };

    let isCompanyDetailsModal = this.state.isCompanyDetailsModal,
      companyDetails = {},
      selectedRole = role;
    isCompanyDetailsModal = true;
    this.setState({ isCompanyDetailsModal, companyDetails, selectedRole });

    let token = cookies.get('IdentityToken'),
      url = "/company/" + companyId + "/tasks",
      response = await API.ProcessAPI(url, postData, token, false, "POST", true);

    companyDetails = response;

    isCompanyDetailsModal = true;
    this.setState({ ...this.state, isCompanyDetailsModal, companyDetails });
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
   * @name - handleGridRowsUpdated
   * This method will update the rows of grid of the current Data Grid
   * @param fromRow
   * @param toRow
   * @param updated
   * @returns none
   */
  handleGridRowsUpdated = ({ fromRow, toRow, updated }) => {
    const rows = this.state.rows.slice();

    for (let i = fromRow; i <= toRow; i += 1) {
      const rowToUpdate = rows[i];
      rows[i] = update(rowToUpdate, { $merge: updated });
    }

    this.setState({ rows });
  };

  /**
   * @method
   * @name - handleGridSort
   * This method will update the rows of grid of Data Grid after the sort
   * @param sortColumn
   * @param sortDirection
   * @returns none
   */
  handleGridSort = (sortColumn, sortDirection) => {
    const comparer = (a, b) => {
      if (sortDirection === 'ASC') {
        return (a[sortColumn] >= b[sortColumn]) ? 1 : -1;
      } else if (sortDirection === 'DESC') {
        return (a[sortColumn] <= b[sortColumn]) ? 1 : -1;
      }
    };

    const sortRows = this.state.rows.slice(0),
      rowsLength = this.state.rows.length || 0;
    const rows = sortDirection === 'NONE' ? this.state.rows.slice(0, rowsLength) : sortRows.sort(comparer).slice(0, rowsLength);

    this.setState({ rows });
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
    this.refs.contractorDashboardDataGrid.deselect();
  };

  // This method is used to setting the row data in react data grid
  rowGetter = i => this.state.rows[i];

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
  * @name - toggleFilter
  * This method will used show or hide the filter modal popup
  * @param none
  * @returns none
  */
  toggleFilter() {
    this.setState(state => ({ isFilterModal: true }));
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
    this.roleFilter.current.selectMultipleOption(filteredRoles);
  }

  /**
  * @method
  * @name - filterGoAction
  * This method will update the selected role on state
  * @param none
  * @returns none
 */
  filterGoAction = () => {
    let filteredRoles = this.state.filteredRoles,
        roles = [];
    Object.keys(filteredRoles).map(function (i) { roles.push(filteredRoles[i].id) });
    this.getRoles(roles);
  };

  render() {
    const { rows, collapseText, collapse, filteredRoles } = this.state;
    let collapseClassName = (collapse ? "show" : "hide"),
      filteredRolesLength = filteredRoles.length;
    return (
      <CardBody>
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
        <div className="card__title">
          <Export 
            data={this.state.rows}
            heads={this.heads}
          />
          <div className="pageheader">
            <img src="https://d2tqbrn06t95pa.cloudfront.net/img/topnav_reports.png?v=2" /> Contractor Training Dashboard
            </div>
          <p className="card__description">This is the default level. Shows a list of all shared roles and the overall progress of the entire contractor fleet(all contractor companies).</p>
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
              <ReactDataGrid
                ref={'contractorDashboardDataGrid'}
                className={"contractor-training-dashboard"}
                onGridSort={this.handleGridSort}
                enableCellSelect={false}
                enableCellAutoFocus={false}
                columns={this.heads}
                rowGetter={this.rowGetter}
                rowsCount={rows.length}
                onGridRowsUpdated={this.handleGridRowsUpdated}
                rowHeight={35}
                minColumnWidth={100}
                emptyRowsView={this.state.isInitial && DataTableEmptyRowsView}
              />
            </div>
          </div>
        </div>
      </CardBody>
    );
  }
}

export default withCookies(CTDashboard);
