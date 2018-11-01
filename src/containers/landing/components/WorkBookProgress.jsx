/* eslint-disable */
import React from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Card, CardBody, Col } from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';

class WorkBookProgressEmptyRowsView extends React.Component{
  render() {
    return (<div className="no-records-found-modal">Sorry, no records</div>)
  }
};

class WorkBookProgress extends React.Component {

  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor(props) {
    super(props);

    this.heads = [
      {
        key: 'taskCode',
        name: 'Task Code',
        sortable: true,
        width: 300,
        editable: false,
        cellClass: "text-left"
      },
      {
        key: 'taskName',
        name: 'OQ Task',
        sortable: true,
        editable: false,
        cellClass: "text-left"
      },
      {
        key: 'completedTasksCount',
        name: 'Completed / Total Tasks',
        sortable: true,
        editable: false,
        cellClass: "text-center"
      },
      {
        key: 'incompletedTasksCount',
        name: 'Incompleted Repetitions',
        sortable: true,
        editable: false,
        cellClass: "text-center last-column"
      },
      {
        key: 'completionPrecentage',
        name: 'Completion Precentage',
        sortable: true,
        editable: false,
        cellClass: "text-center last-column"
      },
    ];

    this.state = {
      modal: this.props.modal,      
      rows: this.createRows(this.props.workBooksProgress),
      pageOfItems: [],
    };
    this.toggle = this.toggle.bind(this);
  }

  componentDidCatch(error, info) {
    // Display fallback UI
    // this.setState({ hasError: true });
    // You can also log the error to an error reporting service
    console.log(error, info);
  }

  createRows = (workbooks) => {
    const rows = [], 
          length = workbooks ? workbooks.length : 0;
    for (let i = 0; i < length; i++) { 
      rows.push({
        userId:  workbooks[i].UserId,
        taskCode: workbooks[i].TaskCode,
        taskName: workbooks[i].TaskName,
        completedTasksCount: workbooks[i].CompletedTasksCount + "/" + workbooks[i].TotalTasks,
        incompletedTasksCount: workbooks[i].IncompletedTasksCount,
        completionPrecentage: workbooks[i].CompletionPrecentage + "%"
      });
    }

    return rows;
  };

  componentWillReceiveProps(newProps) {
    if(this.state.modal != newProps.modal){
      let rows = this.createRows(newProps.workBooksProgress);
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
    this.props.updateState("isWorkBookProgressModal");
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
        <Modal isOpen={this.state.modal}  fade={false}  toggle={this.toggle} centered={true} className="custom-modal-grid">
          <ModalHeader toggle={this.toggle}>Total Tasks and Completed Percentage</ModalHeader>
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
                      emptyRowsView={WorkBookProgressEmptyRowsView} 
                  />
              </div>
            </div>
          </ModalBody>
        </Modal>
      </div>
    );
  }
}

export default withCookies(WorkBookProgress);