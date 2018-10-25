/* eslint-disable */
import React from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Card, CardBody, Col } from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';

class AssignedWorkBook extends React.Component {
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
        key: 'role',
        name: 'Role',
        sortable: true,
        editable: false,
        cellClass: "text-left"
      },
      {
        key: 'completedTasks',
        name: 'Completed/ Total Tasks',
        sortable: true,
        editable: false,
        cellClass: "text-center"
      },
      {
        key: 'percentageCompleted',
        name: 'Percentage Completed',
        sortable: true,
        editable: false,
        cellClass: "text-center"
      },
      {
        key: 'dueDate',
        name: 'Due Date',
        sortable: true,
        editable: false,
        cellClass: "text-center"
      },
    ];

    this.employees = [];

    this.state = {
      modal: this.props.modal,      
      rows: this.createRows(this.props.assignedWorkBooks),
      pageOfItems: [],
    };
    this.toggle = this.toggle.bind(this);
  }

  createRows = (employees) => {
    const rows = [], 
          length = employees ? employees.length : 0;
    for (let i = 0; i < length; i++) { 
        let dueDate = employees[i].DueDate.split("T")[0];
      rows.push({
        employee: employees[i].EmployeeName,
        role: employees[i].Role,
        completedTasks: employees[i].CompletedTasks,
        percentageCompleted: employees[i].PercentageCompleted,
        dueDate: dueDate
      });
    }

    return rows;
  };

  componentWillReceiveProps(newProps) {
    if(this.state.modal != newProps.modal){
      let rows = this.createRows(newProps.assignedWorkBooks);
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
    this.props.updateState("level3WB");
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

export default AssignedWorkBook;