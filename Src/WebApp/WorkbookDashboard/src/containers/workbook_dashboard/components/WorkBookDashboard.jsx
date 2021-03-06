/* eslint-disable */
/*
* WorkBookDashboard.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details to list the workbooks 
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------
createRows(employees)
getEmployees(userId)
getMyEmployees(userId)
getAssignedWorkbooks(userId)
getPastDueWorkbooks(userId)
getComingDueWorkbooks(userId)
getCompletedWorkbooks(userId)
toggle()
handleGridRowsUpdated(fromRow, toRow, updated)
handleGridSort(sortColumn, sortDirection)
updateModalState(modelName)
handleCellFocus(args) 
*/
import React, { PureComponent } from 'react';
import { CardBody, Collapse, Row, Col, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import { WithContext as ReactTags } from 'react-tag-input';
import MyEmployees from './MyEmployees';
import AssignedWorkBook from './AssignedWorkBook';
import WorkBookDuePast from './WorkBookDuePast';
import WorkBookComingDue from './WorkBookComingDue';
import WorkBookCompleted from './WorkBookCompleted';
import * as API from '../../../shared/utils/APIUtils';
import * as Constants from '../../../shared/constants';
import FilterModal from './FilterModal';
import Export from './WorkBookDashboardExport';
import SessionPopup from './SessionPopup';
import _ from "lodash";

// Import React Table
import ReactTable from "react-table";
import "react-table/react-table.css";

/**
 * DataTableEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-table module.
 */

class WorkBookDashboard extends PureComponent {

  constructor() {
    super();
    this.heads = [
      {
        key: 'employee',
        name: 'Employee',
        sortable: true,
        width: 240,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.employeeFormatter,
        cellClass: "text-left"
      },
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
        key: 'assignedWorkBooks',
        name: 'Assigned Workbooks',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.workbookFormatter("assignedWorkBooks", props),
        cellClass: "text-right"
      },
      {
        key: 'inDueWorkBooks',
        name: 'Workbooks Due',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.workbookFormatter("inDueWorkBooks", props),
        cellClass: "text-right"
      },
      {
        key: 'pastDueWorkBooks',
        name: 'Past Due Workbooks',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.workbookFormatter("pastDueWorkBooks", props),
        cellClass: "text-right"
      },
      {
        key: 'completedWorkBooks',
        name: 'Completed Workbooks',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.workbookFormatter("completedWorkBooks", props),
        cellClass: "text-right"
      },
      {
        key: 'total',
        name: 'Total Employees',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.employeeFormatter,
        cellClass: "text-right last-column padding-rt-2p"
      },
    ];

    this.employees = [];
    this.roleFilter = React.createRef();
    this.state = {
      rows: this.createRows(this.employees),
      pageOfItems: [],
      isMyEmployeeModal: false,
      isAssignedModal: false,
      isPastDueModal: false,
      isComingDueModal: false,
      isCompletedModal: false,
      myEmployees: {},
      myEmployeesArray: {},
      assignedWorkBooks: {},
      workBookDuePast: {},
      workBookComingDue: {},
      workBookCompleted: {},
      fakeState: false,
      level: 0,
      supervisorNames: [],
      isInitial: false,
      collapse: false,
      collapseText: "More Options",
      isFilterModal: false,
      filterModalTitle: "Roles",
      filteredRoles: [],
      userId: 0,
      selectedUserId: 0,
      isReloadWindow: false,
      isSessionPopup: false,
      sessionPopupType: "API"
    };

    this.toggle = this.toggle.bind(this);
    this.toggleFilter = this.toggleFilter.bind(this);
    this.handleRoleDelete = this.handleRoleDelete.bind(this);
    this.customCell = this.customCell.bind(this);
    this.customCellTextTooltip = this.customCellTextTooltip.bind(this);
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
   * @name - employeeFormatter
   * This method will format the employee name column Data Grid
   * @param props
   * @returns none
   */
  employeeFormatter = (props) => {
    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let userId = contractorManagementDetails.User.Id || 0;
    if (props.original.userId == userId || props.original.total <= 0 || props.original.employee == "Total") {
      return (
        <span>{props.value}</span>
      );
    } else {
      return (
        <span onClick={e => {
          e.preventDefault();
          let isMyEmployeeModal = true,
            myEmployeesArray = [],
            supervisorNames = [];

          supervisorNames.push({ 'name': props.original.employee, 'column': "NONE", 'order': "NONE", 'userId': props.original.userId });
          this.setState({ isMyEmployeeModal, myEmployeesArray, supervisorNames });
          this.getMyEmployees(props.original.userId);
        }}
          className={"text-clickable"}>
          {props.value}
        </span>
      );
    }
  }

  /**
   * @method
   * @name - workbookFormatter
   * This method will catch all the exceptions in this class
   * @param error
   * @param info
   * @returns none
  */
  workbookFormatter = (type, props) => {
    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let userId = contractorManagementDetails.User.Id || 0;
    if (props.dependentValues.userId == userId || props.dependentValues[type] <= 0 || props.dependentValues.employee == "Total") {
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
   * @name - updateModalState
   * This method will update the modal window state of parent
   * @param modelName
   * @returns none
   */
  updateModalState = (modelName) => {
    let value = !this.state[modelName];
    if (modelName == "isMyEmployeeModal") {
      this.setState({
        [modelName]: value,
        myEmployeesArray: {}
      });
    } else {
      this.setState({
        [modelName]: value
      });
    }
  };

  /**
   * @method
   * @name - updateMyEmployeesArray
   * This method will update MyEmployees Array of state of this component
   * @param employees
   * @param supervisor
   * @returns none
   */
  updateMyEmployeesArray = (employees, supervisor) => {
    let myEmployeesArray = this.state.myEmployeesArray || [],
      level = this.state.level + 1,
      supervisorNames = this.state.supervisorNames,
      employeesLength = employees.length;

    if (employeesLength > 0)
      supervisorNames.push({ 'name': supervisor.employee, 'column': "NONE", 'order': "NONE", 'userId': supervisor.userId });

    if(Array.isArray(myEmployeesArray))
      myEmployeesArray.push(employees);

    this.setState({ ...this.state, myEmployeesArray, level, supervisorNames });
  };

  /**
   * @method
   * @name - popMyEmployeesArray
   * This method will delete last element of MyEmployees of state of this component
   * @param none
   * @returns none
   */
  popMyEmployeesArray = () => {
    let myEmployeesArray = this.state.myEmployeesArray,
      level = this.state.level - 1,
      supervisorNames = this.state.supervisorNames;

    if (myEmployeesArray.length > 0) {
        myEmployeesArray.pop();
    }
    if (supervisorNames.length > 0) {
        supervisorNames.pop();
    }
    this.setState({ ...this.state, myEmployeesArray, level, supervisorNames });
  };

  /**
   * @method
   * @name - componentDidMount
   * This method will invoked whenever the component is mounted
   *  is update to this component class
   * @param none
   * @returns none
  */
  async componentDidMount() {
    let { dashboardAPIToken } = sessionStorage,
      idToken = '';

    if (dashboardAPIToken) {
      dashboardAPIToken = JSON.parse(dashboardAPIToken);
      idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";
    }
    if (idToken) {
      this.setState({ isReloadWindow: false });
      let { contractorManagementDetails } = sessionStorage || '{}';
      contractorManagementDetails = JSON.parse(contractorManagementDetails);
      let companyId = contractorManagementDetails.Company.Id || 0;
      let userId = contractorManagementDetails.User.Id || 0,
        roles = [];

      await this.getFilterOptions();
      this.getEmployees(companyId, userId, roles);
    } else {
      this.setState({ isReloadWindow: true });
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
    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";
    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let companyId = contractorManagementDetails.Company.Id || 0;
    const postData = {
      "AppType": "WORKBOOK_DASHBOARD"
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
      Object.keys(response).map(function (i) { return response[i].id ? response[i].id = response[i].id.toString() : "" });
      this.setState({ filterOptionsRoles: response });
    }
  };

  /**
   * @method
   * @name - getEmployees
   * This method will used to get Employees details
   * @param userId
   * @returns none
   */
  async getEmployees(companyId, userId, roles) {
    this.setState({ userId: userId });
    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";
    let rolesLength = roles.length,
      fields = [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "CURRENT_USER", "Value": userId, "Operator": "=", "Bitwise": "AND" }];
    if (rolesLength > 0) {
      let roleIds = roles.join();
      let roleField = { "Name": "ROLES", "Value": roleIds, "Operator": "=", "Bitwise": "AND" };
      fields.push(roleField);
    }
    const postData = {
      "Fields": fields,
      "ColumnList": Constants.GET_EMPLOYEES_COLUMNS,
      "AppType": "WORKBOOK_DASHBOARD"
    };
    let token = idToken,
      url = "/company/" + companyId + "/workbooks",
      response = await API.ProcessAPI(url, postData, token, false, "POST", true),
      rows = this.createRows(response),
      isInitial = true;

    if (response == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if (response == 'API_ERROR') {
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {
      this.setState({ rows: rows, isInitial: isInitial });
      this.onChangePage([]);
    }
  };

  /**
   * @method
   * @name - getMyEmployees
   * This method will used to get My Employees details
   * @param userId
   * @returns none
   */
  async getMyEmployees(userId) {
    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";
    const postData = {
      "Fields": [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "CURRENT_USER", "Value": userId, "Operator": "=", "Bitwise": "AND" }],
      "ColumnList": Constants.GET_EMPLOYEES_COLUMNS,
      "AppType": "WORKBOOK_DASHBOARD"
    };

    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let companyId = contractorManagementDetails.Company.Id || 0;

    let token = idToken,
      url = "/company/" + companyId + "/workbooks",
      response = await API.ProcessAPI(url, postData, token, false, "POST", true),
      myEmployees = response,
      myEmployeesArray = [],
      isMyEmployeeModal = true,
      fakeState = this.state.fakeState,
      level = this.state.level + 1;

    if (response == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if (response == 'API_ERROR') {
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {
      myEmployeesArray.push(myEmployees);
      fakeState = !fakeState;
      this.setState({ ...this.state, isMyEmployeeModal, myEmployees, myEmployeesArray, fakeState, level });
    }
  };

  /**
   * @method
   * @name - getAssignedWorkbooks
   * This method will used to get Assigned Workbooks details
   * @param userId
   * @returns none
   */
  async getAssignedWorkbooks(userId) {
    const loggedInUserId = this.state.userId || 0;

    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "",
      fields = [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "USER_ID", "Value": userId, "Operator": "=", "Bitwise": "and" }, { "Name": "ASSIGNED_WORKBOOK", "Value": "true", "Operator": "=", "Bitwise": "and" }];
    if (loggedInUserId) {
      let currentUserField = { "Name": "CURRENT_USER", "Value": loggedInUserId, "Operator": "=", "Bitwise": "AND" };
      fields.push(currentUserField);
    }

    const payLoad = {
      "Fields": fields,
      "ColumnList": Constants.GET_ASSIGNED_WORKBOOKS_COLUMNS,
      "AppType": "WORKBOOK_DASHBOARD"
    };

    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let companyId = contractorManagementDetails.Company.Id || 0;

    let isAssignedModal = true,
      assignedWorkBooks = {};
    this.setState({ isAssignedModal, assignedWorkBooks });

    let token = idToken,
      url = "/company/" + companyId + "/workbooks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

    if (response == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if (response == 'API_ERROR') {
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {
      assignedWorkBooks = response;
      isAssignedModal = true;
      this.setState({ ...this.state, isAssignedModal, assignedWorkBooks });
    }
  };

  /**
  * @method
  * @name - getPastDueWorkbooks
  * This method will used to get Past Due Workbooks details
  * @param userId
  * @returns none
  */
  async getPastDueWorkbooks(userId) {
    const loggedInUserId = this.state.userId || 0;
    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "",
      fields = [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "USER_ID", "Value": userId, "Operator": "=", "Bitwise": "and" }, { "Name": "PAST_DUE", "Value": "30", "Operator": "=", "Bitwise": "and" }];
    if (loggedInUserId) {
      let currentUserField = { "Name": "CURRENT_USER", "Value": loggedInUserId, "Operator": "=", "Bitwise": "AND" };
      fields.push(currentUserField);
    }
    const payLoad = {
      "Fields": fields,
      "ColumnList": Constants.GET_WORKBOOKS_PAST_DUE_COLUMNS,
      "AppType": "WORKBOOK_DASHBOARD"
    };

    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let companyId = contractorManagementDetails.Company.Id || 0;

    let isPastDueModal = true,
      workBookDuePast = {};
    this.setState({ isPastDueModal, workBookDuePast });

    let token = idToken,
      url = "/company/" + companyId + "/workbooks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

    if (response == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if (response == 'API_ERROR') {
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {
      workBookDuePast = response;
      isPastDueModal = true;
      this.setState({ ...this.state, isPastDueModal, workBookDuePast });
    }
  };

  /**
   * @method
   * @name - getComingDueWorkbooks
   * This method will used to get Coming Due Workbooks details
   * @param userId
   * @returns none
   */
  async getComingDueWorkbooks(userId) {
    const loggedInUserId = this.state.userId || 0;
    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "",
      fields = [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "USER_ID", "Value": userId, "Operator": "=", "Bitwise": "and" }, { "Name": "WORKBOOK_IN_DUE", "Value": "30", "Operator": "=", "Bitwise": "and" }];
    if (loggedInUserId) {
      let currentUserField = { "Name": "CURRENT_USER", "Value": loggedInUserId, "Operator": "=", "Bitwise": "AND" };
      fields.push(currentUserField);
    }
    const payLoad = {
      "Fields": fields,
      "ColumnList": Constants.GET_WORKBOOKS_COMING_DUE_COLUMNS,
      "AppType": "WORKBOOK_DASHBOARD"
    };

    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let companyId = contractorManagementDetails.Company.Id || 0;

    let isComingDueModal = true,
      workBookComingDue = {};
    this.setState({ isComingDueModal, workBookComingDue });

    let token = idToken,
      url = "/company/" + companyId + "/workbooks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

    if (response == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if (response == 'API_ERROR') {
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {
      workBookComingDue = response;
      isComingDueModal = true;
      this.setState({ ...this.state, isComingDueModal, workBookComingDue });
    }
  };

  /**
   * @method
   * @name - getCompletedWorkbooks
   * This method will used to get Completed Workbooks details
   * @param userId
   * @returns none
   */
  async getCompletedWorkbooks(userId) {
    const loggedInUserId = this.state.userId || 0;
    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "",
      fields = [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "USER_ID", "Value": userId, "Operator": "=", "Bitwise": "and" }, { "Name": "COMPLETED", "Value": "true", "Operator": "=", "Bitwise": "and" }];
    if (loggedInUserId) {
      let currentUserField = { "Name": "CURRENT_USER", "Value": loggedInUserId, "Operator": "=", "Bitwise": "AND" };
      fields.push(currentUserField);
    }
    const payLoad = {
      "Fields": fields,
      "ColumnList": Constants.GET_COMPLETED_WORKBOOKS_COLUMNS,
      "AppType": "WORKBOOK_DASHBOARD"
    };

    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let companyId = contractorManagementDetails.Company.Id || 0;

    let isCompletedModal = true,
      workBookCompleted = {};
    this.setState({ isCompletedModal, workBookCompleted });

    let token = idToken,
      url = "/company/" + companyId + "/workbooks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

    if (response == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if (response == 'API_ERROR') {
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {
      workBookCompleted = response;
      isCompletedModal = true;
      this.setState({ ...this.state, isCompletedModal, workBookCompleted });
    }
  };

  /**
   * @method
   * @name - onChangePage
   * This method will update data grid rows whenever page is changed
   * @param pageOfItems
   * @returns none
   */
  onChangePage = (pageOfItems) => {
    this.setState({ pageOfItems });
  };

  /**
  * @method
  * @name - createRows
  * This method will format the input data
  * for Data Grid
  * @param employees
  * @returns rows
  */
  createRows = (employees) => {
    var assignedWorkBooksCount = 0;
    var inDueWorkBooksCount = 0;
    var pastDueWorkBooksCount = 0;
    var completedWorkBooksCount = 0;
    var totalEmpCount = 0;
    let rows = [],
      length = employees ? employees.length : 0;
    for (let i = 0; i < length; i++) {
      assignedWorkBooksCount += parseInt(employees[i].AssignedWorkBook || 0);
      inDueWorkBooksCount += parseInt(employees[i].InDueWorkBook || 0);
      pastDueWorkBooksCount += parseInt(employees[i].PastDueWorkBook || 0);
      completedWorkBooksCount += parseInt(employees[i].CompletedWorkbook || 0);
      totalEmpCount += parseInt(employees[i].TotalEmployees || 0);
      rows.push({
        userId: employees[i].UserId || 0,
        employee: employees[i].EmployeeName + " (" + employees[i].UserName + " | " + employees[i].UserId + ")",
        role: employees[i].Role,
        assignedWorkBooks: parseInt(employees[i].AssignedWorkBook || 0),
        inDueWorkBooks: parseInt(employees[i].InDueWorkBook || 0),
        pastDueWorkBooks: parseInt(employees[i].PastDueWorkBook || 0),
        completedWorkBooks: parseInt(employees[i].CompletedWorkbook || 0),
        total: parseInt(employees[i].TotalEmployees) || 0
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

    const beforePopRows = this.state.rows,
      rowsLength = this.state.rows.length || 0;
    let totalRow = "";
    if (beforePopRows.length > 0) {
      totalRow = beforePopRows.pop();
    }

    const sortRows = beforePopRows.slice(0);
    const rows = sortDirection === 'NONE' ? this.state.rows.slice(0, rowsLength) : sortRows.sort(comparer).slice(0, rowsLength);

    if (beforePopRows.length > 0)
      rows.push(totalRow);

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
    let userId = args.userId || 0;
    this.setState({ selectedUserId: userId });
    switch (type) {
      case "completedWorkBooks":
        if (userId) {
          this.getCompletedWorkbooks(userId);
        }
        break;
      case "assignedWorkBooks":
        if (userId) {
          this.getAssignedWorkbooks(userId);
        }
        break;
      case "pastDueWorkBooks":
        if (userId) {
          this.getPastDueWorkbooks(userId);
        }
        break;
      case "inDueWorkBooks":
        if (userId) {
          this.getComingDueWorkbooks(userId);
        }
        break;
      default:
        break;
    }
    // this.refs.reactDataGrid.deselect();
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
    if(this.roleFilter.current){
      this.roleFilter.current.selectMultipleOption(true, this, filteredRoles);
    }
  };

  /**
    * @method
    * @name - filterGoAction
    * This method will update the selected role on state
    * @param none
    * @returns none
  */
  filterGoAction = () => {
    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let companyId = contractorManagementDetails.Company.Id || 0,
      userId = contractorManagementDetails.User.Id || 0;
    let filteredRoles = this.state.filteredRoles,
      roles = [];
    Object.keys(filteredRoles).map(function (i) { return roles.push(filteredRoles[i].id) });
    this.getEmployees(companyId, userId, roles);
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
    window.location = window.location.origin + "/Logout.aspx"; //Need to be window.location.origin after integrating with LMS Site
  };

  componentDidUpdate(prevProps, prevState) {
    // One possible fix...
    // var tooltipDivs = document.getElementsByClassName('truncate');
    // tooltipDivs.forEach(function (node) {
    //   var thisTxt = node.textContent || "";
    //   var cloneEle = document.createElement("div");
    //   cloneEle.className += " ele-clone";
    //   cloneEle.textContent = thisTxt;
    //   document.body.appendChild(cloneEle);
    //   if (node.offsetWidth <= (cloneEle.offsetWidth) / 2) {
    //     var att = document.createAttribute("title");
    //     att.value = thisTxt;
    //     node.setAttributeNode(att);
    //   } else {
    //     var att = document.createAttribute("title");
    //     att.value = "";
    //     node.setAttributeNode(att);
    //   }
    //   cloneEle.remove();
    // });
  }

  customCellTextTooltip(props) {
    return (
      <span className={"tooltip-hover truncate"}>
        {props.value}
      </span>
    );
  }

  render() {
    const { rows, collapseText, collapse, filteredRoles, supervisorNames } = this.state;
    let collapseClassName = (collapse ? "show" : "hide"),
      filteredRolesLength = filteredRoles.length;
    let supervisorNamesLength = supervisorNames.length > 0 ? supervisorNames.length - 1 : supervisorNames.length,
      currentUserId = supervisorNames[supervisorNamesLength] ? supervisorNames[supervisorNamesLength].userId : 0;
    let basePath = window.location.origin || "";
    let pgSize = rows.length;
    return (
      <CardBody>
        <Modal backdrop={"static"} isOpen={this.state.isReloadWindow} toggle={this.toggle} fade={false} centered={true} className="auto-logout-modal">
          <ModalHeader> Alert</ModalHeader>
          <ModalBody>Your session has expired. Please login again</ModalBody>
          <ModalFooter>
            <button color="primary" onClick={this.autoLogout}>Go to Login</button>{' '}
          </ModalFooter>
        </Modal>
        <SessionPopup
          backdropClassName={"backdrop"}
          modal={this.state.isSessionPopup}
          sessionPopupType={this.state.sessionPopupType}
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
        <MyEmployees
          selectedUserId={currentUserId}
          backdropClassName={"backdrop"}
          fakeState={this.state.fakeState}
          level={this.state.level}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isMyEmployeeModal}
          updateMyEmployeesArray={this.updateMyEmployeesArray.bind(this)}
          popMyEmployeesArray={this.popMyEmployeesArray.bind(this)}
          myEmployees={this.state.myEmployeesArray}
          supervisorNames={this.state.supervisorNames}
        />
        <AssignedWorkBook
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isAssignedModal}
          assignedWorkBooks={this.state.assignedWorkBooks}
        />
        <WorkBookDuePast
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isPastDueModal}
          assignedWorkBooks={this.state.workBookDuePast}
        />
        <WorkBookComingDue
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isComingDueModal}
          assignedWorkBooks={this.state.workBookComingDue}
        />
        <WorkBookCompleted
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isCompletedModal}
          assignedWorkBooks={this.state.workBookCompleted}
        />
        <div className="card__title">
          <div className="breadcrumbs noprint"><a href={basePath + "/default.aspx"}>Home</a>&gt;<a href={basePath + "/ManageLanding.aspx"}>Manage</a>&gt;<a href={basePath + "/WorkbooksLanding.aspx"}>Training</a></div>          
          <Export
            data={this.state.rows}
            heads={this.heads}
            sheetName={"Workbook Dashboard"}
          />
          <div className="pageheader">
            <img src="https://d2tqbrn06t95pa.cloudfront.net/img/topnav_reports.png?v=2" /> Workbook Dashboard
            </div>
          <p className="card__description">Displays an overview of operator qualification records grouped by subordinate. This report displays current OQ's for the employee's task profile (if an OQ is not in the employee task profile it is not counted on this report). Column counts show sum of the supervisor and subordinates' records</p>
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
              <div className="section-info-pageheader">Employee Dashboard</div>
              <p className="section-info-description">
                View list of employees and the progress of their direct and indirect subordinates. Shows the workbooks assigned, completed and due.<br />
              </p>
            </div>
            <div className="table has-section-view has-total-row is-table-page-view">
              {/* <ReactDataGrid
                ref={'reactDataGrid'}
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
              /> */}
              <ReactTable
                minRows={1}
                data={rows}
                columns={[
                  {
                    Header: "Employee",
                    accessor: "employee",
                    headerClassName: 'header-wordwrap',
                    minWidth: 200,
                    className: 'text-left',
                    Cell: this.employeeFormatter,
                    Footer: (
                      <span>
                        <strong>Total</strong>
                      </span>
                    )
                  },
                  {
                    Header: "Role",
                    accessor: "role",
                    minWidth: 180,
                    className: 'text-left role-cell',
                    // Cell: this.customCellTextTooltip
                  },
                  {
                    Header: "Assigned Workbooks",
                    id: "assignedWorkBooks",
                    accessor: d => d.assignedWorkBooks,
                    headerClassName: 'header-wordwrap',
                    minWidth: 100,
                    className: 'text-center',
                    Cell: this.customCell,
                    Footer: (
                      <span>
                        <strong>
                          {
                            _.sumBy(_.values(rows), 'assignedWorkBooks')
                          }
                        </strong>
                      </span>
                    )
                  },
                  {
                    Header: "Workbooks Due",
                    accessor: "inDueWorkBooks",
                    headerClassName: 'header-wordwrap',
                    minWidth: 100,
                    className: 'text-center',
                    Cell: this.customCell,
                    Footer: (
                      <span>
                        <strong>
                          {
                            _.sumBy(_.values(rows), 'inDueWorkBooks')
                          }
                        </strong>
                      </span>
                    )
                  },
                  {
                    Header: "Past Due Workbooks",
                    accessor: "pastDueWorkBooks",
                    headerClassName: 'header-wordwrap',
                    minWidth: 100,
                    className: 'text-center',
                    Cell: this.customCell,
                    Footer: (
                      <span>
                        <strong>
                          {
                            _.sumBy(_.values(rows), 'pastDueWorkBooks')
                          }
                        </strong>
                      </span>
                    )
                  },
                  {
                    Header: "Completed Workbooks",
                    accessor: "completedWorkBooks",
                    headerClassName: 'header-wordwrap',
                    minWidth: 100,
                    className: 'text-center',
                    Cell: this.customCell,
                    Footer: (
                      <span>
                        <strong>
                          {
                            _.sumBy(_.values(rows), 'completedWorkBooks')
                          }
                        </strong>
                      </span>
                    )
                  },
                  {
                    Header: "Total Employees",
                    accessor: "total",
                    headerClassName: 'header-wordwrap',
                    minWidth: 100,
                    className: 'text-center',
                    Cell: this.employeeFormatter,
                    Footer: (
                      <span>
                        <strong>
                          {
                            _.sumBy(_.values(rows), 'total')
                          }
                        </strong>
                      </span>
                    )
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
                    id: "employee",
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

export default WorkBookDashboard;
