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
import { CardBody} from 'reactstrap';
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import OQDashboardMock from '../components/OQDashboardMock.json'


/**
 * EmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-data-grid module.
 */
class OQDashboardEmptyRowsView extends React.Component{
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
        formatter: this.cellFormatter,
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
        formatter: this.cellFormatter,
        cellClass: "text-right"
      },
      {
        key: 'completedQualification',
        name: 'Completed Qualification',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-right"
      },
      {
        key: 'inCompletedQualification',
        name: 'Incomplete Qualification',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-right"
      },
      {
        key: 'pastDue',
        name: 'Past Due (30 Days)',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-right"
      },
      {
        key: 'comingDue',
        name: 'Due in 30 Days',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-right"
      },
      {
        key: 'total',
        name: 'Total Employees',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-right last-column"
      },
    ];

    this.state = {
      rows: this.createRows(OQDashboardMock.contractorOQView)
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
    // let qualificationsArrayLength = qualificationsArray.length - 1;
    // let qualifications = qualificationsArray[qualificationsArrayLength];
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

    if(length > 0){      
      rows.push({contractors: "Total", role: "", assignedQualification: assignedQualificationCount, completedQualification: completedQualificationCount , inCompletedQualification: inCompletedQualificationCount, pastDue: pastDueCount, comingDue: comingDueCount, total: totalQualificationCount});
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
    if(beforePopRows.length > 0)
    {
      totalRow = beforePopRows.pop();
    }

    const sortRows = beforePopRows.slice(0);
    const rows = sortDirection === 'NONE' ? this.state.rows.slice(0, 10) : sortRows.sort(comparer).slice(0, 10);
    
    if(beforePopRows.length > 0)
      rows.push(totalRow);

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
              <img src="https://d2tqbrn06t95pa.cloudfront.net/img/topnav_reports.png?v=2"/> Contractor OQ Dashboard
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
                      // emptyRowsView={this.state.isInitial && OQDashboardEmptyRowsView} 
                  />
              </div>
            </div>
          </CardBody>
    );
  }
}

export default withCookies(OQDashboard);