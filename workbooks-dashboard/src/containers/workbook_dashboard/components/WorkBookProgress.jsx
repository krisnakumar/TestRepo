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

/**
 * WorkBookProgressEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-table module.
 */
class WorkBookProgressEmptyRowsView extends React.Component {
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
      rows: this.createRows(this.props.workBooksProgress),
      pageOfItems: [],
      isWorkBookRepetitionModal: false,
      workBooksRepetition: {},
      isInitial: false,
      selectedWorkbook: this.props.selectedWorkbook || {}
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
  * @param workbooks
  * @returns rows
  */
  createRows = (workbooks) => {
    const rows = [],
      length = workbooks ? workbooks.length : 0;
    let averageCompletionPrecentage = 0;
    for (let i = 0; i < length; i++) {
      rows.push({
        userId: workbooks[i].UserId,
        workBookId: workbooks[i].WorkbookId,
        taskId: workbooks[i].TaskId,
        taskCode: workbooks[i].TaskCode,
        taskName: workbooks[i].TaskName,
        completedTasksCount: workbooks[i].CompletedTasksCount + "/" + workbooks[i].TotalTasks,
        incompletedTasksCount: workbooks[i].IncompletedTasksCount,
        completionPrecentage: parseInt((workbooks[i].CompletedTasksCount / workbooks[i].TotalTasks * 100)) + "%"
      });
      averageCompletionPrecentage = parseInt(averageCompletionPrecentage + (workbooks[i].CompletedTasksCount / workbooks[i].TotalTasks * 100));
    }
    averageCompletionPrecentage = parseInt(averageCompletionPrecentage / length);

    if (length > 0)
      rows.push({ taskCode: "OQ Task Completion Percentage", taskName: "", completedTasksCount: "", incompletedTasksCount: "", completionPrecentage: averageCompletionPrecentage + "%" });

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
    let rows = this.createRows(newProps.workBooksProgress),
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
    const { cookies } = this.props;
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
      "ColumnList": Constants.GET_WORKBOOKS_REPETITION_COLUMNS
    };

    let isWorkBookRepetitionModal = this.state.isWorkBookRepetitionModal,
      workBooksRepetition = {};
    isWorkBookRepetitionModal = true;
    this.setState({ isWorkBookRepetitionModal, workBooksRepetition });

    let token = idToken,//cookies.get('IdentityToken'),
      //companyId = cookies.get('CompanyId'),
      url = "/company/" + companyId + "/tasks",
      response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

    workBooksRepetition = response;
    isWorkBookRepetitionModal = true;
    this.setState({ ...this.state, isWorkBookRepetitionModal, workBooksRepetition });
    window.dispatchEvent(new Event('resize'));
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
    let isPercentage = sortColumn.includes('Precentage');

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

    const beforePopRows = this.state.rows;
    let totalRow = "";
    if (beforePopRows.length > 0) {
      totalRow = beforePopRows.pop();
    }

    const sortRows = beforePopRows.slice(0),
      rowsLength = this.state.rows.length || 0;

    let rows = sortDirection === 'NONE' ? this.state.rows.slice(0, rowsLength) : sortRows.sort(comparer).slice(0, rowsLength);

    if (isPercentage)
      rows = sortDirection === 'NONE' ? this.state.rows.slice(0, rowsLength) : sortRows.sort(percentageComparer).slice(0, rowsLength);

    if (beforePopRows.length > 0)
      rows.push(totalRow);

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
      workBookId = 0,
      taskId = 0;
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
    this.refs.reactDataGrid.deselect();
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

  // This method is used to setting the row data in react data grid
  rowGetter = i => this.state.rows[i];

  render() {
    const { rows } = this.state;
    return (
      <div>
        <WorkBookRepetition
          backdropClassName={"no-backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isWorkBookRepetitionModal}
          workBooksRepetition={this.state.workBooksRepetition}
          selectedWorkbook={this.state.selectedWorkbook}
        />
        <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid">
          <ModalHeader toggle={this.toggle}>Total Tasks and Completed Percentage</ModalHeader>
          <Export
            data={this.state.rows}
            heads={this.heads}
            sheetName={"Total Tasks and Completed Percentage"}
          />
          <ModalBody>
            <div className="grid-description">
              <h5 className="pad-bt-10">{this.state.selectedWorkbook ? this.state.selectedWorkbook.workbookName : ""} | {this.state.selectedWorkbook ? this.state.selectedWorkbook.percentageCompleted : ""}</h5>
              <h5 className="pad-bt-10">{this.state.selectedWorkbook ? this.state.selectedWorkbook.employee : ""}, {this.state.selectedWorkbook ? this.state.selectedWorkbook.role : ""}</h5>
            </div>
            <div className="grid-container">
              <div className="table has-total-row">
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
                  emptyRowsView={this.state.isInitial && WorkBookProgressEmptyRowsView}
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