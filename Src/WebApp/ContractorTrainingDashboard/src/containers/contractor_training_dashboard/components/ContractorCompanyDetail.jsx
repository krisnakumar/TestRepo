/* eslint-disable */
/*
* ContractorCompanyDetail.jsx
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
import { instanceOf, PropTypes } from 'prop-types';
import * as API from '../../../shared/utils/APIUtils';
import CompanyUserDetail from './CompanyUserDetail';
import SessionPopup from './SessionPopup';
import Export from './CTDashboardExport';

// Import React Table
import ReactTable from "react-table";
import "react-table/react-table.css";

/**
 * ContractorCompanyDetailEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react data grid module.
 */

class ContractorCompanyDetail extends React.Component {

  constructor(props) {
    super(props);

    this.heads = [
      {
        key: 'company',
        name: 'Company',
        sortable: true,
        width: 500,
        editable: false,
        cellClass: "text-left"
      },
      {
        key: 'incompleteUsers',
        name: 'Incomplete Users',
        sortable: true,
        editable: false,
        cellClass: "text-center"
      },
      {
        key: 'completedUsers',
        name: 'Completed Users',
        sortable: true,
        editable: false,
        cellClass: "text-center"
      },
      {
        key: 'total',
        name: 'Total',
        sortable: true,
        editable: false,
        cellClass: "text-center"
      },
      {
        key: 'percentageCompleted',
        name: 'Percentage Completed',
        sortable: true,
        editable: false,
        cellClass: "text-center last-column"
      },
    ];

    this.employees = [];

    this.state = {
      modal: this.props.modal,
      rows: this.createRows(this.props.companyDetails || {}),
      isInitial: false,
      title: this.props.title || "",
      selectedCompany: "",
      isUserDetailsModal: false,
      userDetails: {},
      isSessionPopup: false,
      sessionPopupType: "API"
    };
    this._tBodyComponent = null;
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
      let incompleteQualification = companyTasks[i].InCompletedCompanyQualification || 0;
      let completedQualification = companyTasks[i].CompletedCompanyQualification || 0;
      let percentageCompleted = ((completedQualification / (incompleteQualification + completedQualification) * 100));
      percentageCompleted = percentageCompleted == NaN ? 0 : percentageCompleted;
      rows.push({
        roleId: companyTasks[i].RoleId || 0,
        userId: companyTasks[i].UserId || 0,
        companyId: companyTasks[i].CompanyId || 0,
        company: companyTasks[i].CompanyName || "",
        incompleteUsers: incompleteQualification || 0,
        completedUsers: completedQualification || 0,
        total: (incompleteQualification + completedQualification) || 0,
        percentageCompleted: Math.round(percentageCompleted) + "%" || "0%"
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
    let rows = this.createRows(newProps.companyDetails),
      isArray = Array.isArray(newProps.companyDetails),
      isInitial = isArray;

    if (rows) {
      this.state.modal = newProps.modal;
      this.state.rows = rows;
      this.state.isInitial = isInitial;
      this.state.title = newProps.title || "";
    } else {
      this.setState({
        modal: newProps.modal,
        rows: rows,
        isInitial: isInitial,
        title: newProps.title || ""
      });
    }
  };

  /**
  * @method
  * @name - getUserDetails
  * This method will be used to get Companies User details
  * @param company
  * @param companyId
  * @returns none
  */
  async getUserDetails(company, contractorCompanyId, isCompleted, roleId) {
    const { cookies } = this.props;

    let { contractorManagementDetails } = sessionStorage || '{}';
    contractorManagementDetails = JSON.parse(contractorManagementDetails);
    // get the company Id from the session storage 
    let companyId = contractorManagementDetails.Company.Id || 0;
    let adminId = parseInt(contractorManagementDetails.User.Id) || 0;

    let fields = [{ "Name": "CONTRACTOR_COMPANY", "Value": contractorCompanyId, "Operator": "=" }, { "Name": "ROLE_ID", "Value": roleId, "Operator": "=", "Bitwise": "and" }, { "Name": "ADMIN_ID", "Value": adminId, "Operator": "=", "Bitwise": "and" }];

    if (isCompleted) {
      fields.push({ "Name": "STATUS", "Value": "COMPLETED", "Operator": "=", "Bitwise": "and" });     
    } else {
      fields.push({ "Name": "STATUS", "Value": "IN_COMPLETE", "Operator": "=", "Bitwise": "and" });
    }
    if (isCompleted == null) {
      fields = [{ "Name": "CONTRACTOR_COMPANY", "Value": contractorCompanyId, "Operator": "=" }, { "Name": "ROLE_ID", "Value": roleId, "Operator": "=", "Bitwise": "and" }, { "Name": "ADMIN_ID", "Value": adminId, "Operator": "=", "Bitwise": "and" }, { "Name": "STATUS", "Value": "ALL", "Operator": "=", "Bitwise": "and" }];
    }
    const postData = {
      "Fields": fields,
      "ColumnList": ['USER_ID', 'COMPANY_ID', 'EMPLOYEE_NAME', 'ASSIGNED_COMPANY_QUALIFICATION', 'COMPLETED_COMPANY_QUALIFICATION', 'IN_COMPLETE_COMPANY_QUALIFICATION'],
      "AppType": "TRAINING_DASHBOARD"
    };

    let isUserDetailsModal = this.state.isUserDetailsModal,
      userDetails = {},
      selectedCompany = company;
    isUserDetailsModal = true;
    this.setState({ isUserDetailsModal, userDetails, selectedCompany });
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
      userDetails = response;
      isUserDetailsModal = true;
      this.setState({ ...this.state, isUserDetailsModal, userDetails });
    }
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
      [modelName]: value,
      selectedCompany: ""
    });
  };

  /**
   * @method
   * @name - toggle
   * This method will update the current of modal window
   * @param none
   * @returns none
   */
  toggle() {
    this.setState({
      modal: !this.state.modal
    });
    this.props.updateState("isCompanyDetailsModal");
  }

  /**
   * @method
   * @name - handleCellClick
   * This method will trigger the event of APIs respective to cell clicked Data Grid
   * @param type
   * @param args
   * @returns none
   */
  handleCellClick = (type, args) => {
    const { title } = this.state;
    let companyId = args.companyId || 0,
      roleId = args.roleId || 0,
      company = args.company || "",
      companyType = title ? title.split('-')[0] + "- " + company : company,
      isCompleted = type == "completedUsers";
    switch (type) {
      case "incompleteUsers":
      case "completedUsers":
        this.getUserDetails(companyType, companyId, isCompleted, roleId);
        break;
      case "total":
      case "percentageCompleted":
        this.getUserDetails(companyType, companyId, null, roleId);
        break;
      default:
        console.log(companyType, companyId, isCompleted, roleId);
        break;
    }
    // this.refs.incompleteCompaniesReactDataGrid.deselect();
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
        <CompanyUserDetail
          backdropClassName={"no-backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isUserDetailsModal}
          userDetails={this.state.userDetails}
          title={this.state.selectedCompany}
        />
        <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid grid-modal-popup">
          <ModalHeader className="text-left" toggle={this.toggle}>
            {titleText}
            <p className="section-info-description">Completed Users shows number of users who have completed all tasks in the role, over the total users in the role</p>
            <p className="section-info-description">% Complete shows as a percent the number of users who have completed all tasks in the role vs total users in the role</p>
          </ModalHeader>
          <div>
            <div className="export-menu-one">

            </div>
            <div className="export-menu-two">
              <Export
                data={this.state.rows}
                heads={this.heads}
                sheetName={titleText}
              />
            </div>
          </div>
          <ModalBody>
            <div className="grid-container">
              <div className="table">
                <ReactTable
                  minRows={1}
                  data={rows}
                  columns={[
                    {
                      Header: "Company",
                      id: "company",
                      accessor: "company",
                      headerClassName: 'header-wordwrap',
                      minWidth: 300,
                      className: 'text-left',
                    },
                    {
                      Header: "Incomplete Users",
                      id: "incompleteUsers",
                      accessor: "incompleteUsers",
                      headerClassName: 'header-wordwrap',
                      minWidth: 150,
                      maxWidth: 200,
                      className: 'text-center',
                      Cell: this.customCell,
                    },
                    {
                      Header: "Completed Users",
                      id: "completedUsers",
                      accessor: "completedUsers",
                      headerClassName: 'header-wordwrap',
                      minWidth: 150,
                      maxWidth: 200,
                      className: 'text-center',
                      Cell: this.customCell,
                    },
                    {
                      Header: "Total",
                      id: "total",
                      accessor: "total",
                      headerClassName: 'header-wordwrap',
                      minWidth: 150,
                      maxWidth: 200,
                      className: 'text-center',
                      Cell: this.customCell,
                    },
                    {
                      Header: "Percentage Completed",
                      id: "percentageCompleted",
                      accessor: "percentageCompleted",
                      headerClassName: 'header-wordwrap',
                      minWidth: 150,
                      maxWidth: 200,
                      className: 'text-center',
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
                      id: "company",
                      desc: false
                    }
                  ]}
                  style={{
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

export default ContractorCompanyDetail;
