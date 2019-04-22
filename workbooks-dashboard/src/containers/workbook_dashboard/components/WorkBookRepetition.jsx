/* eslint-disable */
/*
* WorkBookProgress.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details to list the workbooks progress details
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
import Export from './WorkBookDashboardExport';
import _ from "lodash";

// Import React Table
import ReactTable from "react-table";
import "react-table/react-table.css";

/**
 * WorkBookProgressEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the  react data grid module.
 */
class WorkBookRepetitionEmptyRowsView extends React.Component {
  render() {
    return (<div className="no-records-found-modal">Sorry, no records</div>)
  }
};

class WorkBookRepetition extends React.Component {
  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor(props) {
    super(props);
    this.heads = [
      {
        key: 'attempt',
        name: 'Attempt(s)',
        width: 100,
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'status',
        name: 'Complete/Incomplete',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'dateTime',
        name: 'Last Attempted Date',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-center"
      },
      {
        key: 'location',
        name: 'Location',
        sortable: true,
        width: 350,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-center"
      },
      {
        key: 'evaluator',
        name: 'Submitted By',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-center"
      },
      {
        key: 'comments',
        name: 'Comments',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-center last-column"
      }
    ];

    this.state = {
      modal: this.props.modal,
      rows: this.createRows(this.props.workBooksProgress),
      pageOfItems: [],
      isInitial: false,
      selectedWorkbook: this.props.selectedWorkbook
    };
    this.toggle = this.toggle.bind(this);
    this.customCell = this.customCell.bind(this);
  }

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
  createRows = (workbooks) => {
    const rows = [],
      length = workbooks ? workbooks.length : 0;
    for (let i = 0; i < length; i++) {
      rows.push({
        userId: workbooks[i].UserId || 0,
        attempt: workbooks[i].NumberofAttempts || 0,
        status: workbooks[i].Status,
        dateTime: workbooks[i].LastAttemptDate_tasks,
        location: workbooks[i].Location,
        evaluator: workbooks[i].EvaluatorName,
        comments: workbooks[i].Comments || ""
      });
    }

    return rows;
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
    let rows = this.createRows(newProps.workBooksRepetition),
      isArray = Array.isArray(newProps.workBooksRepetition),
      isInitial = isArray;
    this.setState({
      modal: newProps.modal,
      rows: rows,
      isInitial: isInitial,
      selectedWorkbook: newProps.selectedWorkbook
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
    this.setState({
      modal: !this.state.modal
    });
    this.props.updateState("isWorkBookRepetitionModal");
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
        return (a[sortColumn] >= b[sortColumn]) ? 1 : -1;
      } else if (sortDirection === 'DESC') {
        return (a[sortColumn] <= b[sortColumn]) ? 1 : -1;
      }
    };

    const sortRows = this.state.rows.slice(0),
      rowsLength = this.state.rows.length || 0;
    const rows = sortDirection === 'NONE' ? this.state.rows.slice(0, rowsLength) : sortRows.sort(comparer).slice(0, rowsLength);

    this.setState({ rows });
  };

  customCell(props) {
    let self = this;
    return (
      props.value && <span onClick={e => { e.preventDefault(); self.handleCellClick(props.column.id, props.original); }} className={"text-clickable"}>
        {props.value}
      </span> || <span>{props.value}</span>
    );
  }


  // This method is used to setting the row data in react data grid
  rowGetter = i => this.state.rows[i];

  render() {
    const { rows } = this.state;
    let pgSize = (rows.length > 10) ? rows.length : 10;
    return (
      <div>
        <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid grid-modal-popup">
          <ModalHeader toggle={this.toggle}>Workbook Repetition</ModalHeader>
          <div>
            <div className="export-menu-one">

            </div>
            <div className="export-menu-two">
              <Export
                data={this.state.rows}
                heads={this.heads}
                sheetName={"Workbook Repetition"}
              />
            </div>
          </div>
          <ModalBody className={""}>
            <div className="grid-description">
              <h5 className="pad-bt-10">{this.state.selectedWorkbook ? this.state.selectedWorkbook.workbookName : ""} | {this.state.selectedWorkbook ? this.state.selectedWorkbook.percentageCompleted : ""}</h5>
              <h6 className="pad-bt-10">{this.state.selectedWorkbook ? this.state.selectedWorkbook.taskCode : ""} {this.state.selectedWorkbook ? this.state.selectedWorkbook.taskName : ""}</h6>
              <h5 className="pad-bt-10">{this.state.selectedWorkbook ? this.state.selectedWorkbook.employee : ""}, {this.state.selectedWorkbook ? this.state.selectedWorkbook.role : ""}</h5>
            </div>
            <div className="grid-container">
              <div className="table">
                {/* <ReactDataGrid
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
                  emptyRowsView={this.state.isInitial && WorkBookRepetitionEmptyRowsView}
                /> */}
                <ReactTable
                  minRows={1}
                  data={rows}
                  columns={[
                    {
                      Header: "Attempt(s)",
                      accessor: "attempt",
                      headerClassName: 'header-wordwrap',
                      minWidth: 100,
                      className: 'text-center'
                    },
                    {
                      Header: "Complete/Incomplete",
                      accessor: "status",
                      headerClassName: 'header-wordwrap',
                      minWidth: 150,
                      className: 'text-left'
                    },
                    {
                      Header: "Last Attempted Date",
                      id: "dateTime",
                      accessor: d => d.dateTime,
                      headerClassName: 'header-wordwrap',
                      minWidth: 170,
                      maxWidth: 200,
                      className: 'text-center'
                    },
                    {
                      Header: "Location",
                      accessor: "location",
                      headerClassName: 'header-wordwrap',
                      minWidth: 250,
                      className: 'text-left'
                    },
                    {
                      Header: "Submitted By",
                      id: "evaluator",
                      accessor: d => d.evaluator,
                      headerClassName: 'header-wordwrap',
                      minWidth: 250,
                      maxWidth: 400,
                      className: 'text-left'
                    },
                    {
                      Header: "Comments",
                      accessor: "comments",
                      headerClassName: 'header-wordwrap',
                      minWidth: 250,
                      className: 'text-left'
                    }
                  ]
                  }
                  resizable={false}
                  className="-striped -highlight"
                  showPagination={false}
                  showPaginationTop={false}
                  showPaginationBottom={false}
                  showPageSizeOptions={false}
                  pageSizeOptions={[5, 10, 20, 25, 50, 100]}
                  pageSize={!this.state.isInitial ? 5 : pgSize}
                  loading={!this.state.isInitial}
                  loadingText={''}
                  noDataText={!this.state.isInitial ? '' : 'Sorry, no records'}
                  // defaultSorted={[
                  //   {
                  //     id: "role",
                  //     desc: false
                  //   }
                  // ]}
                  style={{
                    // minHeight: "575px", // This will force the table body to overflow and scroll, since there is not enough room
                    maxHeight: "550px"
                  }}
                />
              </div>
            </div>
          </ModalBody>
        </Modal>
      </div>
    );
  }
}

export default withCookies(WorkBookRepetition);