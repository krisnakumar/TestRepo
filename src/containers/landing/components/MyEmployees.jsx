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
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import * as API from '../../../shared/utils/APIUtils';
import * as Constants from '../../../shared/constants';

/**
 * EmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-data-grid module.
 */
class EmptyRowsView extends React.Component{
  render() {
    return (<div className="no-records-found-modal">Sorry, no records</div>)
  }
};

class MyEmployees extends React.Component {
  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor(props) {
    super(props);

    this.heads = [
      {
        key: 'employee',
        name: 'Employee',
        sortable: true,
        width: 200,
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
      rows: this.createRows(this.props.myEmployees),
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
      supervisorNames: this.props.supervisorNames,      
      isAssignedModal: false,
      assignedWorkBooks: {},
      isInitial: false
    };
    this.toggle = this.toggle.bind(this);
    this.updateModalState = this.updateModalState.bind(this);
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
      assignedWorkBooksCount += parseInt(employees[i].AssignedWorkBook);
      inDueWorkBooksCount += parseInt(employees[i].InDueWorkBook);
      pastDueWorkBooksCount += parseInt(employees[i].PastDueWorkBook); 
      completedWorkBooksCount += parseInt(employees[i].CompletedWorkbook);
      totalEmpCount += parseInt(employees[i].TotalEmployees);      
      rows.push({
        userId: employees[i].UserId || 0,
        role: employees[i].Role,
        assignedWorkBooks: employees[i].AssignedWorkBook,
        employee: employees[i].EmployeeName,
        inDueWorkBooks: employees[i].InDueWorkBook,
        pastDueWorkBooks: employees[i].PastDueWorkBook,
        completedWorkBooks: employees[i].CompletedWorkbook,
        total: employees[i].TotalEmployees
      });
    }

    if(length > 0){      
      this.state.myEmployeesArray = employeesArray;
      rows.push({employee: "Total", role: "", assignedWorkBooks:assignedWorkBooksCount, inDueWorkBooks: inDueWorkBooksCount , pastDueWorkBooks:pastDueWorkBooksCount, completedWorkBooks:completedWorkBooksCount, total:totalEmpCount});
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
  async getMyEmployees(userId, supervisor){
    const { cookies } = this.props;
    const postData = {
      "Fields": [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=", }],
      "ColumnList": Constants.GET_EMPLOYEES_COLUMNS
    };

    let token = cookies.get('IdentityToken'),
        companyId = cookies.get('CompanyId'),
        url = "/company/" + companyId + "/workbooks",
        response = await API.ProcessAPI(url, postData, token, false, "POST", true),
        myEmployees = response;

      this.props.updateMyEmployeesArray(myEmployees, supervisor);
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
    const payLoad = {
      "Fields": [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "USER_ID", "Value": userId, "Operator": "=", "Bitwise":"and" }, { "Name": "PAST_DUE", "Value": "30", "Operator": "=", "Bitwise":"and" }],
      "ColumnList": Constants.GET_WORKBOOKS_PAST_DUE_COLUMNS
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
  async getComingDueWorkbooks(userId){
    const { cookies } = this.props;
    const payLoad = {
      "Fields": [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" },{ "Name": "USER_ID", "Value": userId, "Operator": "=", "Bitwise":"and" }, { "Name": "WORKBOOK_IN_DUE", "Value": "30", "Operator": "=", "Bitwise":"and" }],
      "ColumnList": Constants.GET_WORKBOOKS_COMING_DUE_COLUMNS
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
  async getCompletedWorkbooks(userId){
    const { cookies } = this.props;
    const payLoad = {
      "Fields": [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" },{ "Name": "USER_ID", "Value": userId, "Operator": "=", "Bitwise":"and" }, { "Name": "COMPLETED", "Value": "true", "Operator": "=", "Bitwise":"and" }],
      "ColumnList": Constants.GET_COMPLETED_WORKBOOKS_COLUMNS
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
   * @name - getAssignedWorkbooks
   * This method will used to get Assigned Workbooks details
   * @param userId
   * @returns none
   */
  async getAssignedWorkbooks(userId){
    const { cookies } = this.props;
    const payLoad = {
      "Fields": [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }, { "Name": "USER_ID", "Value": userId, "Operator": "=", "Bitwise":"and" }],
      "ColumnList": Constants.GET_ASSIGNED_WORKBOOKS_COLUMNS
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
   * @name - componentWillReceiveProps
   * This method will invoked whenever the props or state
   *  is update to this component class
   * @param newProps
   * @returns none
   */
  componentWillReceiveProps(newProps) {
      let rows = this.createRows(newProps.myEmployees),
          isArray = Array.isArray(newProps.myEmployees),
          isRows = newProps.myEmployees.length > 0 ? true : false;

      var isInitial = false;

      if(isArray && isRows){
        isInitial = rows.length > 0 ? false : true;
      } 

      this.setState({
        modal: newProps.modal,
        rows: rows,
        level: newProps.level,
        supervisorNames: newProps.supervisorNames,
        isInitial: isInitial
      });
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

    if(length == 1 || length == 0 || length == undefined){
      this.setState({
        modal: !this.state.modal
      });
      this.props.updateState("isMyEmployeeModal");
    } else if(length >= 1){
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

    const sortRows = beforePopRows.slice(0),
          rowsLength = this.state.rows.length || 0;
    const rows = sortDirection === 'NONE' ? this.state.rows.slice(0, rowsLength) : sortRows.sort(comparer).slice(0, rowsLength);
    
    if(beforePopRows.length > 0)
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
    if(props.dependentValues.total <= 0 || props.dependentValues.employee == "Total"){
      return (
        <span>{props.value}</span>
      );
    } else {
      return (
       <span onClick={e => { 
          e.preventDefault(); 
          this.getMyEmployees(props.dependentValues.userId, props.dependentValues); }} 
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
    if(props.dependentValues[type] <= 0 || props.dependentValues.employee == "Total"){
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
    switch(type) {
      case "completedWorkBooks":
          userId = args.userId;
          if(userId){
            this.getCompletedWorkbooks(userId);
          }  
          break;
      case "assignedWorkBooks":
          userId = args.userId;
          if(userId){
            this.getAssignedWorkbooks(userId);
          }  
          break;          
      case "pastDueWorkBooks":
          userId = args.userId;
          if(userId){
            this.getPastDueWorkbooks(userId);
          }  
          break;   
      case "inDueWorkBooks":
          userId = args.userId;
          if(userId){
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

  render() {
    const { rows, supervisorNames } = this.state;
    let supervisorNamesLength = supervisorNames.length > 0 ? supervisorNames.length - 1 : supervisorNames.length;
    let supervisorName = supervisorNames[supervisorNamesLength] ? ' - ' + supervisorNames[supervisorNamesLength]. name : "";
    return (     
      <div>
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
        <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"}  isOpen={this.state.modal} toggle={this.toggle} fade={false} centered={true} className="custom-modal-grid">
          <ModalHeader toggle={this.toggle}>My Employees{supervisorName}</ModalHeader>
          <ModalBody>
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
                      emptyRowsView={this.state.isInitial && EmptyRowsView} 
                  />
              </div>
            </div>
          </ModalBody>
        </Modal>
      </div>
    );
  }
}

export default withCookies(MyEmployees);