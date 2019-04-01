/* eslint-disable */
/*
* OQDashboard.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details to list the workbooks 
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------
createRows(employees)
handleGridRowsUpdated(fromRow, toRow, updated)
handleGridSort(sortColumn, sortDirection)
*/
import React, { PureComponent } from 'react';
import { CardBody, Collapse, Row, Col } from 'reactstrap';
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import { WithContext as ReactTags } from 'react-tag-input';
import EmployeeView from '../components/EmployeeView';
import AssignedQualification from '../components/AssignedQualification';
import CompletedQualification from '../components/CompletedQualification';
import InCompletedQualification from '../components/InCompletedQualification';
import PastDueQualification from '../components/PastDueQualification';
import ComingDueQualification from '../components/ComingDueQualification';
import * as API from '../../../shared/utils/APIUtils';
import * as Constants from '../../../shared/constants';
import FilterModal from './FilterModal';
import Export from './OQDashboardExport';

/**
 * OQDashboardEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-data-grid module.
 */
class OQDashboardEmptyRowsView extends React.Component {
  render() {
    return (<div className="no-records-found-modal">Sorry, no records</div>)
  }
};

class OQDashboard extends PureComponent {

  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor() {
    super();
    this.heads = [
      {
        key: 'company',
        name: 'Company',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.qualificationsFormatter("company", props),
        cellClass: "text-left"
      },
      {
        key: 'assignedQualification',
        name: 'Assigned Qualifications',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.qualificationsFormatter("assignedQualification", props),
        cellClass: "text-right"
      },
      {
        key: 'completedQualification',
        name: 'Qualifications',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.qualificationsFormatter("completedQualification", props),
        cellClass: "text-right"
      },
      {
        key: 'inCompletedQualification',
        name: 'Disqualifications',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.qualificationsFormatter("inCompletedQualification", props),
        cellClass: "text-right"
      },
      {
        key: 'pastDue',
        name: 'Expired Qualifications (30 Days)',
        width: 200,
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.qualificationsFormatter("pastDue", props),
        cellClass: "text-right"
      },
      {
        key: 'comingDue',
        name: 'Expires in 30 Days',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.qualificationsFormatter("comingDue", props),
        cellClass: "text-right"
      },
      {
        key: 'total',
        name: 'Total Employees',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.qualificationsFormatter("total", props),
        cellClass: "text-right last-column padding-rt-2p"
      },
    ];

    this.tasks = [];
    this.roleFilter = React.createRef();
    this.state = {
      rows: this.createRows(this.tasks),
      level: 0,
      isEmployeeView: false,
      isAssignedQualificationView: false,
      isCompletedQualificationView: false,
      isInCompletedQualificationView: false,
      isPastDueQualificationView: false,
      isComingDueQualificationView: false,
      employeeQualifications: {},
      employeesQualificationsArray: {},
      assignedQualifications: {},
      completedQualifications: {},
      inCompletedQualifications: {},
      pastDueQualifications: {},
      comingDueQualifications: {},
      contractorsNames: [],
      collapse: false,
      collapseText: "More Options",
      isFilterModal: false,
      filterModalTitle: "Roles",
      filteredRoles: [],
      filterOptionsRoles: []
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
  }

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
    // Do API call for loading initial table view
    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    // let userId = contractorManagementDetails.User.Id || 0;
    const { cookies } = this.props;
    let userId = contractorManagementDetails.User.Id || 0,// cookies.get('UserId'),
      roles = [];
    await this.getFilterOptions();
    this.getQualifications(userId, roles);
  };

  /**
  * @method
  * @name - createRows
  * This method will format the input data
  * for Data Grid
  * @param qualifications
  * @returns rows
  */
  createRows = (qualifications) => {
    let assignedQualificationCount = 0,
      completedQualificationCount = 0,
      inCompletedQualificationCount = 0,
      pastDueCount = 0,
      comingDueCount = 0,
      totalQualificationCount = 0;
    const rows = [],
      length = qualifications ? qualifications.length : 0;
    for (let i = 0; i < length; i++) {
      assignedQualificationCount += parseInt(qualifications[i].AssignedQualification);
      completedQualificationCount += parseInt(qualifications[i].CompletedQualification);
      inCompletedQualificationCount += parseInt(qualifications[i].IncompleteQualification);
      pastDueCount += parseInt(qualifications[i].PastDueQualification);
      comingDueCount += parseInt(qualifications[i].InDueQualification);
      totalQualificationCount += parseInt(qualifications[i].TotalEmployees || 0);
      rows.push({
        userId: qualifications[i].UserId,
        contractors: qualifications[i].EmployeeName,
        company: qualifications[i].CompanyName,
        companyId: qualifications[i].CompanyId,
        role: qualifications[i].Role || "",
        assignedQualification: qualifications[i].AssignedQualification,
        completedQualification: qualifications[i].CompletedQualification,
        inCompletedQualification: qualifications[i].IncompleteQualification,
        pastDue: qualifications[i].PastDueQualification,
        comingDue: qualifications[i].InDueQualification,
        total: qualifications[i].TotalEmployees,
      });
    }

    if (length > 0) {
      rows.push({ company: "Total", role: "", assignedQualification: assignedQualificationCount, completedQualification: completedQualificationCount, inCompletedQualification: inCompletedQualificationCount, pastDue: pastDueCount, comingDue: comingDueCount, total: totalQualificationCount });
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

    const beforePopRows = this.state.rows;
    let totalRow = "";
    if (beforePopRows.length > 0) {
      totalRow = beforePopRows.pop();
    }

    const sortRows = beforePopRows.slice(0),
      rowsLength = this.state.rows.length || 0;
    const rows = sortDirection === 'NONE' ? this.state.rows.slice(0, rowsLength) : sortRows.sort(comparer).slice(0, rowsLength);

    if (beforePopRows.length > 0)
      rows.push(totalRow);

    this.setState({ rows });
  };

  // This method is used to setting the row data in react data grid
  rowGetter = i => this.state.rows[i];

  /**
   * @method
   * @name - qualificationsFormatter
   * This method will format the qualification Data Grid
   * @param type
   * @param props
   * @returns none
   */
  qualificationsFormatter = (type, props) => {
    if (props.dependentValues[type] <= 0 || props.dependentValues.company == "Total") {
      return (
        <span>{props.value}</span>
      );
    } else {
      return (
        <span onClick={e => {
          e.preventDefault(); this.handleCellClick(type, props.dependentValues);
        }} className={"text-clickable"}>
          {props.value}
        </span>
      );
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
      [modelName]: value
    });
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
    let userId = args.userId || 0,
      companyId = args.companyId || 0;
    switch (type) {
      case "company":
      case "total":
        let isEmployeeView = this.state.isEmployeeView,
          employeeQualifications = {},
          employeesQualificationsArray = [],
          contractorsNames = this.state.contractorsNames;
        isEmployeeView = true;
        contractorsNames = [];
        contractorsNames.push({ 'name': args.company, 'column': "NONE", 'order': "NONE" });
        this.setState({ isEmployeeView, employeeQualifications, employeesQualificationsArray, contractorsNames });
        this.getEmployeeQualifications(userId, companyId);
        break;
      case "assignedQualification":
        this.getAssignedQualifications(userId, companyId);
        break;
      case "completedQualification":
        this.getCompletedQualifications(userId, companyId);
        break;
      case "inCompletedQualification":
        this.getInCompletedQualifications(userId, companyId);
        break;
      case "pastDue":
        this.getPastDueQualifications(userId, companyId);
        break;
      case "comingDue":
        this.getComingDueQualifications(userId, companyId);
        break;
      default:
        console.log("default", type, args);
        break;
    }
    this.refs.oQDashboardReactDataGrid.deselect();
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

    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let companyId = contractorManagementDetails.Company.Id || 0;

    let token = idToken,//cookies.get('IdentityToken'),
      //companyId = cookies.get('CompanyId'),
      url = "/company/" + companyId + "/roles",
      response = await API.ProcessAPI(url, "", token, false, "GET", true);
    response = JSON.parse(JSON.stringify(response).split('"Role":').join('"text":'));
    response = JSON.parse(JSON.stringify(response).split('"RoleId":').join('"id":'));
    Object.keys(response).map(function (i) { response[i].id ? response[i].id = response[i].id.toString() : "" });
    this.setState({ filterOptionsRoles: response });
  };

  /**
   * @method
   * @name - getQualifications
   * This method will used to get Qualifications details
   * @param userId
   * @returns none
   */
  async getQualifications(userId, roles) {
    const { cookies } = this.props;
    let rolesLength = roles.length,
      fields = [{ "Name": "USER_ID", "Value": userId, "Operator": "=" }];
    if (rolesLength > 0) {
      let roleIds = roles.join();
      let roleField = { "Name": "ROLES", "Value": roleIds, "Operator": "=", "Bitwise": "AND" };
      fields.push(roleField);
    }
    const payLoad = {
      "Fields": fields,
      "ColumnList": Constants.GET_COMPANY_QUALIFICATION_COLUMNS

    };

    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let companyId = contractorManagementDetails.Company.Id || 0;

    let token = idToken,// cookies.get('IdentityToken'),
      // companyId = cookies.get('CompanyId'),
      url = "/company/" + companyId + "/tasks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true),
      rows = this.createRows(response),
      isInitial = true;

    this.setState({ rows: rows, isInitial: isInitial });
  };

  /**
   * @method
   * @name - getAssignedQualifications
   * This method will used to get Assigned Qualifications
   * @param userId
   * @returns none
   */
  async getAssignedQualifications(userId, companyId) {
    const { cookies } = this.props;
    const payLoad = {
      "Fields": [
        // { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }
      ],
      "ColumnList": Constants.GET_ASSIGNED_QUALIFICATION_COLUMNS
    };

    let isAssignedQualificationView = this.state.isAssignedQualificationView,
      assignedQualifications = {};
    isAssignedQualificationView = true;
    this.setState({ isAssignedQualificationView, assignedQualifications });

    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

    let token = idToken,// cookies.get('IdentityToken'),
      url = "/company/" + companyId + "/tasks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);
    assignedQualifications = response;
    isAssignedQualificationView = true;
    this.setState({ ...this.state, isAssignedQualificationView, assignedQualifications });

  };

  /**
  * @method
  * @name - getCompletedQualifications
  * This method will used to get Completed Qualifications
  * @param userId
  * @returns none
  */
  async getCompletedQualifications(userId, companyId) {
    const { cookies } = this.props;
    const payLoad = {
      "Fields": [
        // { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" },
        { "Name": "COMPLETED", "Value": "true", "Operator": "=" }
      ],
      "ColumnList": Constants.GET_COMPLETED_QUALIFICATION_COLUMNS
    };

    let isCompletedQualificationView = this.state.isCompletedQualificationView,
      completedQualifications = {};
    isCompletedQualificationView = true;
    this.setState({ isCompletedQualificationView, completedQualifications });

    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

    let token = idToken,// cookies.get('IdentityToken'),
      url = "/company/" + companyId + "/tasks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);
    completedQualifications = response;
    isCompletedQualificationView = true;
    this.setState({ ...this.state, isCompletedQualificationView, completedQualifications });
  };

  /**
  * @method
  * @name - getInCompletedQualifications
  * This method will used to get InCompleted Qualifications
  * @param userId
  * @returns none
  */
  async getInCompletedQualifications(userId, companyId) {
    const { cookies } = this.props;
    const payLoad = {
      "Fields": [
        // { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" },
        { "Name": "IN_COMPLETE", "Value": "true", "Operator": "=" }
      ],
      "ColumnList": Constants.GET_IN_COMPLETED_QUALIFICATION_COLUMNS
    };

    let isInCompletedQualificationView = this.state.isInCompletedQualificationView,
      inCompletedQualifications = {};
    isInCompletedQualificationView = true;
    this.setState({ isInCompletedQualificationView, inCompletedQualifications });

    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

    let token = idToken,// cookies.get('IdentityToken'),
      url = "/company/" + companyId + "/tasks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);
    inCompletedQualifications = response;
    isInCompletedQualificationView = true;
    this.setState({ ...this.state, isInCompletedQualificationView, inCompletedQualifications });
  };

  /**
  * @method
  * @name - getPastDueQualifications
  * This method will used to get Past Due Qualifications
  * @param userId
  * @returns none
  */
  async getPastDueQualifications(userId, companyId) {
    const { cookies } = this.props;
    const payLoad = {
      "Fields": [
        // { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" },
        { "Name": "PAST_DUE", "Value": "30", "Operator": "=" }
      ],
      "ColumnList": Constants.GET_PAST_DUE_QUALIFICATION_COLUMNS
    };

    let isPastDueQualificationView = this.state.isPastDueQualificationView,
      pastDueQualifications = {};
    isPastDueQualificationView = true;
    this.setState({ isPastDueQualificationView, pastDueQualifications });

    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

    let token = idToken,// cookies.get('IdentityToken'),
      url = "/company/" + companyId + "/tasks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);
    pastDueQualifications = response;
    isPastDueQualificationView = true;
    this.setState({ ...this.state, isPastDueQualificationView, pastDueQualifications });
  };

  /**
  * @method
  * @name - getComingDueQualifications
  * This method will used to get Coming due qualifications
  * @param userId
  * @returns none
  */
  async getComingDueQualifications(userId, companyId) {
    const { cookies } = this.props;
    const payLoad = {
      "Fields": [
        // { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" },
        { "Name": "IN_DUE", "Value": "30", "Operator": "=" }
      ],
      "ColumnList": Constants.GET_COMING_DUE_QUALIFICATION_COLUMNS
    };

    let isComingDueQualificationView = this.state.isComingDueQualificationView,
      comingDueQualifications = {};
    isComingDueQualificationView = true;
    this.setState({ isComingDueQualificationView, comingDueQualifications });

    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

    let token = idToken,// cookies.get('IdentityToken'),
      url = "/company/" + companyId + "/tasks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

    comingDueQualifications = response;
    isComingDueQualificationView = true;
    this.setState({ ...this.state, isComingDueQualificationView, comingDueQualifications });
  };

  /**
     * @method
     * @name - getEmployeeQualifications
     * This method will used to get Employee Qualifications
     * @param userId
     * @returns none
    */
  async getEmployeeQualifications(userId, companyId) {
    const { cookies } = this.props;
    const payLoad = {
      "Fields": [
        { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }
      ],
      "ColumnList": Constants.GET_EMPLOYEE_QUALIFICATION_COLUMNS
    };

    let isEmployeeView = this.state.isEmployeeView,
      employeeQualifications = {},
      employeesQualificationsArray = [];

    isEmployeeView = true;
    this.setState({ isEmployeeView, employeeQualifications, employeesQualificationsArray });

    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

    let token = idToken,// cookies.get('IdentityToken'),
      url = "/company/" + companyId + "/tasks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

    employeeQualifications = response;
    employeesQualificationsArray.push(employeeQualifications);
    isEmployeeView = true;
    this.setState({ ...this.state, isEmployeeView, employeeQualifications, employeesQualificationsArray });
  };

  /**
     * @method
     * @name - updateEmployeesQualificationsArray
     * This method will update Array of state of this component
     * @param qualifications
     * @returns none
  */
  updateEmployeesQualificationsArray = (qualifications, contractors) => {
    let employeesQualificationsArray = this.state.employeesQualificationsArray,
      level = this.state.level + 1,
      contractorsNames = this.state.contractorsNames,
      contractorsLength = contractorsNames.length;

    if (contractorsLength > 0)
      contractorsNames.push({ 'name': contractors.employee, 'column': "NONE", 'order': "NONE" });

    employeesQualificationsArray.push(qualifications);
    this.setState({ ...this.state, level, employeesQualificationsArray, contractorsNames });
  };

  /**
   * @method
   * @name - popEmployeesQualificationsArray
   * This method will delete last element of state of this component
   * @param none
   * @returns none
  */
  popEmployeesQualificationsArray = () => {
    let employeesQualificationsArray = this.state.employeesQualificationsArray,
      level = this.state.level - 1,
      contractorsNames = this.state.contractorsNames;

    if (employeesQualificationsArray.length > 0) {
      let totalRow = employeesQualificationsArray.pop();
    }
    if (contractorsNames.length > 0) {
      let totalRow = contractorsNames.pop();
    }
    this.setState({ ...this.state, level, employeesQualificationsArray, contractorsNames });
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
    this.roleFilter.current.selectMultipleOption(true, filteredRoles);
  }

  /**
  * @method
  * @name - filterGoAction
  * This method will update the selected role on state
  * @param none
  * @returns none
 */
  filterGoAction = () => {
    const { cookies } = this.props;
    // let userId = cookies.get('UserId');
    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let userId = contractorManagementDetails.User.Id || 0;
    let filteredRoles = this.state.filteredRoles,
      roles = [];
    Object.keys(filteredRoles).map(function (i) { roles.push(filteredRoles[i].id) });
    this.getQualifications(userId, roles);
  };

  render() {
    const { rows, collapseText, collapse, filteredRoles } = this.state;
    let collapseClassName = (collapse ? "show" : "hide"),
      filteredRolesLength = filteredRoles.length;
    return (
      <CardBody>
        <FilterModal
          ref={this.roleFilter}
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          updateSelectedData={this.updateSelectedData.bind(this)}
          modal={this.state.isFilterModal}
          title={this.state.filterModalTitle}
          filterOptionsRoles={this.state.filterOptionsRoles}
        />
        <EmployeeView
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isEmployeeView}
          updateEmployeesQualificationsArray={this.updateEmployeesQualificationsArray.bind(this)}
          popEmployeesQualificationsArray={this.popEmployeesQualificationsArray.bind(this)}
          employeesQualificationsArray={this.state.employeesQualificationsArray}
          employeeQualifications={this.state.employeeQualifications}
          contractorsNames={this.state.contractorsNames}
        />
        <AssignedQualification
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isAssignedQualificationView}
          assignedQualifications={this.state.assignedQualifications}
        />
        <CompletedQualification
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isCompletedQualificationView}
          completedQualifications={this.state.completedQualifications}
        />
        <InCompletedQualification
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isInCompletedQualificationView}
          inCompletedQualifications={this.state.inCompletedQualifications}
        />
        <PastDueQualification
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isPastDueQualificationView}
          pastDueQualifications={this.state.pastDueQualifications}
        />
        <ComingDueQualification
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isComingDueQualificationView}
          comingDueQualifications={this.state.comingDueQualifications}
        />
        <div className="card__title">
          <Export
            data={this.state.rows}
            heads={this.heads}
            sheetName={"Contractor OQ Dashboard"}
          />
          <div className="pageheader">
            <img src="https://d2tqbrn06t95pa.cloudfront.net/img/topnav_reports.png?v=2" /> Contractor OQ Dashboard
            </div>
          <p className="card__description">Displays an overview of contractor qualification records grouped by subordinate. This report displays current OQ's for the contractor employee's task profile (if an OQ is not in the employee task profile it is not counted on this report). Column counts show sum of the supervisor and subordinates' records</p>
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
              <div className="section-info-pageheader">Operator Qualifications</div>
              <p className="section-info-description">
                Shows the list of companies and the progress of their qualifications<br/>
                Qualifications show the count of completed qualifications<br/>
                Disqualifications show the incomplete qualifications while Suspended shows the qualifications suspended by the admin/ evaluator<br/>
              </p>
            </div>
            <div className="table has-section-view has-total-row">
              <ReactDataGrid
                ref={'oQDashboardReactDataGrid'}
                onGridSort={this.handleGridSort}
                enableCellSelect={false}
                enableCellAutoFocus={false}
                columns={this.heads}
                rowGetter={this.rowGetter}
                rowsCount={rows.length}
                onGridRowsUpdated={this.handleGridRowsUpdated}
                rowHeight={35}
                minColumnWidth={100}
                emptyRowsView={this.state.isInitial && OQDashboardEmptyRowsView}
              />
            </div>
          </div>
        </div>
      </CardBody>
    );
  }
}

export default withCookies(OQDashboard);
