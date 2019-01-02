/* eslint-disable */
/*
* EmployeeResultSet.jsx
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
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import ResultSetGuidanceMessage from './ResultSetGuidanceMessage';
import ResultSetEmptyMessage from './ResultSetEmptyMessage';

class EmployeeResultSet extends React.Component { 
  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor(props) {
    super(props);
    this.heads = [
      {
        key: 'employeeName',
        name: 'Employee Name',
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
        key: 'userName',
        name: 'User Name',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
        },
        {
        key: 'email',
        name: 'Email',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
        },
        {
        key: 'alternateName',
        name: 'Alternative Name',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
        },
        {
        key: 'totalEmployees',
        name: 'Total Employees',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-right last-column"
        }
      ];
    
    this.state = {    
      rows: this.createRows(this.props.employees),
      pageOfItems: [],
      isInitial: false,
      sortColumn: "",
      sortDirection: "NONE",
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
        alternateName:  employees[i].AlternateName,
        email: employees[i].Email,
        employeeName: employees[i].EmployeeName,
        role: employees[i].Role,
        totalEmployees: employees[i].TotalEmployees,
        userName: employees[i].UserName
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
      let rows = this.createRows(newProps.employees),
          isArray = Array.isArray(newProps.employees),
          isInitial = isArray;

      const {sortColumn , sortDirection } = this.state;

      if(sortColumn != "" && sortDirection != "NONE"){
        this.state.rows = rows;
        this.state.isInitial = isInitial;
        this.handleGridSort(sortColumn, sortDirection);
      } else {
        this.setState({
          rows: rows,
          isInitial: isInitial      
        });
      }
     
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
    this.state.sortColumn = sortColumn;
    this.state.sortDirection = sortDirection;
    const comparer = (a, b) => {
      if (sortDirection === 'ASC') {
        return (a[sortColumn] > b[sortColumn]) ? 1 : -1;
      } else if (sortDirection === 'DESC') {
        return (a[sortColumn] < b[sortColumn]) ? 1 : -1;
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
        <div className="grid-container employees-result">
            <div className="table employees-result-set">
                <ReactDataGrid
                    ref={'employeeResultSet'}
                    onGridSort={this.handleGridSort}
                    enableCellSelect={false}
                    enableCellAutoFocus={false}
                    columns={this.heads}
                    rowGetter={this.rowGetter}
                    rowsCount={rows.length}
                    onGridRowsUpdated={this.handleGridRowsUpdated}
                    rowHeight={25}
                    headerRowHeight={32}
                    minColumnWidth={100}
                    emptyRowsView={this.state.isInitial ? ResultSetEmptyMessage : ResultSetGuidanceMessage} 
                />
            </div>
        </div>
    );
  }
}

export default withCookies(EmployeeResultSet);