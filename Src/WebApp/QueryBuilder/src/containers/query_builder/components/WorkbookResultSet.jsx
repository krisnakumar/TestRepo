/* eslint-disable */
/*
* WorkbookResultSet.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details
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
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import ResultSetGuidanceMessage from './ResultSetGuidanceMessage';
import ResultSetEmptyMessage from './ResultSetEmptyMessage';
import ResultSetLoadingMessage from './ResultSetLoadingMessage';

class WorkbookResultSet extends React.Component {
  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor(props) {
    super(props);
    this.heads = [
      {
        key: 'workbookId',
        name: 'Workbook ID ',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-right"
      },
      {
        key: 'workbookName',
        name: 'Workbook',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'description',
        name: 'Description',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.descCellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'createdBy',
        name: 'Created By',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
      },
      {
        key: 'dayToComplete',
        name: 'Day to Complete',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-right last-column"
      }
    ];

    this.state = {
      rows: this.createRows(this.props.workbooks),
      pageOfItems: [],
      heads: this.props.columns || this.heads,
      isInitial: false,
      sortColumn: "",
      sortDirection: "NONE",
      isLoading: false
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
      props.value
    );
  }

  /**
  * @method
  * @name - descCellFormatter
  * This method will format the cell column other than workbooks Data Grid
  * @param props
  * @returns none
  */
  descCellFormatter = (props) => {
    return (
      <p className="word-wrap" title={props.value}>{props.value}</p>
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
  * @param workbooks
  * @returns rows
  */
  createRows = (workbooks) => {
    const rows = [],
      length = workbooks ? workbooks.length : 0;
    for (let i = 0; i < length; i++) {
      rows.push({
        workbookId: workbooks[i].WorkBookId || "",
        workbookName: workbooks[i].WorkBookName || "",
        description: workbooks[i].Description || "",
        createdBy: workbooks[i].CreatedBy || "",
        dayToComplete: workbooks[i].DaysToComplete || "",
        isEnabled: workbooks[i].IsEnabled || "",
        assignedTo: workbooks[i].AssignedTo || "",
        dueDate: workbooks[i].DueDate || "",
        supervisorId: workbooks[i].SupervisorId || "",
        userId: workbooks[i].UserId || "",
        isEnabled: workbooks[i].IsEnabled || ""
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
    let rows = this.createRows(newProps.workbooks || []),
      isArray = Array.isArray(newProps.workbooks || []),
      isInitial = isArray,
      isLoading = (newProps.workbooks == undefined);

    const { sortColumn, sortDirection } = this.state
    if (sortColumn != "" && sortDirection != "NONE") {
      this.state.rows = rows;
      this.state.isInitial = isInitial;
      this.state.isLoading = isLoading;
      this.handleGridSort(sortColumn, sortDirection);
    } else {
      this.setState({
        rows: rows,
        isInitial: isInitial,
        isLoading: isLoading
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

  /**
   * @method
   * @name - buildColumns
   * This method will build the columns dynamically from state
   * @param columns
   * @returns columns
   */
  buildColumns = columns =>
    columns.map((col, i) => ({
      ...col,
      formatter: this.cellFormatter,
      hash: new Date().getTime()
    }));

  /**
 * @method
 * @name - addColumns
 * This method will add the columns to the state
 * @param columnOptions
 * @returns none
 */
  addColumns = (columnOptions) => {
    let length = columnOptions.length - 1,
      cellClass = columnOptions[length].cellClass + " last-column";
    columnOptions[length].cellClass = cellClass;
    this.setState({
      heads: this.buildColumns(columnOptions)
    });
    this.forceUpdate();
  };

  render() {
    const { rows } = this.state;
    return (
      <div className="grid-container employees-result">
        <div className="table employees-result-set">
          <ReactDataGrid
            ref={'WorkbookResultSet'}
            onGridSort={this.handleGridSort}
            enableCellSelect={false}
            enableCellAutoFocus={false}
            columns={this.state.heads}
            rowGetter={this.rowGetter}
            rowsCount={this.state.rows.length}
            minHeight={500}
            onGridRowsUpdated={this.handleGridRowsUpdated}
            rowHeight={25}
            headerRowHeight={32}
            minColumnWidth={100}
            emptyRowsView={this.state.isInitial ? (this.state.isLoading ? ResultSetLoadingMessage : ResultSetEmptyMessage) : ResultSetGuidanceMessage}
          />
        </div>
      </div>
    );
  }
}

export default withCookies(WorkbookResultSet);