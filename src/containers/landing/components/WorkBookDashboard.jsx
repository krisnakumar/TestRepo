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
import { CardBody } from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import MyEmployees from './MyEmployees';
import AssignedWorkBook from './AssignedWorkBook';
import WorkBookDuePast from './WorkBookDuePast';
import WorkBookComingDue from './WorkBookComingDue';
import WorkBookCompleted from './WorkBookCompleted';
import * as API from '../../../shared/utils/APIUtils';
import * as Constants from '../../../shared/constants';

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

class WorkBookDashboard extends PureComponent {

  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

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
        cellClass: "text-right last-column"
      },
    ];

    this.employees = [];

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
      isInitial: false
    };

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
    if (props.dependentValues.total <= 0 || props.dependentValues.employee == "Total") {
      return (
        <span>{props.value}</span>
      );
    } else {
      return (
        <span onClick={e => {
          e.preventDefault();
          let isMyEmployeeModal = true,
            myEmployeesArray = [],
            supervisorNames = this.state.supervisorNames;

          supervisorNames = [];
          supervisorNames.push({ 'name': props.dependentValues.employee, 'column': "NONE", 'order': "NONE" });
          this.setState({ isMyEmployeeModal, myEmployeesArray, supervisorNames });
          this.getMyEmployees(props.dependentValues.userId);
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
    if (props.dependentValues[type] <= 0 || props.dependentValues.employee == "Total") {
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
    let myEmployeesArray = this.state.myEmployeesArray,
      level = this.state.level + 1,
      supervisorNames = this.state.supervisorNames,
      employeesLength = employees.length;

    if (employeesLength > 0)
      supervisorNames.push({ 'name': supervisor.employee, 'column': "NONE", 'order': "NONE" });

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
      let totalRow = myEmployeesArray.pop();
    }
    if (supervisorNames.length > 0) {
      let totalRow = supervisorNames.pop();
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
  componentDidMount() {
    const { cookies } = this.props;
    let companyId = cookies.get('CompanyId'),
      userId = cookies.get('UserId');
    this.getEmployees(companyId, userId);
  };

  /**
   * @method
   * @name - getEmployees
   * This method will used to get Employees details
   * @param userId
   * @returns none
   */
  async getEmployees(companyId, userId) {
    const { cookies } = this.props;
    const postData = {
      "Fields": [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=", }],
      "ColumnList": Constants.GET_EMPLOYEES_COLUMNS
    };
    let token = cookies.get('IdentityToken'),
      url = "/company/" + companyId + "/workbooks",
      response = await API.ProcessAPI(url, postData, token, false, "POST", true),
      rows = this.createRows(response),
      isInitial = true;
    this.setState({ rows: rows, isInitial: isInitial });
    this.onChangePage([]);
  };

  /**
   * @method
   * @name - getMyEmployees
   * This method will used to get My Employees details
   * @param userId
   * @returns none
   */
  async getMyEmployees(userId) {
    const { cookies } = this.props;
    const postData = {
      "Fields": [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=", }],
      "ColumnList": Constants.GET_EMPLOYEES_COLUMNS
    };

    let token = cookies.get('IdentityToken'),
      companyId = cookies.get('CompanyId'),
      url = "/company/" + companyId + "/workbooks",
      response = await API.ProcessAPI(url, postData, token, false, "POST", true),
      myEmployees = response,
      myEmployeesArray = this.state.myEmployeesArray,
      isMyEmployeeModal = this.state.isMyEmployeeModal,
      fakeState = this.state.fakeState,
      level = this.state.level + 1;

    myEmployeesArray = [];
    myEmployeesArray.push(myEmployees);
    fakeState = !fakeState;
    isMyEmployeeModal = true;
    this.setState({ ...this.state, isMyEmployeeModal, myEmployees, myEmployeesArray, fakeState, level });
  };

  /**
   * @method
   * @name - getAssignedWorkbooks
   * This method will used to get Assigned Workbooks details
   * @param userId
   * @returns none
   */
  async getAssignedWorkbooks(userId) {
    const { cookies } = this.props;
    const payLoad = {
      "Fields": [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "ASSIGNED", "Value": "true", "Operator": "=" }],
      "ColumnList": ["USERID", "WORKBOOK_ID", "EMPLOYEE_NAME", "WORKBOOK_NAME", "COMPLETED_WORKBOOK", "TOTAL_WORKBOOK", "DUE_DATE"]
    };

    let isAssignedModal = this.state.isAssignedModal,
      assignedWorkBooks = {};
    isAssignedModal = true;
    this.setState({ isAssignedModal, assignedWorkBooks });

    let token = cookies.get('IdentityToken'),
      companyId = cookies.get('CompanyId'),
      url = "/company/" + companyId + "/workbooks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

    assignedWorkBooks = response;
    isAssignedModal = true;
    this.setState({ ...this.state, isAssignedModal, assignedWorkBooks });
  };

  /**
  * @method
  * @name - getPastDueWorkbooks
  * This method will used to get Past Due Workbooks details
  * @param userId
  * @returns none
  */
  async getPastDueWorkbooks(userId) {
    const { cookies } = this.props;
    const payLoad = {
      "Fields": [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "PAST_DUE", "Value": "true", "Operator": "=" }],
      "ColumnList": ["USERID", "ROLE", "WORKBOOK_ID", "EMPLOYEE_NAME", "WORKBOOK_NAME", "COMPLETED_WORKBOOK", "TOTAL_WORKBOOK", "DUE_DATE"]
    };

    let isPastDueModal = this.state.isPastDueModal,
      workBookDuePast = {};
    isPastDueModal = true;
    this.setState({ isPastDueModal, workBookDuePast });

    let token = cookies.get('IdentityToken'),
      companyId = cookies.get('CompanyId'),
      url = "/company/" + companyId + "/workbooks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

    workBookDuePast = response;
    isPastDueModal = true;
    this.setState({ ...this.state, isPastDueModal, workBookDuePast });
  };

  /**
   * @method
   * @name - getComingDueWorkbooks
   * This method will used to get Coming Due Workbooks details
   * @param userId
   * @returns none
   */
  async getComingDueWorkbooks(userId) {
    const { cookies } = this.props;
    const payLoad = {
      "Fields": [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "WORKBOOK_IN_DUE", "Value": "true", "Operator": "=" }],
      "ColumnList": ["USERID", "ROLE", "WORKBOOK_ID", "EMPLOYEE_NAME", "WORKBOOK_NAME", "COMPLETED_WORKBOOK", "TOTAL_WORKBOOK", "DUE_DATE"]
    };

    let isComingDueModal = this.state.isComingDueModal,
      workBookComingDue = {};
    isComingDueModal = true;
    this.setState({ isComingDueModal, workBookComingDue });

    let token = cookies.get('IdentityToken'),
      companyId = cookies.get('CompanyId'),
      url = "/company/" + companyId + "/workbooks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

    workBookComingDue = response;
    isComingDueModal = true;
    this.setState({ ...this.state, isComingDueModal, workBookComingDue });
  };

  /**
   * @method
   * @name - getCompletedWorkbooks
   * This method will used to get Completed Workbooks details
   * @param userId
   * @returns none
   */
  async getCompletedWorkbooks(userId) {
    const { cookies } = this.props;
    const payLoad = {
      "Fields": [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "COMPLETED", "Value": "true", "Operator": "=" }],
      "ColumnList": ["USERID", "ROLE", "WORKBOOK_ID", "EMPLOYEE_NAME", "WORKBOOK_NAME", "COMPLETED_WORKBOOK", "TOTAL_WORKBOOK", "DUE_DATE", "LAST_ATTEMPT_DATE"]
    };

    let isCompletedModal = this.state.isCompletedModal,
      workBookCompleted = {};
    isCompletedModal = true;
    this.setState({ isCompletedModal, workBookCompleted });

    let token = cookies.get('IdentityToken'),
      companyId = cookies.get('CompanyId'),
      url = "/company/" + companyId + "/workbooks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

    workBookCompleted = response;

    isCompletedModal = true;
    this.setState({ ...this.state, isCompletedModal, workBookCompleted });
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
    const rows = [],
      length = employees ? employees.length : 0;
    for (let i = 0; i < length; i++) {
      assignedWorkBooksCount += parseInt(employees[i].AssignedWorkBook);
      inDueWorkBooksCount += parseInt(employees[i].InDueWorkBook);
      pastDueWorkBooksCount += parseInt(employees[i].PastDueWorkBook);
      completedWorkBooksCount += parseInt(employees[i].CompletedWorkbook);
      totalEmpCount += parseInt(employees[i].TotalEmployees)
      rows.push({
        userId: employees[i].UserId || 0,
        employee: employees[i].EmployeeName,
        role: employees[i].Role,
        assignedWorkBooks: employees[i].AssignedWorkBook,
        inDueWorkBooks: employees[i].InDueWorkBook,
        pastDueWorkBooks: employees[i].PastDueWorkBook,
        completedWorkBooks: employees[i].CompletedWorkbook,
        total: employees[i].TotalEmployees
      });
    }

    if (length > 0)
      rows.push({ employee: "Total", role: "", assignedWorkBooks: assignedWorkBooksCount, inDueWorkBooks: inDueWorkBooksCount, pastDueWorkBooks: pastDueWorkBooksCount, completedWorkBooks: completedWorkBooksCount, total: totalEmpCount });

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
        return (a[sortColumn] > b[sortColumn]) ? 1 : -1;
      } else if (sortDirection === 'DESC') {
        return (a[sortColumn] < b[sortColumn]) ? 1 : -1;
      }
    };

    const beforePopRows = this.state.rows;
    let totalRow = "";
    if (beforePopRows.length > 0) {
      totalRow = beforePopRows.pop();
    }

    const sortRows = beforePopRows.slice(0);
    const rows = sortDirection === 'NONE' ? this.state.rows.slice(0, 10) : sortRows.sort(comparer).slice(0, 10);

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
    this.refs.reactDataGrid.deselect();
  };

  // This method is used to setting the row data in react data grid
  rowGetter = i => this.state.rows[i];

  render() {
    const { rows } = this.state;
    return (
      <CardBody>
        <MyEmployees
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
          <div className="pageheader">
            <img src="https://d2tqbrn06t95pa.cloudfront.net/img/topnav_reports.png?v=2" /> Workbook Dashboard
            </div>
          <p className="card__description">View list of workbooks assigned, completed and due for employees</p>
        </div>
        <div className="grid-container">
          <div className="table has-total-row">
            <ReactDataGrid
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
            />
          </div>
        </div>
      </CardBody>
    );
  }
}

export default withCookies(WorkBookDashboard);