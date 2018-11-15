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
import { CardBody} from 'reactstrap';
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

/**
 * DataTableEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-table module.
 */
class DataTableEmptyRowsView extends React.Component{
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
        cellClass: "text-left text-clickable"
      },
      {
        key: 'role',
        name: 'Role',
        sortable: true,
        editable: false,
        cellClass: "text-left"
      },
      {
        key: 'assignedWorkBooks',
        name: 'Assigned Workbooks',
        sortable: true,
        editable: false,
        cellClass: "text-right text-clickable"
      },
      {
        key: 'inDueWorkBooks',
        name: 'Workbooks Due',
        sortable: true,
        editable: false,
        cellClass: "text-right text-clickable"
      },
      {
        key: 'pastDueWorkBooks',
        name: 'Past Due Workbooks',
        sortable: true,
        editable: false,
        cellClass: "text-right text-clickable"
      },
      {
        key: 'completedWorkBooks',
        name: 'Completed Workbooks',
        sortable: true,
        editable: false,
        cellClass: "text-right text-clickable"
      },
      {
        key: 'total',
        name: 'Total Employees',
        sortable: true,
        editable: false,
        cellClass: "text-right text-clickable last-column"
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
    if(modelName == "isMyEmployeeModal"){
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
   * @returns none
   */
  updateMyEmployeesArray= (employees) => {
    let myEmployeesArray = this.state.myEmployeesArray,
        level = this.state.level + 1,
        supervisorNames = this.state.supervisorNames;

    if(employees.length > 0)
      supervisorNames.push(employees[0].FirstName + " " + employees[0].LastName);

    myEmployeesArray.push(employees);
    this.setState({ ...this.state, myEmployeesArray, level, supervisorNames });
  };

  /**
   * @method
   * @name - popMyEmployeesArray
   * This method will detele last element of MyEmployees of state of this component
   * @param none
   * @returns none
   */
  popMyEmployeesArray= () => {
    let myEmployeesArray = this.state.myEmployeesArray,    
        level = this.state.level - 1,
        supervisorNames = this.state.supervisorNames;  

    if(myEmployeesArray.length > 0)
    {
      let totalRow = myEmployeesArray.pop();
    }
    if(supervisorNames.length > 0)
    {
      let totalRow = supervisorNames.pop();
    }
    this.setState({ ...this.state, myEmployeesArray, level, supervisorNames});
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
    let companyId = cookies.get('CompanyId')   
    this.getEmployees(companyId);
  };

  /**
   * @method
   * @name - getEmployees
   * This method will used to get Employees details
   * @param userId
   * @returns none
   */
  async getEmployees(userId){
    const { cookies } = this.props;
    let token = cookies.get('IdentityToken'),
        url = "https://klrg45ssob.execute-api.us-west-2.amazonaws.com/dev/users/"+ userId +"/employees",
        response = await API.ProcessAPI(url, "", token, false, "GET", true),
        rows = this.createRows(response),
        isInitial = true;
    this.setState({ rows: rows, isInitial: isInitial});
    this.onChangePage([]);
  };

  /**
   * @method
   * @name - getMyEmployees
   * This method will used to get My Employees details
   * @param userId
   * @returns none
   */
  async getMyEmployees(userId){
    const { cookies } = this.props;

    let token = cookies.get('IdentityToken'),
        url = "https://klrg45ssob.execute-api.us-west-2.amazonaws.com/dev/users/"+ userId +"/employees",
        response = await API.ProcessAPI(url, "", token, false, "GET", true),
        myEmployees = response,
        myEmployeesArray = this.state.myEmployeesArray,
        isMyEmployeeModal = this.state.isMyEmployeeModal,
        fakeState = this.state.fakeState,
        level = this.state.level + 1 ;
        
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
  async getAssignedWorkbooks(userId){
    const { cookies } = this.props;

    let isAssignedModal = this.state.isAssignedModal,
        assignedWorkBooks = {};
    isAssignedModal = true;
    this.setState({ isAssignedModal, assignedWorkBooks });

    let token = cookies.get('IdentityToken'),
        url = "https://klrg45ssob.execute-api.us-west-2.amazonaws.com/dev/users/"+ userId +"/workbooks/assigned",
        response = await API.ProcessAPI(url, "", token, false, "GET", true);

        
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
  async getPastDueWorkbooks(userId){    
    const { cookies } = this.props;

    let isPastDueModal = this.state.isPastDueModal,
        workBookDuePast = {};
        isPastDueModal = true;
    this.setState({ isPastDueModal, workBookDuePast });

    let token = cookies.get('IdentityToken'),
        url = "https://klrg45ssob.execute-api.us-west-2.amazonaws.com/dev/users/"+ userId +"/workbooks/pastdue",
        response = await API.ProcessAPI(url, "", token, false, "GET", true);

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
  async getComingDueWorkbooks(userId){
    const { cookies } = this.props;

    let isComingDueModal = this.state.isComingDueModal,
        workBookComingDue = {};
        isComingDueModal = true;
    this.setState({ isComingDueModal, workBookComingDue });

    let token = cookies.get('IdentityToken'),
        url = "https://klrg45ssob.execute-api.us-west-2.amazonaws.com/dev/users/"+ userId +"/workbooks/comingdue",
        response = await API.ProcessAPI(url, "", token, false, "GET", true);

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
  async getCompletedWorkbooks(userId){
    const { cookies } = this.props;

    let isCompletedModal = this.state.isCompletedModal,
        workBookCompleted = {};
        isCompletedModal = true;
    this.setState({ isCompletedModal, workBookCompleted });

    let token = cookies.get('IdentityToken'),
        url = "https://klrg45ssob.execute-api.us-west-2.amazonaws.com/dev/users/"+ userId + "/workbooks/completed",
        response = await API.ProcessAPI(url, "", token, false, "GET", true);

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
      assignedWorkBooksCount += parseInt(employees[i].AssignedWorkBooks);
      inDueWorkBooksCount += parseInt(employees[i].InDueWorkBooks);
      pastDueWorkBooksCount += parseInt(employees[i].PastDueWorkBooks);
      completedWorkBooksCount += parseInt(employees[i].CompletedWorkBooks);
      totalEmpCount += parseInt(employees[i].EmployeeCount)
      rows.push({
        userId: employees[i].UserId,
        employee: employees[i].FirstName + ' ' + employees[i].LastName,
        role: employees[i].Role,
        assignedWorkBooks: employees[i].AssignedWorkBooks,
        inDueWorkBooks: employees[i].InDueWorkBooks,
        pastDueWorkBooks: employees[i].PastDueWorkBooks,
        completedWorkBooks: employees[i].CompletedWorkBooks,
        total: employees[i].EmployeeCount,
      });
    }

    if(length > 0)
    rows.push({employee: "Total", role: "", assignedWorkBooks:assignedWorkBooksCount, inDueWorkBooks: inDueWorkBooksCount , pastDueWorkBooks:pastDueWorkBooksCount, completedWorkBooks:completedWorkBooksCount, total:totalEmpCount});

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
    if(beforePopRows.length > 0)
    {
      totalRow = beforePopRows.pop();
    }

    const sortRows = beforePopRows.slice(0);
    const rows = sortDirection === 'NONE' ? this.state.rows.slice(0, 10) : sortRows.sort(comparer).slice(0, 10);
    
    if(beforePopRows.length > 0)
      rows.push(totalRow);

    this.setState({ rows });
  };

  /**
   * @method
   * @name - handleCellFocus
   * This method will trigger the event of API's respective to cell clicked Data Grid
   * @param args
   * @returns none
   */
  handleCellFocus = (args) => {
    if(args.idx == 0 || args.idx == 6){
      let userId = this.state.rows[args.rowIdx].userId;
      let employee = this.state.rows[args.rowIdx].employee;
      
      if(userId){        
        let supervisorNames = this.state.supervisorNames,
            isMyEmployeeModal = this.state.isMyEmployeeModal,
            myEmployeesArray =  this.state.myEmployeesArray;

        supervisorNames.push(employee);
        isMyEmployeeModal = true;
        myEmployeesArray = [];

        this.setState({ isMyEmployeeModal, myEmployeesArray });
        this.getMyEmployees(userId);
      }
    } else if(args.idx == 2){
      let userId = this.state.rows[args.rowIdx].userId;

      if(userId){        
        this.getAssignedWorkbooks(userId);
      }
    } else if(args.idx == 4){
      let userId = this.state.rows[args.rowIdx].userId;

      if(userId){
        this.getPastDueWorkbooks(userId);
      }
    } else if(args.idx == 3){
      let userId = this.state.rows[args.rowIdx].userId;

      if(userId){
        this.getComingDueWorkbooks(userId);
      }      
    } else if(args.idx == 5){
      let userId = this.state.rows[args.rowIdx].userId;

      if(userId){
        this.getCompletedWorkbooks(userId);
      }      
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
              <img src="https://d2tqbrn06t95pa.cloudfront.net/img/topnav_reports.png?v=2"/> Workbooks Dashboard
            </div>
            </div>
            <div className="grid-container">
              <div className="table">
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
                      onCellSelected={(args) => { this.handleCellFocus(args) }}
                      emptyRowsView={this.state.isInitial && DataTableEmptyRowsView} 
                  />
              </div>
            </div>
          </CardBody>
    );
  }
}

export default withCookies(WorkBookDashboard);