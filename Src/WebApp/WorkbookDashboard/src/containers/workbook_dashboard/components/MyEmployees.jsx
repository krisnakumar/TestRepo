/* eslint-disable */
/*
* MyEmployees.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render My employees(supervisor view) to list the employees 
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------
createRows(employees)
getPastDueWorkbooks(userId)
getComingDueWorkbooks(userId)
getCompletedWorkbooks(userId)
toggle()
handleGridRowsUpdated(fromRow, toRow, updated)
handleGridSort(sortColumn, sortDirection)
updateModalState(modelName)
handleCellFocus(args) 
*/

import React from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';
import WorkBookDuePast from './WorkBookDuePast';
import WorkBookComingDue from './WorkBookComingDue';
import WorkBookCompleted from './WorkBookCompleted';
import AssignedWorkBook from './AssignedWorkBook';
import * as API from '../../../shared/utils/APIUtils';
import * as Constants from '../../../shared/constants';
import SessionPopup from './SessionPopup';
import Export from './WorkBookDashboardExport';
import _ from "lodash";

// Import React Table
import ReactTable from "react-table";
import "react-table/react-table.css";

/**
 * EmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-data-grid module.
 */

class MyEmployees extends React.Component {

  constructor(props) {
    super(props);

    this.heads = [
      {
        key: 'employee',
        name: 'Employee',
        sortable: true,
        width: 180,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.employeeFormatter,
        cellClass: "text-left"
      },
      {
        key: 'role',
        name: 'Role',
        width: 150,
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
        width: 200,
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
        cellClass: "text-right last-column"
      },
    ];

    this.employees = [];

    this.state = {
      modal: this.props.modal,
      rows: this.createRows(this.props.myEmployees || []),
      pageOfItems: [],
      isMyEmployeeModal: false,
      myEmployees: {},
      myEmployeesArray: this.props.myEmployees || [],
      level: this.props.level,
      isPastDueModal: false,
      isComingDueModal: false,
      isCompletedModal: false,
      workBookDuePast: {},
      workBookComingDue: {},
      workBookCompleted: {},
      supervisorNames: this.props.supervisorNames || [],
      isAssignedModal: false,
      assignedWorkBooks: {},
      isInitial: false,
      sortColumn: "",
      sortDirection: "NONE",
      selectedUserId: this.props.selectedUserId || 0,
      isSessionPopup: false,
      sessionPopupType: "API"
    };
    this.toggle = this.toggle.bind(this);
    this.updateModalState = this.updateModalState.bind(this);
    this.customCell = this.customCell.bind(this);
    this.customCellTextTooltip = this.customCellTextTooltip.bind(this);
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
   * @name - createRows
   * This method will format the input data
   * for Data Grid
   * @param workbooks
   * @returns rows
   */
  createRows = (employeesArray) => {
    let employeesArrayLength = employeesArray.length - 1;
    let employees = employeesArray[employeesArrayLength];
    let assignedWorkBooksCount = 0,
      inDueWorkBooksCount = 0,
      pastDueWorkBooksCount = 0,
      completedWorkBooksCount = 0,
      totalEmpCount = 0;
    const rows = [],
      length = employees ? employees.length : 0;
    for (let i = 0; i < length; i++) {
      assignedWorkBooksCount += parseInt(employees[i].AssignedWorkBook || 0);
      inDueWorkBooksCount += parseInt(employees[i].InDueWorkBook || 0);
      pastDueWorkBooksCount += parseInt(employees[i].PastDueWorkBook || 0);
      completedWorkBooksCount += parseInt(employees[i].CompletedWorkbook || 0);
      totalEmpCount += parseInt(employees[i].TotalEmployees || 0);
      rows.push({
        userId: employees[i].UserId || 0,
        role: employees[i].Role,
        assignedWorkBooks: employees[i].AssignedWorkBook || 0,
        employee: employees[i].EmployeeName + " (" + employees[i].UserName + " | " + employees[i].UserId + ")",
        inDueWorkBooks: employees[i].InDueWorkBook || 0,
        pastDueWorkBooks: employees[i].PastDueWorkBook || 0,
        completedWorkBooks: employees[i].CompletedWorkbook || 0,
        total: parseInt(employees[i].TotalEmployees) || 0
      });
    }

    if (length > 0) {
      this.state.myEmployeesArray = employeesArray;      
    }

    return rows;
  };

  /**
   * @method
   * @name - getMyEmployees
   * This method will used to get My Employees details supervisior
   * @param userId
   * @param supervisor
   * @returns none
   */
  async getMyEmployees(userId, supervisor) {
    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "",
      fields = [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=", }, { "Name": "CURRENT_USER", "Value": userId, "Operator": "=", "Bitwise": "AND" }];
    const postData = {
      "Fields": fields,
      "ColumnList": Constants.GET_EMPLOYEES_COLUMNS,
      "AppType": "WORKBOOK_DASHBOARD"
    };

    // Company Id get from session storage
    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let companyId = contractorManagementDetails.Company.Id || 0;

    let token = idToken,
      url = "/company/" + companyId + "/workbooks",
      response = await API.ProcessAPI(url, postData, token, false, "POST", true),
      myEmployees = response;

    if (response == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if (response == 'API_ERROR') {
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {
      this.props.updateMyEmployeesArray(myEmployees, supervisor);
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
    const currentUserId = this.state.selectedUserId || 0;

    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "",
      fields = [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "USER_ID", "Value": userId, "Operator": "=", "Bitwise": "and" }, { "Name": "PAST_DUE", "Value": "30", "Operator": "=", "Bitwise": "and" }];
    if (currentUserId) {
      let currentUserField = { "Name": "CURRENT_USER", "Value": currentUserId, "Operator": "=", "Bitwise": "AND" };
      fields.push(currentUserField);
    }
    const payLoad = {
      "Fields": fields,
      "ColumnList": Constants.GET_WORKBOOKS_PAST_DUE_COLUMNS,
      "AppType": "WORKBOOK_DASHBOARD"
    };

    // Company Id get from session storage
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
    const currentUserId = this.state.selectedUserId || 0;
    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "",
      fields = [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "USER_ID", "Value": userId, "Operator": "=", "Bitwise": "and" }, { "Name": "WORKBOOK_IN_DUE", "Value": "30", "Operator": "=", "Bitwise": "and" }];
    if (currentUserId) {
      let currentUserField = { "Name": "CURRENT_USER", "Value": currentUserId, "Operator": "=", "Bitwise": "AND" };
      fields.push(currentUserField);
    }
    const payLoad = {
      "Fields": fields,
      "ColumnList": Constants.GET_WORKBOOKS_COMING_DUE_COLUMNS,
      "AppType": "WORKBOOK_DASHBOARD"
    };

    let isComingDueModal = true,
      workBookComingDue = {};
    this.setState({ isComingDueModal, workBookComingDue });

    // Company Id get from session storage
    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let companyId = contractorManagementDetails.Company.Id || 0;

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
    const currentUserId = this.state.selectedUserId || 0;
    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "",
      fields = [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "USER_ID", "Value": userId, "Operator": "=", "Bitwise": "and" }, { "Name": "COMPLETED", "Value": "true", "Operator": "=", "Bitwise": "and" }];
    if (currentUserId) {
      let currentUserField = { "Name": "CURRENT_USER", "Value": currentUserId, "Operator": "=", "Bitwise": "AND" };
      fields.push(currentUserField);
    }
    const payLoad = {
      "Fields": fields,
      "ColumnList": Constants.GET_COMPLETED_WORKBOOKS_COLUMNS,
      "AppType": "WORKBOOK_DASHBOARD"
    };

    // Company Id get from session storage
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
   * @name - getAssignedWorkbooks
   * This method will used to get Assigned Workbooks details
   * @param userId
   * @returns none
   */
  async getAssignedWorkbooks(userId) {
    const currentUserId = this.state.selectedUserId || 0;
    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "",
      fields = [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "USER_ID", "Value": userId, "Operator": "=", "Bitwise": "and" }, { "Name": "ASSIGNED_WORKBOOK", "Value": "true", "Operator": "=", "Bitwise": "and" }];
    if (currentUserId) {
      let currentUserField = { "Name": "CURRENT_USER", "Value": currentUserId, "Operator": "=", "Bitwise": "AND" };
      fields.push(currentUserField);
    }
    const payLoad = {
      "Fields": fields,
      "ColumnList": Constants.GET_ASSIGNED_WORKBOOKS_COLUMNS,
      "AppType": "WORKBOOK_DASHBOARD"
    };

    // Company Id get from session storage
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
   * @name - componentWillReceiveProps
   * This method will invoked whenever the props or state
   *  is update to this component class
   * @param newProps
   * @returns none
   */
  componentWillReceiveProps(newProps) {
    const { sortColumn, sortDirection } = this.state;
    let rows = this.createRows(newProps.myEmployees),
      isArray = Array.isArray(newProps.myEmployees),
      isRows = newProps.myEmployees.length > 0 ? true : false;

    let isInitial = false;

    if (isArray && isRows) {
      isInitial = rows.length > 0 ? false : true;
    }

    if (sortColumn != "" && sortDirection != "NONE") {
      this.state.modal = newProps.modal;
      this.state.rows = rows;
      this.state.level = newProps.level;
      this.state.supervisorNames = newProps.supervisorNames;
      this.state.isInitial = isInitial;
      this.state.selectedUserId = newProps.selectedUserId
      this.handleGridSort(sortColumn, sortDirection);
    } else {
      this.setState({
        modal: newProps.modal,
        rows: rows,
        level: newProps.level,
        supervisorNames: newProps.supervisorNames,
        isInitial: isInitial,
        selectedUserId: newProps.selectedUserId
      });
    }
  }

  /**
   * @method
   * @name - toggle
   * This method will update the current of modal window
   * @param workbooks
   * @returns none
   */
  toggle() {
    let myEmployeesArray = this.state.myEmployeesArray,
      length = myEmployeesArray.length;

    if (length == 1 || length == 0 || length == undefined) {
      this.setState({
        modal: !this.state.modal
      });
      this.props.updateState("isMyEmployeeModal");
    } else if (length >= 1) {
      this.props.popMyEmployeesArray();
    }
  }

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
    this.state.sortColumn = sortColumn;
    this.state.sortDirection = sortDirection;

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
  * @param type
  * @param props
  * @returns none
  */
  employeeFormatter = (props) => {
    const { supervisorNames } = this.state;
    let supervisorNamesLength = supervisorNames.length > 0 ? supervisorNames.length - 1 : supervisorNames.length;
    let currentUserId = supervisorNames[supervisorNamesLength] ? supervisorNames[supervisorNamesLength].userId : 0;
    if (props.original.userId == currentUserId || props.original.total <= 0 || props.original.employee == "Total") {
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
          this.getMyEmployees(props.original.userId, props.original);
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
   * This method will format the workbooks column Data Grid
   * @param type
   * @param props
   * @returns none
   */
  workbookFormatter = (type, props) => {
    const { supervisorNames } = this.state;
    let supervisorNamesLength = supervisorNames.length > 0 ? supervisorNames.length - 1 : supervisorNames.length;
    let currentUserId = supervisorNames[supervisorNamesLength] ? supervisorNames[supervisorNamesLength].userId : 0;
    if (props.dependentValues.userId == currentUserId || props.dependentValues[type] <= 0 || props.dependentValues.employee == "Total") {
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
  * @name - handleCellClick
  * This method will trigger the event of API's respective to cell clicked Data Grid
  * @param type
  * @param args
  * @returns none
  */
  handleCellClick = (type, args) => {
    let userId = 0;
    switch (type) {
      case "completedWorkBooks":
        userId = args.userId;
        if (userId) {
          this.getCompletedWorkbooks(userId);
        }
        break;
      case "assignedWorkBooks":
        userId = args.userId;
        if (userId) {
          this.getAssignedWorkbooks(userId);
        }
        break;
      case "pastDueWorkBooks":
        userId = args.userId;
        if (userId) {
          this.getPastDueWorkbooks(userId);
        }
        break;
      case "inDueWorkBooks":
        userId = args.userId;
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

  customCell(props) {
    let self = this;
    return (
      props.value && <span onClick={e => { e.preventDefault(); self.handleCellClick(props.column.id, props.original); }} className={"text-clickable"}>
        {props.value}
      </span> || <span>{props.value}</span>
    );
  }

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
    const { rows, supervisorNames } = this.state;
    let supervisorNamesLength = supervisorNames.length > 0 ? supervisorNames.length - 1 : supervisorNames.length;
    let supervisorName = supervisorNames[supervisorNamesLength] ? ' - ' + supervisorNames[supervisorNamesLength].name : "";
    let pgSize = (rows.length > 10) ? rows.length : 10;
    return (
      <div>
        <SessionPopup
          backdropClassName={"backdrop"}
          modal={this.state.isSessionPopup}
          sessionPopupType={this.state.sessionPopupType}
        />
        <AssignedWorkBook
          backdropClassName={"no-backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isAssignedModal}
          assignedWorkBooks={this.state.assignedWorkBooks}
        />
        <WorkBookComingDue
          backdropClassName={"no-backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isComingDueModal}
          assignedWorkBooks={this.state.workBookComingDue}
        />
        <WorkBookDuePast
          backdropClassName={"no-backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isPastDueModal}
          assignedWorkBooks={this.state.workBookDuePast}
        />
        <WorkBookCompleted
          backdropClassName={"no-backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isCompletedModal}
          assignedWorkBooks={this.state.workBookCompleted}
        />
        <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} toggle={this.toggle} fade={false} centered={true} className="custom-modal-grid grid-modal-popup">
          <ModalHeader className="text-left" toggle={this.toggle}>
            My Employees{supervisorName}
            <p className="section-info-description">View the progress of the direct subordinates of employee</p>
          </ModalHeader>
          <div>
            <div className="export-menu-one">

            </div>
            <div className="export-menu-two">
              <Export
                data={this.state.rows}
                heads={this.heads}
                sheetName={"My Employees"}
              />
            </div>
          </div>
          <ModalBody className={""}>
            <div className="grid-container">
              <div className="table has-total-row">
                <ReactTable
                  minRows={1}
                  data={rows}
                  columns={[
                    {
                      Header: "Employee",
                      accessor: "employee",
                      headerClassName: 'header-wordwrap',
                      minWidth: 210,
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
                      headerClassName: 'header-wordwrap',
                      minWidth: 200,
                      className: 'text-left',
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
                  pageSize={this.state.isInitial ? 5 : pgSize}
                  loading={this.state.isInitial}
                  loadingText={''}
                  noDataText={!this.state.isInitial ? '' : 'Sorry, no records'}
                  defaultSorted={[
                    {
                      id: "employee",
                      desc: false
                    }
                  ]}
                  style={{
                    // minHeight: "575px", // This will force the table body to overflow and scroll, since there is not enough room
                    maxHeight: "550px"
                  }}
                />
              </div>
            </div>
          </ModalBody>
        </Modal>
      </div>
    );
  }
}

export default MyEmployees;
