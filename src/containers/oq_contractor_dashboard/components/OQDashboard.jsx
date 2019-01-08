/* eslint-disable */
/*
* OQDashboard.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details to list the workbooks 
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------
createRows(employees)
handleGridRowsUpdated(fromRow, toRow, updated)
handleGridSort(sortColumn, sortDirection)
*/
import React, { PureComponent } from 'react';
import { CardBody, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import OQDashboardMock from '../components/OQDashboardMock.json';
import ContractorView from '../components/ContractorView';
import AssignedQualification from '../components/AssignedQualification';
import CompletedQualification from '../components/CompletedQualification';
import InCompletedQualification from '../components/InCompletedQualification';
import PastDueQualification from '../components/PastDueQualification';
import ComingDueQualification from '../components/ComingDueQualification';

/**
 * EmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-data-grid module.
 */
class OQDashboardEmptyRowsView extends React.Component {
  render() {
    return (<div className="no-records-found-modal">Sorry, no records</div>)
  }
};

class OQDashboard extends PureComponent {

  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor() {
    super();
    this.heads = [
      {
        key: 'contractors',
        name: 'Contractors',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.qualificationsFormatter("total", props),
        cellClass: "text-left"
      },
      {
        key: 'role',
        name: 'Role',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'assignedQualification',
        name: 'Assigned Qualification',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.qualificationsFormatter("assignedQualification", props),
        cellClass: "text-right"
      },
      {
        key: 'completedQualification',
        name: 'Completed Qualification',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.qualificationsFormatter("completedQualification", props),
        cellClass: "text-right"
      },
      {
        key: 'inCompletedQualification',
        name: 'Incomplete Qualification',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.qualificationsFormatter("inCompletedQualification", props),
        cellClass: "text-right"
      },
      {
        key: 'pastDue',
        name: 'Past Due (30 Days)',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.qualificationsFormatter("pastDue", props),
        cellClass: "text-right"
      },
      {
        key: 'comingDue',
        name: 'Due in 30 Days',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.qualificationsFormatter("comingDue", props),
        cellClass: "text-right"
      },
      {
        key: 'total',
        name: 'Total Employees',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.qualificationsFormatter("total", props),
        cellClass: "text-right last-column"
      },
    ];

    this.state = {
      rows: this.createRows(OQDashboardMock.contractorOQView),
      isContractorView: false,
      isAssignedQualificationView: false,
      isCompletedQualificationView: false,
      isInCompletedQualificationView: false,
      isPastDueQualificationView: false,
      isComingDueQualificationView: false,
      contractorQualifications: {},
      assignedQualifications: {},
      completedQualifications: {},
      inCompletedQualifications: {},
      pastDueQualifications: {},
      comingDueQualifications: {}
    };
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
   * @name - componentDidMount
   * This method will invoked whenever the component is mounted
   *  is update to this component class
   * @param none
   * @returns none
  */
  componentDidMount() {
    // Do API call for loading initial table view
  };



  /**
  * @method
  * @name - createRows
  * This method will format the input data
  * for Data Grid
  * @param qualifications
  * @returns rows
  */
  createRows = (qualifications) => {
    let assignedQualificationCount = 0,
      completedQualificationCount = 0,
      inCompletedQualificationCount = 0,
      pastDueCount = 0,
      comingDueCount = 0,
      totalQualificationCount = 0;
    const rows = [],
      length = qualifications ? qualifications.length : 0;
    for (let i = 0; i < length; i++) {
      assignedQualificationCount += parseInt(qualifications[i].assignedQualification);
      completedQualificationCount += parseInt(qualifications[i].completedQualification);
      inCompletedQualificationCount += parseInt(qualifications[i].inCompletedQualification);
      pastDueCount += parseInt(qualifications[i].pastDue);
      comingDueCount += parseInt(qualifications[i].comingDue);
      totalQualificationCount += parseInt(qualifications[i].total);
      rows.push({
        contractors: qualifications[i].contractors,
        role: qualifications[i].role,
        assignedQualification: qualifications[i].assignedQualification,
        completedQualification: qualifications[i].completedQualification,
        inCompletedQualification: qualifications[i].inCompletedQualification,
        pastDue: qualifications[i].pastDue,
        comingDue: qualifications[i].comingDue,
        total: qualifications[i].total,
      });
    }

    if (length > 0) {
      rows.push({ contractors: "Total", role: "", assignedQualification: assignedQualificationCount, completedQualification: completedQualificationCount, inCompletedQualification: inCompletedQualificationCount, pastDue: pastDueCount, comingDue: comingDueCount, total: totalQualificationCount });
    }
    return rows;
  };

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
        return (a[sortColumn] > b[sortColumn]) ? 1 : -1;
      } else if (sortDirection === 'DESC') {
        return (a[sortColumn] < b[sortColumn]) ? 1 : -1;
      }
    };

    const beforePopRows = this.state.rows;
    let totalRow = "";
    if (beforePopRows.length > 0) {
      totalRow = beforePopRows.pop();
    }

    const sortRows = beforePopRows.slice(0),
      rowsLength = this.state.rows.length || 0;
    const rows = sortDirection === 'NONE' ? this.state.rows.slice(0, rowsLength) : sortRows.sort(comparer).slice(0, rowsLength);

    if (beforePopRows.length > 0)
      rows.push(totalRow);

    this.setState({ rows });
  };

  // This method is used to setting the row data in react data grid
  rowGetter = i => this.state.rows[i];

  /**
   * @method
   * @name - qualificationsFormatter
   * This method will format the qualification Data Grid
   * @param type
   * @param props
   * @returns none
   */
  qualificationsFormatter = (type, props) => {
    if (props.dependentValues[type] <= 0 || props.dependentValues.contractors == "Total") {
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
    let userId = 0;
    switch (type) {
      case "contractors":
      case "total":
        this.getContractorQualifications(userId);
        break;
      case "assignedQualification":
        this.getAssignedQualifications(userId);
        break;
      case "completedQualification":
        this.getCompletedQualifications(userId);
        break;
      case "inCompletedQualification":
        this.getInCompletedQualifications(userId);
        break;
      case "pastDue":
        this.getPastDueQualifications(userId);
        break;
      case "comingDue":
        this.getComingDueQualifications(userId);
        break;
      default:
        console.log("default", type, args);
        break;
    }
    this.refs.oQDashboardReactDataGrid.deselect();
  };

  /**
   * @method
   * @name - getContractorQualifications
   * This method will used to get Contractor Qualifications
   * @param userId
   * @returns none
   */
  async getContractorQualifications(userId) {
    const { cookies } = this.props;

    let isContractorView = this.state.isContractorView,
      contractorQualifications = {};
    isContractorView = true;
    this.setState({ isContractorView, contractorQualifications });

    // Commented due to using mock JSON

    // let token = cookies.get('IdentityToken'),
    //     url = "Need to update url",
    //     response = await API.ProcessAPI(url, "", token, false, "GET", true);

    let response = OQDashboardMock.employeeView;

    contractorQualifications = response;
    isContractorView = true;
    this.setState({ ...this.state, isContractorView, contractorQualifications });
  };

  /**
   * @method
   * @name - getAssignedQualifications
   * This method will used to get Assigned Qualifications
   * @param userId
   * @returns none
   */
  async getAssignedQualifications(userId) {
    const { cookies } = this.props;

    let isAssignedQualificationView = this.state.isAssignedQualificationView,
      assignedQualifications = {};
    isAssignedQualificationView = true;
    this.setState({ isAssignedQualificationView, assignedQualifications });

    // Commented due to using mock JSON

    // let token = cookies.get('IdentityToken'),
    //     url = "Need to update url",
    //     response = await API.ProcessAPI(url, "", token, false, "GET", true);

    let response = OQDashboardMock.assignedQualification;

    assignedQualifications = response;
    isAssignedQualificationView = true;
    this.setState({ ...this.state, isAssignedQualificationView, assignedQualifications });
  };

  /**
  * @method
  * @name - getCompletedQualifications
  * This method will used to get Completed Qualifications
  * @param userId
  * @returns none
  */
  async getCompletedQualifications(userId) {
    const { cookies } = this.props;

    let isCompletedQualificationView = this.state.isCompletedQualificationView,
      completedQualifications = {};
    isCompletedQualificationView = true;
    this.setState({ isCompletedQualificationView, completedQualifications });

    // Commented due to using mock JSON

    // let token = cookies.get('IdentityToken'),
    //     url = "Need to update url",
    //     response = await API.ProcessAPI(url, "", token, false, "GET", true);

    let response = OQDashboardMock.completedQualifications;

    completedQualifications = response;
    isCompletedQualificationView = true;
    this.setState({ ...this.state, isCompletedQualificationView, completedQualifications });
  };

  /**
  * @method
  * @name - getInCompletedQualifications
  * This method will used to get InCompleted Qualifications
  * @param userId
  * @returns none
  */
  async getInCompletedQualifications(userId) {
    const { cookies } = this.props;

    let isInCompletedQualificationView = this.state.isInCompletedQualificationView,
      inCompletedQualifications = {};
    isInCompletedQualificationView = true;
    this.setState({ isInCompletedQualificationView, inCompletedQualifications });

    // Commented due to using mock JSON

    // let token = cookies.get('IdentityToken'),
    //     url = "Need to update url",
    //     response = await API.ProcessAPI(url, "", token, false, "GET", true);

    let response = OQDashboardMock.inCompletedQualifications;

    inCompletedQualifications = response;
    isInCompletedQualificationView = true;
    this.setState({ ...this.state, isInCompletedQualificationView, inCompletedQualifications });
  };

  /**
  * @method
  * @name - getPastDueQualifications
  * This method will used to get Past Due Qualifications
  * @param userId
  * @returns none
  */
  async getPastDueQualifications(userId) {
    const { cookies } = this.props;

    let isPastDueQualificationView = this.state.isPastDueQualificationView,
      pastDueQualifications = {};
      isPastDueQualificationView = true;
    this.setState({ isPastDueQualificationView, pastDueQualifications });

    // Commented due to using mock JSON

    // let token = cookies.get('IdentityToken'),
    //     url = "Need to update url",
    //     response = await API.ProcessAPI(url, "", token, false, "GET", true);

    let response = OQDashboardMock.inCompletedQualifications;

    pastDueQualifications = response;
    isPastDueQualificationView = true;
    this.setState({ ...this.state, isPastDueQualificationView, pastDueQualifications });
  };


  /**
  * @method
  * @name - getComingDueQualifications
  * This method will used to get Coming due qualifications
  * @param userId
  * @returns none
  */
  async getComingDueQualifications(userId) {
    const { cookies } = this.props;

    let isComingDueQualificationView = this.state.isComingDueQualificationView,
      comingDueQualifications = {};
      isComingDueQualificationView = true;
    this.setState({ isComingDueQualificationView, comingDueQualifications });

    // Commented due to using mock JSON

    // let token = cookies.get('IdentityToken'),
    //     url = "Need to update url",
    //     response = await API.ProcessAPI(url, "", token, false, "GET", true);

    let response = OQDashboardMock.inCompletedQualifications;

    comingDueQualifications = response;
    isComingDueQualificationView = true;
    this.setState({ ...this.state, isComingDueQualificationView, comingDueQualifications });
  };

  render() {
    const { rows } = this.state;
    return (
      <CardBody>
        <ContractorView
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isContractorView}
          contractorQualifications={this.state.contractorQualifications}
        />
        <AssignedQualification
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isAssignedQualificationView}
          assignedQualifications={this.state.assignedQualifications}
        />
        <CompletedQualification
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isCompletedQualificationView}
          completedQualifications={this.state.completedQualifications}
        />
        <InCompletedQualification
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isInCompletedQualificationView}
          inCompletedQualifications={this.state.inCompletedQualifications}
        />
        <PastDueQualification
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isPastDueQualificationView}
          pastDueQualifications={this.state.pastDueQualifications}
        />
        <ComingDueQualification
          backdropClassName={"backdrop"}
          updateState={this.updateModalState.bind(this)}
          modal={this.state.isComingDueQualificationView}
          comingDueQualifications={this.state.comingDueQualifications}
        />
        <div className="card__title">
          <div className="pageheader">
            <img src="https://d2tqbrn06t95pa.cloudfront.net/img/topnav_reports.png?v=2" /> Contractor OQ Dashboard
            </div>
          <p className="card__description">Add workbook widgets here</p>
        </div>
        <div className="grid-container">
          <div className="table has-total-row">
            <ReactDataGrid
              ref={'oQDashboardReactDataGrid'}
              onGridSort={this.handleGridSort}
              enableCellSelect={false}
              enableCellAutoFocus={false}
              columns={this.heads}
              rowGetter={this.rowGetter}
              rowsCount={rows.length}
              onGridRowsUpdated={this.handleGridRowsUpdated}
              rowHeight={35}
              minColumnWidth={100}
              // rowRenderer={RowRenderer}
              // emptyRowsView={this.state.isInitial && OQDashboardEmptyRowsView} 
            />
          </div>
        </div>
      </CardBody>
    );
  }
}

export default withCookies(OQDashboard);

const RowRenderer = ({ row, idx }) => {
  debugger;
  return (
      {row}
  );
};