/* eslint-disable */
import React from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Card, CardBody, Col } from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';

class WorkbookLevelTwo extends React.Component {
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
        cellClass: "text-right"
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
        cellClass: "text-right"
      },
    ];

    this.employees = [];

    this.state = {
      modal: this.props.modal,      
      rows: this.createRows(this.props.myEmployees),
      pageOfItems: [],
      levelTwoWB: false,
      level3WB: false
    };
    this.toggle = this.toggle.bind(this);
  }

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

    const sortRows = this.state.rows.slice(0);
    const rows = sortDirection === 'NONE' ? this.state.rows.slice(0, 10) : sortRows.sort(comparer).slice(0, 10);

    this.setState({ rows });
  };

  rowGetter = i => this.state.rows[i];

  render() {
    const { rows } = this.state;
    return (
      <div>
        <Modal isOpen={this.state.modal} toggle={this.toggle} centered={true} className="custom-modal-grid">
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
                  />
              </div>
            </div>
          </ModalBody>
        </Modal>
      </div>
    );
  }
}

export default WorkbookLevelTwo;