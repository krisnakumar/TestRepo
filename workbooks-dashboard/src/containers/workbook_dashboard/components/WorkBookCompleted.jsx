/* eslint-disable */
/*
* WorkBookCompleted.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details to list the workbooks completed 
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------
createRows(employees)
toggle()
handleGridRowsUpdated(fromRow, toRow, updated)
handleGridSort(sortColumn, sortDirection)
updateModalState(modelName)
*/
import React from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Card, CardBody, Col } from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';
import * as API from '../../../shared/utils/APIUtils';
import * as Constants from '../../../shared/constants';
import WorkBookProgress from './WorkBookProgress';
import Export from './WorkBookDashboardExport';
import SessionPopup from './SessionPopup';
import _ from "lodash";

// Import React Table
import ReactTable from "react-table";
import "react-table/react-table.css";
/**
 * WorkBookCompletedEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-table module.
 */
class WorkBookCompletedEmptyRowsView extends React.Component {
  render() {
    return (<div className="no-records-found-modal">Sorry, no records</div>)
  }
};

class WorkBookCompleted extends React.Component {
  constructor(props) {
    super(props);

    this.heads = [
      {
        key: 'employee',
        name: 'Employee',
        sortable: true,
        width: 180,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'role',
        width: 200,
        name: 'Role',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'workbookName',
        name: 'Workbook',
        width: 550,
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'completedTasks',
        name: 'Completed / Total Repetitions',
        width: 180,
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.workbookFormatter("completedTasks", props),
        cellClass: "text-center"
      },
      {
        key: 'completionDate',
        name: 'Completion Date',
        sortable: true,
        width: 150,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-center last-column"
      },
    ];

    this.employees = [];

    this.state = {
      modal: this.props.modal,
      rows: this.createRows(this.props.assignedWorkBooks),
      pageOfItems: [],
      isInitial: false,
      isWorkBookProgressModal: false,
      workBooksProgress: {},
      selectedWorkbook: {},
      isSessionPopup: false,
      sessionPopupType: "API"
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
   * @param employees
   * @returns rows
   */
  createRows = (employees) => {
    const rows = [],
      length = employees ? employees.length : 0;
    for (let i = 0; i < length; i++) {
      rows.push({
        userId: employees[i].UserId,
        workBookId: employees[i].WorkBookId,
        employee: employees[i].EmployeeName + " (" + employees[i].UserName + " | " + employees[i].UserId + ")",
        role: employees[i].Role,
        workbookName: employees[i].WorkBookName,
        completedTasks: employees[i].RepsCompleted + "/" + employees[i].RepsRequired,
        completionDate: employees[i].DueDate ? (employees[i].DueDate.split("T")[0] || "") : ""
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
    let rows = this.createRows(newProps.assignedWorkBooks),
      isArray = Array.isArray(newProps.assignedWorkBooks),
      isInitial = isArray;
    this.setState({
      modal: newProps.modal,
      rows: rows,
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
    this.setState({
      modal: !this.state.modal
    });
    this.props.updateState("isCompletedModal");
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
   * @name - handleCellClick
   * This method will trigger the event of API's respective to cell clicked Data Grid
   * @param type
   * @param args
   * @returns none
   */
  handleCellClick = (type, args) => {
    let userId = 0,
      workBookId = 0;
    this.state.selectedWorkbook = args;
    switch (type) {
      case "percentageCompleted":
      case "completedTasks":
        userId = args.userId;
        workBookId = args.workBookId;
        if (userId && workBookId)
          this.getWorkBookProgress(userId, workBookId);
        break;
      default:
        break;
    }
    // this.refs.reactDataGrid.deselect();
  };

  /**
    * @method
    * @name - getWorkBookProgress
    * This method will used to get workbook progress details
    * @param userId
    * @param workBookId
    * @returns none
  */
  async getWorkBookProgress(userId, workBookId) {
    const { cookies } = this.props;
    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";
    const payLoad = {
      "Fields": [{ "Name": "USER_ID", "Value": userId, "Operator": "=" }, { "Name": "WORKBOOK_ID", "Value": workBookId, "Operator": "=", "Bitwise": "and" }],
      "ColumnList": Constants.GET_WORKBOOKS_PROGRESS_COLUMNS,
      "AppType": "WORKBOOK_DASHBOARD"
    };

    // Company Id get from session storage
    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let companyId = contractorManagementDetails.Company.Id || 0;

    let isWorkBookProgressModal = this.state.isWorkBookProgressModal,
      workBooksProgress = {};
    isWorkBookProgressModal = true;
    this.setState({ isWorkBookProgressModal, workBooksProgress });

    let token = idToken,
      url = "/company/" + companyId + "/workbooks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

    if (response == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if (response == 'API_ERROR') {
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {
      workBooksProgress = response;
      isWorkBookProgressModal = true;
      this.setState({ ...this.state, isWorkBookProgressModal, workBooksProgress });
    }
  };

  // This method is used to setting the row data in react data grid
  rowGetter = i => this.state.rows[i];

  render() {
    const { rows } = this.state;
    let pgSize = (rows.length > 10) ? rows.length : 10;
    return (
      <div>
        <SessionPopup
          backdropClassName={"backdrop"}
          modal={this.state.isSessionPopup}
          sessionPopupType={this.state.sessionPopupType}
        />
        <WorkBookProgress
          backdropClassName={"no-backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isWorkBookProgressModal}
          workBooksProgress={this.state.workBooksProgress}
          selectedWorkbook={this.state.selectedWorkbook}
        />
        <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid">
          <ModalHeader toggle={this.toggle}>Workbook Completed</ModalHeader>
          <Export
            data={this.state.rows}
            heads={this.heads}
            sheetName={"Workbook Completed"}
          />
          <ModalBody>
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
                      emptyRowsView={this.state.isInitial && WorkBookCompletedEmptyRowsView} 
                  /> */}
                <ReactTable
                  data={rows}
                  columns={[
                    {
                      Header: "Employee",
                      accessor: "employee",
                      headerClassName: 'header-wordwrap',
                      minWidth: 250,
                      className: 'text-left'
                    },
                    {
                      Header: "Role",
                      accessor: "role",
                      headerClassName: 'header-wordwrap',
                      minWidth: 200,
                      className: 'text-left'
                    },
                    {
                      Header: "Workbook",
                      id: "workbookName",
                      accessor: d => d.workbookName,
                      headerClassName: 'header-wordwrap',
                      minWidth: 350,
                      className: 'text-left'
                    },
                    {
                      Header: "Completed / Total Repetitions",
                      id: "completedTasks",
                      accessor: "completedTasks",
                      headerClassName: 'header-wordwrap',
                      minWidth: 120,
                      maxWidth: 200,
                      className: 'text-center',
                      Cell: this.customCell
                    },
                    {
                      Header: "Completion Date",
                      accessor: "completionDate",
                      headerClassName: 'header-wordwrap',
                      minWidth: 120,
                      maxWidth: 200,
                      className: 'text-center'
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
                />
              </div>
            </div>
          </ModalBody>
        </Modal>
      </div>
    );
  }
}

export default WorkBookCompleted;