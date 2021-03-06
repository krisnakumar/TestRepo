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
import WorkBookRepetition from './WorkBookRepetition';
import * as API from '../../../shared/utils/APIUtils';
import * as Constants from '../../../shared/constants';
import Export from './WorkBookDashboardExport';
import SessionPopup from './SessionPopup';
import _ from "lodash";

// Import React Table
import ReactTable from "react-table";
import "react-table/react-table.css";
/**
 * WorkBookProgressEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-table module.
 */

class WorkBookProgress extends React.Component {

  constructor(props) {
    super(props);
    this.heads = [
      {
        key: 'taskCode',
        name: 'Task Code',
        sortable: true,
        width: 200,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'taskName',
        name: 'Task Name',
        width: 300,
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'completedTasksCount',
        name: 'Completed / Total Repetitions',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.workbookFormatter("completedTasksCount", props),
        cellClass: "text-center"
      },
      {
        key: 'incompletedTasksCount',
        name: 'Incomplete Repetitions',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.workbookIncompleteFormatter("incompletedTasksCount", props),
        cellClass: "text-center"
      },
      {
        key: 'completionPrecentage',
        name: 'Percentage Completed',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-center last-column"
      }
    ];

    this.state = {
      modal: this.props.modal,
      selectedWorkbook: this.props.selectedWorkbook || {},
      rows: this.createRows(this.props.workBooksProgress, this.props.selectedWorkbook.userId, this.props.selectedWorkbook.workBookId),
      pageOfItems: [],
      isWorkBookRepetitionModal: false,
      workBooksRepetition: {},
      isInitial: false,
      averageCompletionPrecentage: 0,
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
  * @name - createRows
  * This method will format the input data
  * for Data Grid
  * @param workbooks
  * @returns rows
  */
  createRows = (workbooks, userId, workBookId) => {
    const rows = [],
      length = workbooks ? workbooks.length : 0;
    let averageCompletionPrecentage = 0;
    // userId = this.state.selectedWorkbook ? (this.state.selectedWorkbook.userId|| 0) : 0,
    // workBookId = this.state.selectedWorkbook ? (this.state.selectedWorkbook.workBookId || 0) : 0;
    for (let i = 0; i < length; i++) {
      rows.push({
        userId: userId,
        workBookId: workBookId,
        taskId: workbooks[i].TaskId,
        taskCode: workbooks[i].TaskCode,
        taskName: workbooks[i].TaskName,
        completedTasksCount: workbooks[i].RepsCompleted + "/" + workbooks[i].RepsRequired,
        incompletedTasksCount: parseInt(workbooks[i].RepsRequired) - parseInt(workbooks[i].RepsCompleted),
        completionPrecentage: parseInt((workbooks[i].RepsCompleted / workbooks[i].RepsRequired * 100)) + "%"
      });
      averageCompletionPrecentage = parseInt(averageCompletionPrecentage + (workbooks[i].RepsCompleted / workbooks[i].RepsRequired * 100));
    }
    averageCompletionPrecentage = parseInt(averageCompletionPrecentage / length);

    //if (length > 0)
    //rows.push({ taskCode: "OQ Task Completion Percentage", taskName: "", completedTasksCount: "", incompletedTasksCount: "", completionPrecentage: averageCompletionPrecentage + "%" });
    this.setState({ averageCompletionPrecentage: averageCompletionPrecentage });
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
    let rows = this.createRows(newProps.workBooksProgress, newProps.selectedWorkbook.userId, newProps.selectedWorkbook.workBookId),
      isArray = Array.isArray(newProps.workBooksProgress),
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
   * @name - getWorkbookRepetitions
   * This method will used to get Workbook Repetitions details
   * @param userId
   * @param workBookId
   * @param taskId
   * @returns none
   */
  async getWorkbookRepetitions(userId, workBookId, taskId, status) {
    let { dashboardAPIToken } = sessionStorage || '{}';
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    let companyId = contractorManagementDetails.Company.Id || 0;

    const payLoad = {
      "Fields": [
        { "Name": "USER_ID", "Value": userId, "Operator": "=" },
        { 'Name': "STATUS", "Value": status, "Operator": "=", 'Bitwise': "AND" },
        { "Name": "WORKBOOK_ID", "Value": workBookId, "Operator": "=", "Bitwise": "AND" },
        { "Name": "TASK_ID", "Value": taskId, "Operator": "=", "Bitwise": "AND" }
      ],
      "ColumnList": Constants.GET_WORKBOOKS_REPETITION_COLUMNS,
      "AppType": "WORKBOOK_DASHBOARD"
    };

    let isWorkBookRepetitionModal = true,
      workBooksRepetition = {};
    this.setState({ isWorkBookRepetitionModal, workBooksRepetition });

    let token = idToken,
      url = "/company/" + companyId + "/workbooks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

    if (response == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if (response == 'API_ERROR') {
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {
      workBooksRepetition = response;
      isWorkBookRepetitionModal = true;
      this.setState({ ...this.state, isWorkBookRepetitionModal, workBooksRepetition });
    }
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
    this.props.updateState("isWorkBookProgressModal");
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
      workBookId = 0,
      taskId = 0,
    status = type == "incompletedTasksCount" ? "FAILED" : "COMPLETED";

    this.state.selectedWorkbook.taskCode = args.taskCode;
    this.state.selectedWorkbook.taskName = args.taskName;
    switch (type) {
      case "incompletedTasksCount":
      case "completedTasksCount":
        userId = args.userId;
        workBookId = args.workBookId;
        taskId = args.taskId;
        if (userId && workBookId && taskId)
          this.getWorkbookRepetitions(userId, workBookId, taskId, status);
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
   * @name - workbookIncompleteFormatter
   * This method will format the workbooks column Data Grid
   * @param type
   * @param props
   * @returns none
   */
  workbookIncompleteFormatter = (type, props) => {
    if (props.dependentValues.employee == "Total" || props.dependentValues.incompletedTasksCount == 0) {
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
  };

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
  };

  customCell(props) {
    let self = this;
    return (
      props.value && <span onClick={e => { e.preventDefault(); self.handleCellClick(props.column.id, props.original); }} className={"text-clickable"}>
        {props.value}
      </span> || <span>{props.value}</span>
    );
  }

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
        <WorkBookRepetition
          backdropClassName={"no-backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isWorkBookRepetitionModal}
          workBooksRepetition={this.state.workBooksRepetition}
          selectedWorkbook={this.state.selectedWorkbook}
        />
        <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid grid-modal-popup">
          <ModalHeader toggle={this.toggle}>Total Tasks and Completed Percentage</ModalHeader>
          <ModalBody className={""}>
            <div>
              <div className="export-menu-one">
              </div>
              <div className="export-menu-two">
                <Export
                  data={this.state.rows}
                  heads={this.heads}
                  sheetName={"Total Tasks and Completed Percentage"}
                />
              </div>
            </div>
            <div className="grid-description">
              <h5 className="pad-bt-10">View the workbook completion percentage and the total number of tasks in each workbook</h5>
              <h5 className="pad-bt-10">{this.state.selectedWorkbook ? this.state.selectedWorkbook.workbookName : ""} | {this.state.selectedWorkbook ? this.state.selectedWorkbook.percentageCompleted : ""}</h5>
              <h5 className="pad-bt-10">{this.state.selectedWorkbook ? this.state.selectedWorkbook.employee : ""}, {this.state.selectedWorkbook ? this.state.selectedWorkbook.role : ""}</h5>
            </div>
            <div className="grid-container">
              <div className="table has-total-row">
                <ReactTable
                  minRows={1}
                  data={rows}
                  columns={[
                    {
                      Header: "Task Code",
                      accessor: "taskCode",
                      headerClassName: 'header-wordwrap',
                      minWidth: 200,
                      className: 'text-left',
                      Cell: this.employeeFormatter,
                      Footer: (
                        <span>
                          <strong>OQ Task Completion Percentage</strong>
                        </span>
                      )
                    },
                    {
                      Header: "Task Name",
                      accessor: "taskName",
                      headerClassName: 'header-wordwrap',
                      minWidth: 350,
                      className: 'text-left',
                      // Cell: props => <span><span className="tooltip-div">{props.value}</span><span className='tooltip-text'>{props.value}</span></span>
                    },
                    {
                      Header: "Completed / Total Repetitions",
                      id: "completedTasksCount",
                      accessor: d => d.completedTasksCount,
                      headerClassName: 'header-wordwrap',
                      minWidth: 150,
                      className: 'text-center',
                      Cell: this.customCell
                    },
                    {
                      Header: "Incomplete Repetitions",
                      id: "incompletedTasksCount",
                      accessor: "incompletedTasksCount",
                      headerClassName: 'header-wordwrap',
                      minWidth: 150,
                      className: 'text-center'
                    },
                    {
                      Header: "Percentage Completed",
                      id: "completionPrecentage",
                      accessor: "completionPrecentage",
                      headerClassName: 'header-wordwrap',
                      minWidth: 120,
                      className: 'text-center',
                      Footer: (
                        <span>
                          <strong>
                            {
                              //_.sumBy(_.values(rows), 'completionPrecentage')
                              this.state.averageCompletionPrecentage ? this.state.averageCompletionPrecentage + "%" : "0%"
                            }
                          </strong>
                        </span>
                      )
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

export default WorkBookProgress;