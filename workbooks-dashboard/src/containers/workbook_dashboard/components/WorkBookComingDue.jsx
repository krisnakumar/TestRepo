
/* eslint-disable */
/*
* WorkBookComingDue.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details to list the workbooks coming due 
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
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import WorkBookProgress from './WorkBookProgress';
import * as API from '../../../shared/utils/APIUtils';
import * as Constants from '../../../shared/constants';
import Export from './WorkBookDashboardExport';
import SessionPopup from './SessionPopup';
import _ from "lodash";

// Import React Table
import ReactTable from "react-table";
import "react-table/react-table.css";

/**
 * WorkBookComingDueEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-data-grid module.
 */
class WorkBookComingDueEmptyRowsView extends React.Component {
  render() {
    return (<div className="no-records-found-modal">Sorry, no records</div>)
  }
};

class WorkBookComingDue extends React.Component {

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
        width: 180,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'role',
        name: 'Role',
        sortable: true,
        width: 150,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'workbookName',
        width: 450,
        name: 'Workbook',
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
        key: 'percentageCompleted',
        name: 'Percentage Completed',
        sortable: true,
        width: 180,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.workbookFormatter("percentageCompleted", props),
        cellClass: "text-center"
      },
      {
        key: 'dueDate',
        name: 'Due Date',
        sortable: true,
        width: 180,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-center last-column"
      }
    ];

    this.employees = [];

    this.state = {
      modal: this.props.modal,
      rows: this.createRows(this.props.assignedWorkBooks),
      pageOfItems: [],
      isWorkBookProgressModal: false,
      workBooksProgress: {},
      isInitial: false,
      selectedWorkbook: {},
      isSessionPopup: false,
      sessionPopupType: "API"
    };
    this.toggle = this.toggle.bind(this);
    this.customCell = this.customCell.bind(this);
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
      let dueDate = employees[i].DueDate ? employees[i].DueDate.split("T")[0] : "";
      rows.push({
        userId: employees[i].UserId,
        workBookId: employees[i].WorkBookId,
        employee: employees[i].EmployeeName + " (" + employees[i].UserName + " | " + employees[i].UserId + ")",
        role: employees[i].Role,
        completedTasks: employees[i].RepsCompleted + "/" + employees[i].RepsRequired,
        workbookName: employees[i].WorkBookName,
        percentageCompleted: Math.round(parseInt(employees[i].RepsCompleted) / parseInt(employees[i].RepsRequired) * 100) + "%",
        dueDate: dueDate
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
    this.props.updateState("isComingDueModal");
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
   * @name - workbookFormatter
   * This method will format the workbooks column Data Grid
   * @param type
   * @param props
   * @returns none
   */
  workbookFormatter = (type, props) => {
    if (props.dependentValues.employee == "Total") {
      return (
        <span>{props.value}</span>
      );
    } else {
      return (
        <span onClick={e => { e.preventDefault(); this.handleCellClick(type, props.dependentValues); }} className={"text-clickable"}>
          {props.value}
        </span>
      );
    }
  }

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
          <ModalHeader toggle={this.toggle}>WorkBook Due in 30 Days</ModalHeader>
          <Export
            data={this.state.rows}
            heads={this.heads}
            sheetName={"WorkBook Due in 30 Days"}
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
                      emptyRowsView={this.state.isInitial && WorkBookComingDueEmptyRowsView} 
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
                      minWidth: 300,
                      className: 'text-left'
                    },
                    {
                      Header: "Completed / Total Repetitions",
                      id: "completedTasks",
                      accessor: "completedTasks",
                      headerClassName: 'header-wordwrap',
                      minWidth: 125,
                      maxWidth: 200,
                      className: 'text-center',
                      Cell: this.customCell
                    },
                    {
                      Header: "Percentage Completed",
                      accessor: "percentageCompleted",
                      headerClassName: 'header-wordwrap',
                      minWidth: 125,
                      className: 'text-center'
                    },
                    {
                      Header: "Due Date",
                      accessor: "dueDate",
                      headerClassName: 'header-wordwrap',
                      minWidth: 125,
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
                  style={{
                    minHeight: "575px", // This will force the table body to overflow and scroll, since there is not enough room
                    maxHeight: "575px"
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

export default withCookies(WorkBookComingDue);