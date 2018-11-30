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

const initialState = {
  entity: "employees",
  fieldData: FieldData.field["employees"].slice(0, 2)   
};

class QueryPane extends PureComponent {

  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor(props) {
    super(props);
    this.queryClause = React.createRef();

    this.state = initialState;

    this.passEmployeesResults = this.passEmployeesResults.bind(this);
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

  runQuery = () => {
    this.queryClause.current.buildQuery();
  };

  resetQuery = () => {
    let fieldData = initialState.fieldData;
    this.setState(fieldData);
    this.forceUpdate();
  };

  passEmployeesResults(employees){
   this.props.passEmployeesResultsToQuerySection(employees);
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
              passEmployeesResults={this.passEmployeesResults}
            />          
        </Table> 
      </div>
    );
  }
}

export default withCookies(QueryPane);