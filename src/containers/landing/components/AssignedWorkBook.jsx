/* eslint-disable */
/*
* AssignedWorkBook.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Assigned workbooks details to list the workbooks progress details
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------
createRows(employees)
toggle()
handleGridRowsUpdated(fromRow, toRow, updated)
handleGridSort(sortColumn, sortDirection)
updateModalState(modelName)
handleCellFocus(args) 
*/
import React from 'react';
import { Modal, ModalHeader, ModalBody } from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';
import WorkBookProgress from './WorkBookProgress';
import * as API from '../../../shared/utils/APIUtils';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';


/**
 * AssignedWorkBookEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react data grid module.
 */
class AssignedWorkBookEmptyRowsView extends React.Component{
  render() {
    return (<div className="no-records-found-modal">Sorry, no records</div>)
  }
};

class AssignedWorkBook extends React.Component {

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
        width: 300,
        editable: false,
        cellClass: "text-left"
      },
      {
        key: 'workbookName',
        name: 'Workbook Name',
        sortable: true,
        editable: false,
        cellClass: "text-left"
      },
      {
        key: 'completedTasks',
        name: 'Completed / Total Tasks',
        sortable: true,
        editable: false,
        cellClass: "text-center text-clickable"
      },
      {
        key: 'percentageCompleted',
        name: 'Percentage Completed',
        sortable: true,
        editable: false,
        cellClass: "text-center text-clickable"
      },
      {
        key: 'dueDate',
        name: 'Due Date',
        sortable: true,
        editable: false,
        cellClass: "text-center last-column"
      },
    ];

    this.employees = [];

    this.state = {
      modal: this.props.modal,      
      rows: this.createRows(this.props.assignedWorkBooks),
      pageOfItems: [],
      isWorkBookProgressModal: false,
      workBooksProgress: {}
    };
    this.toggle = this.toggle.bind(this);
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
   * @name - getWorkBookProgress
   * This method will used to get workbook progress details
   * @param userId
   * @param workBookId
   * @returns none
   */
  async getWorkBookProgress(userId, workBookId){
    const { cookies } = this.props;

    let token = cookies.get('IdentityToken'),
        url = "https://klrg45ssob.execute-api.us-west-2.amazonaws.com/dev/users/"+ userId +"/assigned-workbooks/"+ workBookId +"/tasks",
        response = await API.ProcessAPI(url, "", token, false, "GET", true),
        workBooksProgress = response,
        isWorkBookProgressModal = this.state.isWorkBookProgressModal;

        isWorkBookProgressModal = true;
    this.setState({ ...this.state, isWorkBookProgressModal, workBooksProgress });
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
    const rows = [], 
          length = employees ? employees.length : 0;
    for (let i = 0; i < length; i++) { 
        let dueDate = employees[i].DueDate.split("T")[0];
      rows.push({
        userId:  employees[i].UserId,
        workBookId: employees[i].WorkBookId,
        workbookName: employees[i].WorkbookName,
        employee: employees[i].EmployeeName,
        role: employees[i].Role,
        completedTasks: employees[i].CompletedTasks,
        percentageCompleted: employees[i].PercentageCompleted + "%",
        dueDate: dueDate
      });
    }

    return rows;
  };

  /**
   * @method
   * @name - componentWillReceiveProps
   * This method will invoked whenever the props or state
   * is update to this component class
   * @param newProps
   * @returns none
   */
  componentWillReceiveProps(newProps) {
    if(this.state.modal != newProps.modal){
      let rows = this.createRows(newProps.assignedWorkBooks);
      this.setState({
        modal: newProps.modal,
        rows: rows
      });
    }
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
    this.setState({
      [modelName]: value
    });
  };

  /**
   * @method
   * @name - toggle
   * This method will update the current of modal window
   * @param workbooks
   * @returns none
   */
  toggle() {
    this.setState({
      modal: !this.state.modal
    });
    this.props.updateState("isAssignedModal");
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

    const sortRows = this.state.rows.slice(0);
    const rows = sortDirection === 'NONE' ? this.state.rows.slice(0, 10) : sortRows.sort(comparer).slice(0, 10);

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
    if(args.idx == 2 || args.idx == 3){
      let userId = this.state.rows[args.rowIdx].userId,
          workBookId = this.state.rows[args.rowIdx].workBookId;

      if(userId && workBookId)
        this.getWorkBookProgress(userId, workBookId);      
    }
    this.refs.reactDataGrid.deselect();
  };
  
  // This method is used to setting the row data in react data grid
  rowGetter = i => this.state.rows[i];

  render() {
    const { rows } = this.state;
    return (
      <div>
        <WorkBookProgress
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isWorkBookProgressModal}
          workBooksProgress={this.state.workBooksProgress}
        />
        <Modal isOpen={this.state.modal}  fade={false}  toggle={this.toggle} centered={true} className="custom-modal-grid">
          <ModalHeader toggle={this.toggle}>Assigned Workbook</ModalHeader>
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
                      onCellSelected={(args) => { this.handleCellFocus(args) }}
                      emptyRowsView={AssignedWorkBookEmptyRowsView} 
                  />
              </div>
            </div>
          </ModalBody>
        </Modal>
      </div>
    );
  }
}

export default withCookies(AssignedWorkBook);