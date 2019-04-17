/* eslint-disable */
/*
* CompanyUserDetail.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Companies task details 
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
import * as API from '../../../shared/utils/APIUtils';
import UserTaskDetail from './UserTaskDetail';
import SessionPopup from './SessionPopup';
import Export from './CTDashboardExport';

// Import React Table
import ReactTable from "react-table";
import "react-table/react-table.css";

/**
 * CompanyUserDetailEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react data grid module.
 */
class CompanyUserDetailEmptyRowsView extends React.Component {
  render() {
    return (<div className="no-records-found-modal">Sorry, no records</div>)
  }
};

class CompanyUserDetail extends React.Component {

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
        width: 500,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'incomplete',
        name: 'Incomplete',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.cellClickFormatter("incomplete", props),
        cellClass: "text-center"
      },
      {
        key: 'completed',
        name: 'Completed',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.cellClickFormatter("completed", props),
        cellClass: "text-center"
      },
      {
        key: 'total',
        name: 'Total',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.cellClickFormatter("total", props),
        cellClass: "text-center"
      },
      {
        key: 'percentageCompleted',
        name: '% Complete',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-center last-column"
      },
    ];

    this.employees = [];

    this.state = {
      modal: this.props.modal,
      rows: this.createRows(this.props.userDetails || {}),
      isInitial: false,
      title: this.props.title || "",
      isTaskDetailsModal: false,
      taskDetails: {},
      selectedEmployee: "",
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
  * @param companyTasks
  * @returns rows
  */
  createRows = (companyTasks) => {
    const rows = [],
      length = companyTasks ? companyTasks.length : 0;
    for (let i = 0; i < length; i++) {
      let incompleteQualification = companyTasks[i].IncompleteUserQualification || 0;
      let completedQualification = companyTasks[i].CompletedUserQualification || 0;
      let percentageCompleted = ((completedQualification / (incompleteQualification + completedQualification) * 100));
      percentageCompleted = percentageCompleted == NaN ? 0 : percentageCompleted;
      rows.push({
        roleId: companyTasks[i].RoleId || 0,
        userId: companyTasks[i].UserId || 0,
        companyId: companyTasks[i].CompanyId || 0,
        employee: companyTasks[i].EmployeeName + " (" + companyTasks[i].UserName + " | " + companyTasks[i].UserId + ")" || "",
        incomplete: incompleteQualification,
        completed: completedQualification,
        total: (incompleteQualification + completedQualification) || 0,
        percentageCompleted: Math.round(percentageCompleted) + "%" || "0%"
      });
    }

    return rows;
  };

  /**
   * @method
   * @name - componentWillReceiveProps
   * This method will be invoked whenever the props or state
   * is update to this component class
   * @param newProps
   * @returns none
   */
  componentWillReceiveProps(newProps) {
    let rows = this.createRows(newProps.userDetails),
      isArray = Array.isArray(newProps.userDetails),
      isInitial = isArray;

    if (rows) {
      this.state.modal = newProps.modal;
      this.state.rows = rows;
      this.state.isInitial = isInitial;
      this.state.title = newProps.title || "";
      this.handleGridSort("employee", "ASC");
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
    this.props.updateState("isUserDetailsModal");
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
  * @name - getUserTaskDetails
  * This method will be used to get Companies User details
  * @param company
  * @param companyId
  * @returns none
  */
  async getUserTaskDetails(employeeName, contractorCompanyId, userId, isCompleted, roleId) {
    const { cookies } = this.props;
    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    // get the company Id from the session storage 
    let companyId = contractorManagementDetails.Company.Id || 0;
    let adminId = parseInt(contractorManagementDetails.User.Id) || 0;

    let fields = [{ "Name": "USER_ID", "Value": userId, "Operator": "=", "Bitwise": "and" }, { "Name": "CONTRACTOR_COMPANY", "Value": contractorCompanyId, "Operator": "=", "Bitwise": "and" }, { "Name": "ROLE_ID", "Value": roleId, "Operator": "=", "Bitwise": "and" }, { "Name": "ADMIN_ID", "Value": adminId, "Operator": "=", "Bitwise": "and" }];

    if (isCompleted) {
      fields.push({ "Name": "STATUS", "Value": "COMPLETED", "Operator": "=", "Bitwise": "and" });
    } else {
      fields.push({ "Name": "STATUS", "Value": "IN_COMPLETE", "Operator": "=", "Bitwise": "and" });
    }
    if (isCompleted == null) {
      fields = [{ "Name": "USER_ID", "Value": userId, "Operator": "=", "Bitwise": "and" }, { "Name": "CONTRACTOR_COMPANY", "Value": contractorCompanyId, "Operator": "=", "Bitwise": "and" }, { "Name": "ROLE_ID", "Value": roleId, "Operator": "=", "Bitwise": "and" }, { "Name": "ADMIN_ID", "Value": adminId, "Operator": "=", "Bitwise": "and" }, { "Name": "STATUS", "Value": "ALL", "Operator": "=", "Bitwise": "and" }];
    }

    const postData = {
      "Fields": fields,
      "ColumnList": ['TASK_NAME', 'TASK_CODE', 'STATUS'],
      "AppType": "TRAINING_DASHBOARD"
    };

    let isTaskDetailsModal = this.state.isTaskDetailsModal,
      taskDetails = {},
      selectedEmployee = employeeName;
    isTaskDetailsModal = true;

    this.setState({ isTaskDetailsModal, taskDetails, selectedEmployee });
    let { dashboardAPIToken } = sessionStorage || {};
    dashboardAPIToken = JSON.parse(dashboardAPIToken);
    let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";
    let token = idToken,
      url = "/company/" + companyId + "/tasks",
      response = await API.ProcessAPI(url, postData, token, false, "POST", true);

    if (response == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if (response == 'API_ERROR') {
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {
      taskDetails = response;
      isTaskDetailsModal = true;
      this.setState({ ...this.state, isTaskDetailsModal, taskDetails, selectedEmployee });
    }
  };

  /**
   * @method
   * @name - handleCellClick
   * This method will trigger the event of APIs respective to cell clicked Data Grid
   * @param type
   * @param args
   * @returns none
   */
  handleCellClick = (type, args) => {
    let userId = args.userId || 0,
      companyId = args.companyId || 0,
      roleId = args.roleId || 0,
      employeeName = this.state.title ? (this.state.title + " - " + args.employee) : args.employee,
      isCompleted = type == "completed";
    switch (type) {
      case "incomplete":
      case "completed":
        this.getUserTaskDetails(employeeName, companyId, userId, isCompleted, roleId);
        break;
      case "total":
      case "percentageCompleted":
        this.getUserTaskDetails(employeeName, companyId, userId, null, roleId);
        break;
      default:
        console.log(type, args);
        break;
    }
    // this.refs.companyUserDetailReactDataGrid.deselect();
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
   * @name - cellClickFormatter
   * This method will format the workbooks column Data Grid
   * @param type
   * @param props
   * @returns none
   */
  cellClickFormatter = (type, props) => {
    if (props.dependentValues[type] <= 0) {
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

  // This method is used to set the row data in react data grid
  rowGetter = i => this.state.rows[i];

  render() {
    const { rows, title } = this.state;
    let titleText = title || "";
    let pgSize = (rows.length > 10) ? rows.length : 10;
    return (
      <div>
        <SessionPopup
          backdropClassName={"backdrop"}
          modal={this.state.isSessionPopup}
          sessionPopupType={this.state.sessionPopupType}
        />
        <UserTaskDetail
          backdropClassName={"no-backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isTaskDetailsModal}
          taskDetails={this.state.taskDetails}
          title={this.state.selectedEmployee}
        />
        <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid">
          <ModalHeader className="text-left" toggle={this.toggle}>
            {titleText}
            <p className="section-info-description">This level will display the contractor's training progress required by the role</p>
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
                {/* <ReactDataGrid
                  ref={'companyUserDetailReactDataGrid'}
                  onGridSort={this.handleGridSort}
                  enableCellSelect={false}
                  enableCellAutoFocus={false}
                  columns={this.heads}
                  rowGetter={this.rowGetter}
                  rowsCount={rows.length}
                  onGridRowsUpdated={this.handleGridRowsUpdated}
                  rowHeight={35}
                  minColumnWidth={120}
                  emptyRowsView={this.state.isInitial && CompanyUserDetailEmptyRowsView}
                  sortColumn="employee"
                  sortDirection="ASC"
                /> */}
                <ReactTable
                  data={rows}
                  columns={[
                    {
                      Header: "Employee",
                      id: "employee",
                      accessor: "employee",
                      headerClassName: 'header-wordwrap',
                      minWidth: 300,
                      className: 'text-left'
                    },
                    {
                      Header: "Incomplete",
                      id: "incomplete",
                      accessor: d => d.incomplete,
                      headerClassName: 'header-wordwrap',
                      minWidth: 150,
                      maxWidth: 200,
                      className: 'text-center',
                      Cell: this.customCell
                    },
                    {
                      Header: "Completed",
                      id: "completed",
                      accessor: "completed",
                      headerClassName: 'header-wordwrap',
                      minWidth: 150,
                      maxWidth: 200,
                      className: 'text-center',
                      Cell: this.customCell
                    },
                    {
                      Header: "Total",
                      id: "total",
                      accessor: "total",
                      headerClassName: 'header-wordwrap',
                      minWidth: 150,
                      maxWidth: 200,
                      className: 'text-center',
                      Cell: this.customCell
                    },
                    {
                      Header: "% Complete",
                      id: "percentageCompleted",
                      accessor: "percentageCompleted",
                      headerClassName: 'header-wordwrap',
                      minWidth: 150,
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
                  pageSize={!this.state.isInitial ? 10 : pgSize}
                  loading={!this.state.isInitial}
                  loadingText={''}
                  noDataText={!this.state.isInitial ? '' : 'Sorry, no records'}
                  defaultSorted={[
                    {
                      id: "employee",
                      desc: false
                    }
                  ]}
                  style={{
                    minHeight: "400px", // This will force the table body to overflow and scroll, since there is not enough room
                    maxHeight: "800px"
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

export default withCookies(CompanyUserDetail);