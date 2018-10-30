/* eslint-disable */
import React from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Card, CardBody, Col } from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';
import AssignedWorkBook from './AssignedWorkBook';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';

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
        cellClass: "text-left"
      },
      {
        key: 'workbook',
        name: 'Workbook',
        sortable: true,
        editable: false,
        cellClass: "text-left"
      },
      {
        key: 'inDueWorkBooks',
        name: 'WorkBook Due in 30 Days',
        width: 200,
        sortable: true,
        editable: false,
        cellClass: "text-right text-clickable"
      },
      {
        key: 'pastDueWorkBooks',
        name: 'Past Due WorkBooks',
        sortable: true,
        editable: false,
        cellClass: "text-right"
      },
      {
        key: 'completedWorkBooks',
        name: 'Completed WorkBook',
        sortable: true,
        editable: false,
        cellClass: "text-right"
      },
      {
        key: 'total',
        name: 'Total Employees',
        sortable: true,
        editable: false,
        cellClass: "text-right last-column"
      },
    ];

    this.employees = [];

    this.state = {
      modal: this.props.modal,      
      rows: this.createRows(this.props.myEmployees),
      pageOfItems: [],
      levelTwoWB: false,
      level3WB: false,
      isAssignedWorkBook: false,
      assignedWorkBooks: {}
    };
    this.toggle = this.toggle.bind(this);
    this.updateModalState = this.updateModalState.bind(this);
  }

  componentDidCatch(error, info) {
    // Display fallback UI
    //this.setState({ hasError: true });
    // You can also log the error to an error reporting service
    console.log(error, info);
  }

  getAssignedWorkbooks = (userId) => {
    let _self = this,
        url = "https://fp34gqm7i7.execute-api.us-west-2.amazonaws.com/test/users/"+ userId +"/workbooks/assigned";
    const { cookies } = _self.props;
    fetch(url,  {headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      "Authorization": cookies.get('IdentityToken')
    }})
    .then(function(response) {
      return response.json()
    }).then(function(json) { 
        let assignedWorkBooks = json,
        isAssignedWorkBook = _self.state.isAssignedWorkBook;
        isAssignedWorkBook = true;
        _self.setState({ ..._self.state, isAssignedWorkBook, assignedWorkBooks });
        return json
    }).catch(function(ex) {
      console.log('parsing failed', ex)
    })
  };

  createRows = (employees) => {
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

    if(length > 0)
    rows.push({employee: "Total", role: "", assignedWorkBooks:assignedWorkBooksCount, inDueWorkBooks: inDueWorkBooksCount , pastDueWorkBooks:pastDueWorkBooksCount, completedWorkBooks:completedWorkBooksCount, total:totalEmpCount});

    return rows;
  };

  componentWillReceiveProps(newProps) {
    if(this.state.modal != newProps.modal){
      let rows = this.createRows(newProps.myEmployees);
      this.setState({
        modal: newProps.modal,
        rows: rows
      });
    }
  }

  toggle() {
    this.setState({
      modal: !this.state.modal
    });
    this.props.updateState("levelTwoWB");
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
    if(args.idx == 2){
      let userId = this.state.rows[args.rowIdx].userId;

      if(userId)
      this.getAssignedWorkbooks(userId); 

    }
    this.refs.reactDataGrid.deselect();
  };


  render() {
    const { rows } = this.state;
    return (     
      <div>
         <AssignedWorkBook
            updateState={this.updateModalState.bind(this)}
            modal={this.state.isAssignedWorkBook}
            assignedWorkBooks={this.state.assignedWorkBooks}
          />
        <Modal isOpen={this.state.modal} toggle={this.toggle} fade={false} centered={true} className="custom-modal-grid">
          <ModalHeader toggle={this.toggle}>My Employees(Supervisor View)</ModalHeader>
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
                      rowHeight={44}
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