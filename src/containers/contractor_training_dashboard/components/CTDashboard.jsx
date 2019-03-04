/* eslint-disable */
/*
* CTDashboard.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details to list the workbooks 
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------
createRows()
handleGridRowsUpdated(fromRow, toRow, updated)
handleGridSort(sortColumn, sortDirection)
*/
import React, { PureComponent } from 'react';
import { CardBody } from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import * as API from '../../../shared/utils/APIUtils';
import * as Constants from '../../../shared/constants';


const mockDataLevelOne = [
  {
    "Role": "Bundle 1",
    "IncompleteCompanies": 1,
    "CompletedCompanies": 2
  },
  {
    "Role": "Bundle 2",
    "IncompleteCompanies": 0,
    "CompletedCompanies": 1
  },
  {
    "Role": "Bundle 3",
    "IncompleteCompanies": 4,
    "CompletedCompanies": 0
  },
  {
    "Role": "Core Training",
    "IncompleteCompanies": 3,
    "CompletedCompanies": 3
  }
];

/**
 * DataTableEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-table module.
 */
class DataTableEmptyRowsView extends React.Component {
  render() {
    return (<div className="no-records-found">Sorry, no records</div>)
  }
};

class CTDashboard extends PureComponent {

  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor() {
    super();
    this.heads = [
      {
        key: 'role',
        name: 'Role',
        sortable: true,
        width: 200,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'incompleteCompanies',
        name: 'Incomplete Companies',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-center"
      },
      {
        key: 'completedCompanies',
        name: 'Completed Companies',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-center last-column"
      }
    ];

    this.state = {
      rows: this.createRows([]),
      isInitial: false
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
    const { cookies } = this.props;
    let companyId = cookies.get('CompanyId'),
      userId = cookies.get('UserId');
    this.getRoles(companyId, userId);
  };

  /**
  * @method
  * @name - getEmployees
  * This method will used to get Employees details
  * @param userId
  * @returns none
  */
  async getRoles(companyId, userId) {
    const { cookies } = this.props;
    const postData = {
      "Fields": [{ "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }],
      "ColumnList": Constants.GET_EMPLOYEES_COLUMNS
    };
    let token = cookies.get('IdentityToken'),
      url = "/company/" + companyId + "/workbooks",
      // response = await API.ProcessAPI(url, postData, token, false, "POST", true),
      // rows = this.createRows(response),
      isInitial = true,
      rows = this.createRows(mockDataLevelOne);
    this.setState({ rows: rows, isInitial: isInitial });
  };

  /**
  * @method
  * @name - createRows
  * This method will format the input data
  * for Data Grid
  * @param roleDetails
  * @returns rows
  */
  createRows = (roleDetails) => {
    const rows = [],
      length = roleDetails ? roleDetails.length : 0;
    for (let i = 0; i < length; i++) {
      rows.push({
        role: roleDetails[i].Role || "",
        incompleteCompanies: parseInt(roleDetails[i].IncompleteCompanies),
        completedCompanies: parseInt(roleDetails[i].CompletedCompanies)
      });
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

  // This method is used to setting the row data in react data grid
  rowGetter = i => this.state.rows[i];

  render() {
    const { rows } = this.state;
    return (
      <CardBody>
        <div className="card__title">
          <div className="pageheader">
            <img src="https://d2tqbrn06t95pa.cloudfront.net/img/topnav_reports.png?v=2" /> Contractor Training Dashboard
            </div>
          <p className="card__description">This is the default level. Shows a list of all shared roles and the overall progress of the entire contractor fleet(all contractor companies)</p>
        </div>
        <div className="grid-container">
          <div className="table is-table-page-view">
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
              emptyRowsView={this.state.isInitial && DataTableEmptyRowsView}
            />
          </div>
        </div>
      </CardBody>
    );
  }
}

export default withCookies(CTDashboard);
