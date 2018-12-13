/* eslint-disable */
/*
* QueryPane.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details to list the workbooks 
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------

*/
import React, { PureComponent } from 'react';
import 'whatwg-fetch'
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import { Table, CardBody, Button, Container, Row, Col } from 'reactstrap';
import FieldData from './../data';
import QueryClause from './QueryClause';

class QueryPane extends PureComponent {

  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor(props) {
    super(props);
    this.queryClause = React.createRef();

    this.initialState = {
      entity: this.props.selectedOption.value,
      fieldData: FieldData.field[this.props.selectedOption.value].slice(0, 2)
    };

    this.state = this.initialState;

    this.passEmployeesResults = this.passEmployeesResults.bind(this);
    this.passWorkbooksResults = this.passWorkbooksResults.bind(this); 
    this.passTasksResults = this.passTasksResults.bind(this);
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
        entity: newProps.selectedOption.value,
        fieldData: FieldData.field[newProps.selectedOption.value].slice(0, 2)
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
  passEmployeesResults(employees){
   this.props.passEmployeesResultsToQuerySection(employees);
  }

  /**
   * @method
   * @name - passWorkbooksResults
   * This method used to pass the workbooks parent component
   * @param workbooks
   * @returns none
  */
  passWorkbooksResults(workbooks){
    this.props.passWorkbooksResultsToQuerySection(workbooks);
  }

   /**
   * @method
   * @name - passTasksResults
   * This method used to pass the tasks parent component
   * @param tasks
   * @returns none
  */
  passTasksResults(tasks){
    this.props.passTasksToQuerySection(tasks);
  }

  render() {
    return (         
        <div className="query-builder-section">
          <Table className="query-section-table">
             <thead>
              <tr>
                <th width="125">Add/Delete</th>
                <th>And/Or</th>
                <th>Field</th>
                <th>Operator</th>
                <th>Value</th>
              </tr>
            </thead>
            <QueryClause 
              ref={this.queryClause}
              fieldData={this.state.fieldData}
              entity={this.state.entity}
              passEmployeesResults={this.passEmployeesResults}
              passWorkbooksResults={this.passWorkbooksResults}
              passTasksResults={this.passTasksResults}
            />          
        </Table> 
      </div>
    );
  }
}

export default withCookies(QueryPane);