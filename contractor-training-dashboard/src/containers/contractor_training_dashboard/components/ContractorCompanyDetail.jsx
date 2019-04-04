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
import ReactDataGrid from 'react-data-grid';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import * as API from '../../../shared/utils/APIUtils';
import CompanyUserDetail from './CompanyUserDetail';
import Export from './CTDashboardExport';

/**
 * ContractorCompanyDetailEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react data grid module.
 */
class ContractorCompanyDetailEmptyRowsView extends React.Component {
  render() {
    return (<div className="no-records-found-modal">Sorry, no records</div>)
  }
};

class ContractorCompanyDetail extends React.Component {

  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor(props) {
    super(props);

    this.heads = [
      {
        key: 'company',
        name: 'Company',
        sortable: true,
        width: 200,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'incompleteUsers',
        name: 'Incomplete Users',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.cellClickFormatter("incompleteUsers", props),
        cellClass: "text-right"
      },
      {
        key: 'completedUsers',
        name: 'Completed Users',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.cellClickFormatter("completedUsers", props),
        cellClass: "text-right"
      },
      {
        key: 'total',
        name: 'Total',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.cellClickFormatter("total", props),
        cellClass: "text-right"
      },
      {
        key: 'percentageCompleted',
        name: '% Complete',
        width: 100,
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.cellClickFormatter("percentageCompleted", props),
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
      userDetails: {}
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
  * @param companyTasks
  * @returns rows
  */
  createRows = (companyTasks) => {
    const rows = [],
      length = companyTasks ? companyTasks.length : 0;
    for (let i = 0; i < length; i++) {
      rows.push({
        companyId: companyTasks[i].CompanyId || 0,
        company: companyTasks[i].CompanyName || "",
        incompleteUsers: companyTasks[i].InCompletedCompanyQualification || 0,
        completedUsers: companyTasks[i].CompletedCompanyQualification || 0,
        total: companyTasks[i].TotalCompanyQualification || 0,
        percentageCompleted: ((companyTasks[i].CompletedCompanyQualification / companyTasks[i].TotalCompanyQualification * 100) + "%") || "0%"
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
    this.setState({
      modal: newProps.modal,
      rows: rows,
      isInitial: isInitial,
      title: newProps.title || ""
    });
  };

  /**
  * @method
  * @name - getUserDetails
  * This method will be used to get Companies User details
  * @param company
  * @param companyId
  * @returns none
  */
  async getUserDetails(company, companyId, isCompleted) {
    const { cookies } = this.props;
    let fields = [];

    if (isCompleted) {
      fields.push({ "Name": "COMPLETED", "Value": "true", "Operator": "=" });
    } else {
      fields.push({ "Name": "IN_COMPLETE", "Value": "true", "Operator": "=" });
    }
    if(isCompleted == null){
      fields = [];
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
    let token = idToken,// cookies.get('IdentityToken'),
      url = "/company/" + companyId + "/tasks",
      response = await API.ProcessAPI(url, postData, token, false, "POST", true);

    userDetails = response;

    isUserDetailsModal = true;
    this.setState({ ...this.state, isUserDetailsModal, userDetails });
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
   * @name - handleCellClick
   * This method will trigger the event of APIs respective to cell clicked Data Grid
   * @param type
   * @param args
   * @returns none
   */
  handleCellClick = (type, args) => {
    const { title } = this.state;
    let companyId = args.companyId || 0,
      company = args.company || "",
      companyType = title ? title.split('-')[0] + "- " + company : company,
      isCompleted = type == "completedUsers";
    switch (type) {
      case "incompleteUsers":
      case "completedUsers":
        this.getUserDetails(companyType, companyId, isCompleted);
        break;
      case "total":
      case "percentageCompleted":
        this.getUserDetails(companyType, companyId, null);
        break;
      default:
        console.log(companyType, companyId, isCompleted);
        break;
    }
    this.refs.incompleteCompaniesReactDataGrid.deselect();
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

  // This method is used to setting the row data in react data grid
  rowGetter = i => this.state.rows[i];

  render() {
    const { rows, title } = this.state;
    let titleText = title || "";
    return (
      <div>
        <CompanyUserDetail
          backdropClassName={"no-backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isUserDetailsModal}
          userDetails={this.state.userDetails}
          title={this.state.selectedCompany}
        />
        <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid">
          <ModalHeader className="text-left" toggle={this.toggle}>
            {titleText}
            <p className="section-info-description">Completed Users shows number of users who have completed all tasks in the role, over the total users in the role</p>
            <p className="section-info-description">% Complete shows as a percent the number of users who have completed all tasks in the role vs total users in the role</p>
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
                  ref={'incompleteCompaniesReactDataGrid'}
                  onGridSort={this.handleGridSort}
                  enableCellSelect={false}
                  enableCellAutoFocus={false}
                  columns={this.heads}
                  rowGetter={this.rowGetter}
                  rowsCount={rows.length}
                  onGridRowsUpdated={this.handleGridRowsUpdated}
                  rowHeight={35}
                  minColumnWidth={100}
                  emptyRowsView={this.state.isInitial && ContractorCompanyDetailEmptyRowsView}
                />
              </div>
            </div>
          </ModalBody>
        </Modal>
      </div>
    );
  }
}

export default withCookies(ContractorCompanyDetail);
