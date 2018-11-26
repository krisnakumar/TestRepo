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

import SplitPane  from 'react-split-pane';



const options = [
  { value: 'employees', label: 'Employees' },
  { value: 'workbooks', label: 'Workbooks' },
  { value: 'tasks', label: 'Tasks' }
];

class QuerySection extends PureComponent {

  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor() {
    super();
    
    this.state = {    
      selectedOption: options[0], 
      isClearable: false
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
                  />
                </Col>                
                <Col xs="auto">
                  <Button className="query-section-button" size="sm" aria-label="Run Query">
                    <span aria-hidden>&ndash;</span> Run Query
                  </Button>
                </Col>
                <Col xs="auto">
                  <Button className="query-section-button" size="sm" aria-label="Reset">
                    <span aria-hidden>&ndash;</span> Reset
                  </Button>
                </Col>                
              </Row>
            </div>
      
            <div className="wrapper">
              <SplitterLayout primaryIndex={0} primaryMinSize={150} secondaryMinSize={150} customClassName={"query-builder-section"} vertical={true}>
                
              </SplitterLayout>
            </div>
          </CardBody>
    );
  }
}

export default withCookies(QuerySection);