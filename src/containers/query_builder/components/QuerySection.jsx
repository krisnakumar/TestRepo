/* eslint-disable */
/*
* QuerySection.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details to list the workbooks 
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------

*/
import React, { PureComponent } from 'react';
import { CardBody} from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import * as API from '../../../shared/utils/APIUtils';
import { Button, Container, Row, Col } from 'reactstrap';
import Select from 'react-select';
import SplitterLayout from 'react-splitter-layout';
import QueryPane  from './QueryPane';
import EmployeeResultSet from './EmployeeResultSet';


const options = [
  { value: 'employees', label: 'Employees' },
  { value: 'workbooks', label: 'Workbooks' },
  { value: 'tasks', label: 'Tasks' }
];

class QuerySection extends PureComponent {

  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor(props) {
    super(props);

    this.queryPane = React.createRef();
    this.employeeResultSet = React.createRef();

    this.state = {    
      selectedOption: options[0], 
      isClearable: false,
      employees: {}
    };
    
  }

  handleChange = (selectedOption) => {
    this.setState({ selectedOption });
    console.log(`Option selected:`, selectedOption);
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

  onRunQueryClick = () => {
    this.queryPane.current.runQuery();
  };

  onResetQueryClick = () => {
    this.setState({ employees: {} });
    this.queryPane.current.resetQuery();
  };

  passEmployeesResults= (employees) => {
    this.setState({ employees: employees});
  }

  render() {
    const { selectedOption, isClearable } = this.state;
    return (         
          <CardBody>
            <div className="card__title">
             <div className="pageheader">
              <img src="https://d2tqbrn06t95pa.cloudfront.net/img/topnav_reports.png?v=2"/> Query Builder
            </div>
            <p className="card__description">Customized Queries</p>
            </div>
           
            <div className="grid-container-query-selection">
              <Row>
                <Col xs="2">
                  <Select
                    clearable={isClearable}
                    isRtl={true}
                    isSearchable={false}
                    openOnClick={false}
                    value={selectedOption}
                    onChange={this.handleChange}
                    options={options}
                    placeholder={""}
                    className={"select-entity"}
                  />
                </Col>                
                <Col xs="auto">
                  <button onClick={this.onRunQueryClick} className="query-section-button" size="sm" aria-label="Run Query">
                    <span aria-hidden><i className="fa fa-angle-right"></i></span> Run Query
                  </button>
                </Col>
                <Col xs="auto">
                  <button onClick={this.onResetQueryClick} className="query-section-button" size="sm" aria-label="Reset">
                    <span aria-hidden><i className="fa fa-refresh" aria-hidden="true"></i></span> Reset
                  </button>
                </Col>                
              </Row>
            </div>
      
            <div className="wrapper">
              <SplitterLayout primaryIndex={0} primaryMinSize={200} secondaryMinSize={150} customClassName={"query-builder-section"} vertical={true}>
                <QueryPane ref={this.queryPane} passEmployeesResultsToQuerySection={this.passEmployeesResults} />
                <EmployeeResultSet ref={this.employeeResultSet} employees={this.state.employees}/>
              </SplitterLayout>
            </div>
          </CardBody>
    );
  }
}

export default withCookies(QuerySection);