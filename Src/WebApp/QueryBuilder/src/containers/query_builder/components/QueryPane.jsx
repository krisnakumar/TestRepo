/* eslint-disable */
/*
* QueryPane.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details to list the workbooks 
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
import React, { PureComponent } from 'react';
import 'whatwg-fetch'
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import { Table, CardBody, Button, Container, Row, Col } from 'reactstrap';
import FieldData from './../data';
import QueryClause from './QueryClause';
import SessionPopup from './SessionPopup';

class QueryPane extends PureComponent {

  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor(props) {
    super(props);
    this.queryClause = React.createRef();

    this.initialState = {
      entity: this.props.selectedOption ? this.props.selectedOption.value : "employees",
      fieldData: FieldData.field[this.props.selectedOption ? this.props.selectedOption.value : "employees"].slice(0, 2),
      isSessionPopup: false,
      sessionPopupType: "API"
    };

    this.state = this.initialState;

    this.passEmployeesResults = this.passEmployeesResults.bind(this);
    this.passWorkbooksResults = this.passWorkbooksResults.bind(this);
    this.passTasksResults = this.passTasksResults.bind(this);
    this.checkQuerySelections = this.checkQuerySelections.bind(this);
    this.passReloadQuery = this.passReloadQuery.bind(this);
    this.passResultSetColumns = this.passResultSetColumns.bind(this);
  }

  /**
   * @method
   * @name - componentWillReceiveProps
   * This method will invoked whenever the props or state
   *  is update to this component class
   * @param newProps
   * @returns none
  */
  componentWillReceiveProps(newProps) {
    this.setState({
      entity: newProps.selectedOption ? newProps.selectedOption.value : "employees",
      fieldData: FieldData.field[newProps.selectedOption ? newProps.selectedOption.value : "employees"].slice(0, 2)
    });
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
  };

  /**
   * @method
   * @name - runQuery
   * This method triggers the parent component buildQuery function
   * @param none
   * @returns none
  */
  runQuery = () => {
    this.queryClause.current.buildQuery();
  };

  /**
   * @method
   * @name - runQuery
   * This method triggers the parent component buildQuery function
   * @param none
   * @returns none
  */
  reloadQuery = (addedColumnOptions) => {
    let type = this.state.entity;
    switch (type) {
      case 'employees':
        this.props.passEmployeesResultsToQuerySection(undefined);
        break;
      case 'workbooks':
        this.props.passWorkbooksResultsToQuerySection(undefined);
        break;
      case 'tasks':
        this.props.passTasksToQuerySection(undefined);
        break;
      default:
        break;
    }
    this.queryClause.current.reloadQuery(addedColumnOptions);
  };

  /**
    * @method
    * @name - resetQuery
    * This method reset the query clause by trigger the parent component function
    * @param none
    * @returns none
 */
  resetQuery = () => {
    this.queryClause.current.resetQueryClause();
  };

  /**
   * @method
   * @name - passEmployeesResults
   * This method used to pass the employees parent component
   * @param employees
   * @returns none
  */
  passEmployeesResults(employees) {
    if (employees == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if(employees == 'API_ERROR'){
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {      
      this.props.passEmployeesResultsToQuerySection(employees);
    }
  }

  /**
   * @method
   * @name - passWorkbooksResults
   * This method used to pass the workbooks parent component
   * @param workbooks
   * @returns none
  */
  passWorkbooksResults(workbooks) {
    if (workbooks == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if(workbooks == 'API_ERROR'){
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {      
      this.props.passWorkbooksResultsToQuerySection(workbooks);
    }    
  }

  /**
  * @method
  * @name - passTasksResults
  * This method used to pass the tasks parent component
  * @param tasks
  * @returns none
 */
  passTasksResults(tasks) {
    if (tasks == 401) {
      this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
    } else if(tasks == 'API_ERROR'){
      this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
    } else {            
      this.props.passTasksToQuerySection(tasks);
    }  
  }

  /**
* @method
* @name - passReloadQuery
* This method used to pass the reload query parent component
* @param tasks
* @returns none
*/
  passReloadQuery() {
    this.props.onRunQueryClick();
  }

  /**
  * @method
  * @name - checkQuerySelections
  * This method used to check query selection state is initial
  * @param none
  * @returns none
 */
  checkQuerySelections() {
    let isSame = this.queryClause.current.checkQueryState();
    return isSame;
  }

  /**
   * @method
   * @name - passEmployeesColumns
   * This method used to pass the tasks parent component
   * @param tasks
   * @returns none
 */
  passResultSetColumns(type, columns) {
    switch (type) {
      case 'employees':
        this.props.passEmployeesColumnsToQuerySection(type, columns);
        break;
      case 'workbooks':
        this.props.passWorkbooksColumnsToQuerySection(type, columns);
        break;
      case 'tasks':
        this.props.passTasksColumnsToQuerySection(type, columns);
        break;
      default:
        break;
    }
  }

  render() {
    return (
      <div className="query-builder-section">
        <SessionPopup
          backdropClassName={"backdrop"}
          modal={this.state.isSessionPopup}
          sessionPopupType={this.state.sessionPopupType}
        />
        <Table className="query-section-table">
          <thead className="query-section-table thead">
            <tr>
              <th width="180" className="tableWidth-5">Add/Delete</th>
              <th className="tableWidth-10">And/Or</th>
              <th className="tableWidth-20">Field</th>
              <th className="tableWidth-20">Operator</th>
              <th>Value</th>
            </tr>
          </thead>
          <QueryClause
            ref={this.queryClause}
            fieldData={this.state.fieldData}
            entity={this.state.entity}
            reloadQuery={this.passReloadQuery}
            passEmployeesResults={this.passEmployeesResults}
            passWorkbooksResults={this.passWorkbooksResults}
            passTasksResults={this.passTasksResults}
            passResultSetColumns={this.passResultSetColumns}
          />
        </Table>
      </div>
    );
  }
}

export default withCookies(QueryPane);