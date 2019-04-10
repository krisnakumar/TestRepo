/* eslint-disable */
/*
* UserTaskDetail.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render User task details 
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
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import Export from './CTDashboardExport';

/**
 * UserTaskDetailEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react data grid module.
 */
class UserTaskDetailEmptyRowsView extends React.Component {
  render() {
    return (<div className="no-records-found-modal">Sorry, no records</div>)
  }
};

class UserTaskDetail extends React.Component {

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
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'taskName',
        name: 'Task Name',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'status',
        name: 'Status',
        width: 200,
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left last-column"
      }
    ];

    this.state = {
      modal: this.props.modal,
      rows: this.createRows(this.props.taskDetails || {}),
      isInitial: false,
      title: this.props.title || ""
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
  * @name - createRows
  * This method will format the input data
  * for Data Grid
  * @param taskDetails
  * @returns rows
  */
  createRows = (taskDetails) => {
    const rows = [],
      length = taskDetails ? taskDetails.length : 0;
    for (let i = 0; i < length; i++) {
      rows.push({
        taskCode: taskDetails[i].TaskCode || "",
        taskName: taskDetails[i].TaskName || "",
        status: taskDetails[i].Status || ""
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
    let rows = this.createRows(newProps.taskDetails),
      isArray = Array.isArray(newProps.taskDetails),
      isInitial = isArray;

    if (rows) {
      this.state.modal = newProps.modal;
      this.state.rows = rows;
      this.state.isInitial = isInitial;
      this.state.title = newProps.title || "";
      this.handleGridSort("taskName", "ASC");
    } else {
      this.setState({
        modal: newProps.modal,
        rows: rows,
        isInitial: isInitial,
        title: newProps.title || ""
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
    this.props.updateState("isTaskDetailsModal");
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
    let isPercentage = sortColumn.includes('percentage');

    const comparer = (a, b) => {
      if (sortDirection === 'ASC') {
        return (a[sortColumn] >= b[sortColumn]) ? 1 : -1;
      } else if (sortDirection === 'DESC') {
        return (a[sortColumn] <= b[sortColumn]) ? 1 : -1;
      }
    };

    const percentageComparer = (a, b) => {
      if (sortDirection === 'ASC') {
        return (parseInt(a[sortColumn]) >= parseInt(b[sortColumn])) ? 1 : -1;
      } else if (sortDirection === 'DESC') {
        return (parseInt(a[sortColumn]) <= parseInt(b[sortColumn])) ? 1 : -1;
      }
    };

    const sortRows = this.state.rows.slice(0),
      rowsLength = this.state.rows.length || 0;

    let rows = sortDirection === 'NONE' ? this.state.rows.slice(0, rowsLength) : sortRows.sort(comparer).slice(0, rowsLength);

    if (isPercentage)
      rows = sortDirection === 'NONE' ? this.state.rows.slice(0, rowsLength) : sortRows.sort(percentageComparer).slice(0, rowsLength);

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


  // This method is used to setting the row data in react data grid
  rowGetter = i => this.state.rows[i];

  render() {
    const { rows, title } = this.state;
    let titleText = title || "";
    return (
      <div>
        <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid">
          <ModalHeader className="text-left" toggle={this.toggle}>
            {titleText}
            <p className="section-info-description">This level will display the contractors training tasks and their respective status</p>
            <p className="section-info-description"> </p>
          </ModalHeader>
          <Export
            data={this.state.rows}
            heads={this.heads}
            sheetName={titleText}
          />
          <ModalBody>
            <div className="grid-container">
              <div className="table">
                <ReactDataGrid
                  ref={'userTaskDetailReactDataGrid'}
                  onGridSort={this.handleGridSort}
                  enableCellSelect={false}
                  enableCellAutoFocus={false}
                  columns={this.heads}
                  rowGetter={this.rowGetter}
                  rowsCount={rows.length}
                  onGridRowsUpdated={this.handleGridRowsUpdated}
                  rowHeight={35}
                  minColumnWidth={100}
                  emptyRowsView={this.state.isInitial && UserTaskDetailEmptyRowsView}
                  sortColumn="taskName"
                  sortDirection="ASC"
                />
              </div>
            </div>
          </ModalBody>
        </Modal>
      </div>
    );
  }
}

export default withCookies(UserTaskDetail);