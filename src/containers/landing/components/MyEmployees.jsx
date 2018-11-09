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
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import * as API from '../../../shared/utils/APIUtils';

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
        width: 250,
        editable: false,
        cellClass: "text-left text-clickable"
      },
      {
        key: 'workbook',
        name: 'Workbooks',
        sortable: true,
        editable: false,
        cellClass: "text-left"
      },
      {
        key: 'inDueWorkBooks',
        name: 'Workbooks Due in 30 Days',
        width: 200,
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
        cellClass: "text-right last-column text-clickable"
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
      supervisorNames: this.props.supervisorNames
    };
    this.toggle = this.toggle.bind(this);
    this.updateModalState = this.updateModalState.bind(this);
  }

  componentDidCatch(error, info) {
    // Display fallback UI
    // this.setState({ hasError: true });
    // You can also log the error to an error reporting service
    console.log(error, info);
  }

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
      assignedWorkBooksCount += parseInt(employees[i].AssignedWorkBooks);
      inDueWorkBooksCount += parseInt(employees[i].InDueWorkBooks);
      pastDueWorkBooksCount += parseInt(employees[i].PastDueWorkBooks); 
      completedWorkBooksCount += parseInt(employees[i].CompletedWorkBooks);
      totalEmpCount += parseInt(employees[i].EmployeeCount);      
      rows.push({
        userId: employees[i].UserId,
        employee: employees[i].FirstName + ' ' + employees[i].LastName,
        workbook: employees[i].WorkbookName,
        inDueWorkBooks: employees[i].InDueWorkBooks,
        pastDueWorkBooks: employees[i].PastDueWorkBooks,
        completedWorkBooks: employees[i].CompletedWorkBooks,
        total: employees[i].EmployeeCount,
      });
    }

    if(length > 0){      
      this.state.myEmployeesArray = employeesArray;
      rows.push({employee: "Total", role: "", assignedWorkBooks:assignedWorkBooksCount, inDueWorkBooks: inDueWorkBooksCount , pastDueWorkBooks:pastDueWorkBooksCount, completedWorkBooks:completedWorkBooksCount, total:totalEmpCount});
    }
    

    return rows;
  };

  async getMyEmployees(userId){
    const { cookies } = this.props;

    let token = cookies.get('IdentityToken'),
        url = "https://omwlc1qx62.execute-api.us-west-2.amazonaws.com/dev/users/"+ userId +"/employees",
        response = await API.ProcessAPI(url, "", token, false, "GET", true),
        myEmployees = response,
        isMyEmployeeModal = this.state.isMyEmployeeModal;

      this.props.updateMyEmployeesArray(myEmployees);
  };

  async getPastDueWorkbooks(userId){
    const { cookies } = this.props;

    let token = cookies.get('IdentityToken'),
        url = "https://omwlc1qx62.execute-api.us-west-2.amazonaws.com/dev/users/"+ userId +"/workbooks/pastdue",
        response = await API.ProcessAPI(url, "", token, false, "GET", true),
        workBookDuePast = response,
        isPastDueModal = this.state.isPastDueModal;

      isPastDueModal = true;
      this.setState({ ...this.state, isPastDueModal, workBookDuePast });
  };

  async getComingDueWorkbooks(userId){
    const { cookies } = this.props;

    let token = cookies.get('IdentityToken'),
        url = "https://omwlc1qx62.execute-api.us-west-2.amazonaws.com/dev/users/"+ userId +"/workbooks/comingdue",
        response = await API.ProcessAPI(url, "", token, false, "GET", true),
        workBookComingDue = response,
        isComingDueModal = this.state.isComingDueModal;

        isComingDueModal = true;
    this.setState({ ...this.state, isComingDueModal, workBookComingDue });
  };

  async getCompletedWorkbooks(userId){
    const { cookies } = this.props;

    let token = cookies.get('IdentityToken'),
        url = "https://omwlc1qx62.execute-api.us-west-2.amazonaws.com/dev/users/"+ userId + "/workbooks/completed",
        response = await API.ProcessAPI(url, "", token, false, "GET", true),
        workBookCompleted = response,
        isCompletedModal = this.state.isCompletedModal;

    isCompletedModal = true;
    this.setState({ ...this.state, isCompletedModal, workBookCompleted });
  };

  componentWillReceiveProps(newProps) {
    if(this.state.modal != newProps.modal){
      let rows = this.createRows(newProps.myEmployees);
      this.setState({
        modal: newProps.modal,
        rows: rows,
        level: newProps.level,
        supervisorNames: newProps.supervisorNames
      });
    } else if(this.state.level != newProps.level){
      let rows = this.createRows(newProps.myEmployees);
      this.setState({
        modal: newProps.modal,
        rows: rows,
        level: newProps.level,
        supervisorNames: newProps.supervisorNames
      });
    }
  }

  toggle() {
    let myEmployeesArray = this.state.myEmployeesArray,
        length = myEmployeesArray.length;

    if(length == 1 || length == 0){
      this.setState({
        modal: !this.state.modal
      });
      this.props.updateState("isMyEmployeeModal");
    } else if(length >= 1){
      this.props.popMyEmployeesArray();
    }   
  }

  handleGridRowsUpdated = ({ fromRow, toRow, updated }) => {
    const rows = this.state.rows.slice();

    for (let i = fromRow; i <= toRow; i += 1) {
      const rowToUpdate = rows[i];
      rows[i] = update(rowToUpdate, { $merge: updated });
    }

    this.setState({ rows });
  };

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

  rowGetter = i => this.state.rows[i];

  EmptyRowsView() {
      return (<div>Sorry, no records :(</div>)
  };

  updateModalState = (modelName) => {
    let value = !this.state[modelName];
    this.setState({
      [modelName]: value
    });
  };

  handleCellFocus = (args) => {
    if(args.idx == 0 || args.idx == 5){
      let userId = this.state.rows[args.rowIdx].userId;

      if(userId)
      this.getMyEmployees(userId);
    } else if(args.idx == 3){
      let userId = this.state.rows[args.rowIdx].userId;

      if(userId)
      this.getPastDueWorkbooks(userId);
    } else if(args.idx == 2){
      let userId = this.state.rows[args.rowIdx].userId;

      if(userId)
      this.getComingDueWorkbooks(userId);
    } else if(args.idx == 4){
      let userId = this.state.rows[args.rowIdx].userId;

      if(userId)
      this.getCompletedWorkbooks(userId);
    }
    this.refs.reactDataGrid.deselect();
  };


  render() {
    const { rows, supervisorNames } = this.state;
    let supervisorNamesLength = supervisorNames.length == 1 ? supervisorNames.length - 1 : supervisorNames.length - 2;
    let supervisorName = supervisorNames[supervisorNamesLength];
    return (     
      <div>
         <WorkBookComingDue
            updateState={this.updateModalState.bind(this)}
            modal={this.state.isComingDueModal}
            assignedWorkBooks={this.state.workBookComingDue}
          />
           <WorkBookDuePast
            updateState={this.updateModalState.bind(this)}
            modal={this.state.isPastDueModal}
            assignedWorkBooks={this.state.workBookDuePast}
          />
          <WorkBookCompleted
              updateState={this.updateModalState.bind(this)}
              modal={this.state.isCompletedModal}
              assignedWorkBooks={this.state.workBookCompleted}
          />
        <Modal isOpen={this.state.modal} toggle={this.toggle} fade={false} centered={true} className="custom-modal-grid">
          <ModalHeader toggle={this.toggle}>My Employees - {supervisorName}</ModalHeader>
          <ModalBody>
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
                      emptyRowsView={EmptyRowsView} 
                      onCellSelected={(args) => { this.handleCellFocus(args) }}
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