/* eslint-disable */
import React from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Card, CardBody, Col } from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';
import WorkBookProgress from './WorkBookProgress';
import Loader from '../../_layout/loader/Loader';
import * as API from '../../../shared/utils/APIUtils';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';

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
        key: 'role',
        name: 'Role',
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

  componentDidCatch(error, info) {
    // Display fallback UI
    // this.setState({ hasError: true });
    // You can also log the error to an error reporting service
    console.log(error, info);
  }

  async getWorkBookProgress(userId, workBookId){
    const { cookies } = this.props;

    let token = cookies.get('IdentityToken'),
        url = "https://omwlc1qx62.execute-api.us-west-2.amazonaws.com/dev/users/"+ userId +"/assigned-workbooks/"+ workBookId +"/tasks",
        response = await API.ProcessAPI(url, "", token, false, "GET", true),
        workBooksProgress = response,
        isWorkBookProgressModal = this.state.isWorkBookProgressModal;

        isWorkBookProgressModal = true;
    this.setState({ ...this.state, isWorkBookProgressModal, workBooksProgress });
  };

  createRows = (employees) => {
    const rows = [], 
          length = employees ? employees.length : 0;
    for (let i = 0; i < length; i++) { 
        let dueDate = employees[i].DueDate.split("T")[0];
      rows.push({
        userId:  employees[i].UserId,
        workBookId: employees[i].WorkBookId,
        employee: employees[i].EmployeeName,
        role: employees[i].Role,
        completedTasks: employees[i].CompletedTasks,
        percentageCompleted: employees[i].PercentageCompleted + "%",
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

  updateModalState = (modelName) => {
    let value = !this.state[modelName];
    this.setState({
      [modelName]: value
    });
  };

  toggle() {
    this.setState({
      modal: !this.state.modal
    });
    this.props.updateState("isAssignedModal");
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

  handleCellFocus = (args) => {
    if(args.idx == 2 || args.idx == 3){
      let userId = this.state.rows[args.rowIdx].userId,
          workBookId = this.state.rows[args.rowIdx].workBookId;

      if(userId && workBookId)
        this.getWorkBookProgress(userId, workBookId);      
    }
    this.refs.reactDataGrid.deselect();
  };
  
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