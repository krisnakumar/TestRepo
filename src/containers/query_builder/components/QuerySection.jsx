/* eslint-disable */
/*
* QuerySection.jsx
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
import { CardBody } from 'reactstrap';
import 'whatwg-fetch'
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import { Row, Col, Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import Select from 'react-select';
import SplitterLayout from 'react-splitter-layout';
import QueryPane from './QueryPane';
import EmployeeResultSet from './EmployeeResultSet';
import WorkbookResultSet from './WorkbookResultSet';
import TaskResultSet from './TaskResultSet';
import SlidingPane from './SlideOut';
import FieldData from './../data';

const options = [
  { value: 'employees', label: 'Employees' },
  { value: 'workbooks', label: 'Workbook' },
  { value: 'tasks', label: 'Task' }
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
      lastSelectedOption: options[0],
      isClearable: false,
      employees: {},
      workbooks: {},
      tasks: {},
      modal: false,
      isEmployee: true,
      isWorkbook: false,
      isTask: false,
      isPaneOpen: false,
      isPaneOpenLeft:  false,
      resultSet: []
    };
    this.toggle = this.toggle.bind(this);
    this.confirmEntitySelection = this.confirmEntitySelection.bind(this);
    this.onOpenClose = this.onOpenClose.bind(this); 
  }

  /**
   * @method
   * @name - toggle
   * This method used set state of modal to open and close
   * @param none
   * @returns none
  */
  toggle() {
    this.setState({
      modal: false,
      isResetModal: false
    });
  }

  /**
  * @method
  * @name - toggle
  * This method used set state of modal to open and close
  * @param none
  * @returns none
 */
  confirmEntitySelection() {
    let isEmployee = this.state.lastSelectedOption.value == "employees",
      isWorkbook = this.state.lastSelectedOption.value == "workbooks",
      isTask = this.state.lastSelectedOption.value == "tasks";

    this.setState({
      isEmployee: isEmployee,
      isWorkbook: isWorkbook,
      isTask: isTask,
      employees: {},
      workbooks: {},
      tasks: {},
      selectedOption: this.state.lastSelectedOption,
      modal: false,
      isResetModal: false
    });
  }

  /**
   * @method
   * @name - handleChange
   * This method used to select query clause entity selection
   * @param selectedOption
   * @returns none
  */
  handleChange = (selectedOption) => {

    if (this.state.selectedOption.value != selectedOption.value) {
      let isSame = this.queryPane.current.checkQuerySelections();
      if (isSame) {
        this.state.modal = false;
        this.state.lastSelectedOption = selectedOption;
        this.confirmEntitySelection();
      } else {
        this.setState({
          modal: true,
          lastSelectedOption: selectedOption
        });
      }
    }
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
  * @name - onRunQueryClick
  * This method build and make a API Request as per the query clause selection
  * @param none
  * @returns none
 */
  onRunQueryClick = () => {
    this.queryPane.current.runQuery();
  };

  /**
   * @method
   * @name - onResetQueryClick
   * This method reset the query clause selection to initial state
   * @param none
   * @returns none
  */
  onResetQueryClick = () => {
    let isSame = this.queryPane.current.checkQuerySelections();
    if (!isSame) {
      this.setState({ isResetModal: true });
    } else {
      this.resetQuery();
    }
  };


 /**
   * @method
   * @name - onColumnOptionsClick
   * This method Add or remove the column from initial state
   * @param none
   * @returns none
  */
 onColumnOptionsClick = () => { 
   this.setState({isPaneOpen:true})
   
  };

   /**
   * @method
   * @name - columnOptionsSlideToggle
   * This method used to get the workbooks from child component
   * @param employees
   * @returns none
  */
 columnOptionsSlideToggle = () => {
  this.setState({ isPaneOpen: false });
}

  /**
   * @method
   * @name - resetQueryClick
   * This method reset the query clause selection to initial state
   * @param none
   * @returns none
  */
  resetQuery = () => {
    this.setState({ employees: {}, workbooks: {}, tasks: {}, isResetModal: false });
    this.queryPane.current.resetQuery();
  };

  /**
   * @method
   * @name - passEmployeesResults
   * This method used to get the employees from child component
   * @param employees
   * @returns none
  */
  passEmployeesResults = (employees) => {
    this.setState({ employees: employees });
  }

  /**
   * @method
   * @name - passEmployeesResults
   * This method used to get the workbooks from child component
   * @param employees
   * @returns none
  */
  passWorkbookResults = (workbooks) => {
    this.setState({ workbooks: workbooks });
  }

  /**
   * @method
   * @name - passTasksResults
   * This method used to get the tasks from child component
   * @param employees
   * @returns none
  */
  passTasksResults = (tasks) => {
    this.setState({ tasks: tasks });
  }

   /**
     * @method
     * @name - onOpenClose
     * This method will used to set the css props to select menu outer to positioning
     * @param none
     * @returns none
    */
   onOpenClose() {
      let inputWrapper = document.querySelector(".is-open").getBoundingClientRect();
          document.querySelector(".Select-menu").style.width = inputWrapper.width+'px';
          document.querySelector(".Select-menu").style.top = inputWrapper.top+inputWrapper.height+'px';
  };

  render() {
    const { selectedOption, isClearable} = this.state;
    this.state.resultSet = []; 
   let fieldDataTemp = FieldData;
    switch(selectedOption.value) {
      case 'employees':
          this.state.resultSet = fieldDataTemp.columns.employees;
          break;
      case 'workbooks':
          this.state.resultSet = fieldDataTemp.columns.workbooks;
          break;
      case 'tasks':
          this.state.resultSet = fieldDataTemp.columns.tasks;
          break;
      default:
          break;
    }
    return (
      <CardBody>
        <div className="card__title">
          <div className="pageheader">
            <img src="https://d2tqbrn06t95pa.cloudfront.net/img/topnav_reports.png?v=2" /> Query Builder
            </div>
          <p className="card__description">Choose an entity from the available list. Create a query with the attributes available for the corresponding entity. Run the query to see corresponding search result.</p>
        </div>
        <Modal backdrop={"static"} isOpen={this.state.modal} toggle={this.toggle} fade={false} centered={true} className="custom-modal-confirm">
          <ModalHeader toggle={this.toggle}>Alert!</ModalHeader>
          <ModalBody>Your query and result(s) will be lost. Do you wish to proceed?</ModalBody>
          <ModalFooter>
            <button color="primary" onClick={this.confirmEntitySelection}>Continue</button>{' '}
            <button color="secondary" onClick={this.toggle}>Cancel</button>
          </ModalFooter>
        </Modal>
        <Modal backdrop={"static"} isOpen={this.state.isResetModal} toggle={this.toggle} fade={false} centered={true} className="custom-modal-reset">
          <ModalHeader toggle={this.toggle}>Alert!</ModalHeader>
          <ModalBody>Your query and result(s) will be lost. Do you wish to proceed?</ModalBody>
          <ModalFooter>
            <button color="primary" onClick={this.resetQuery}>Continue</button>{' '}
            <button color="secondary" onClick={this.toggle}>Cancel</button>
          </ModalFooter>
        </Modal>
        <div className="grid-container-query-selection">
          <Row>
            <Col xs="2" className="padding-rt-0">
              <Select
                clearable={isClearable}
                isRtl={true}
                onOpen={ this.onOpenClose.bind()}
                isSearchable={false}
                searchable={false}
                openOnClick={false}
                value={selectedOption}
                onChange={this.handleChange}
                options={options}
                backspaceRemoves={false}
                deleteRemoves={false}
                placeholder={""}
                className={"select-entity"}
              />
            </Col>
            <Col xs="auto">
              <button onClick={this.onRunQueryClick} className="query-section-button" size="sm" title="Run Query" aria-label="Run Query">
                <span aria-hidden className=""><i className="fa fa-caret-right"></i></span>
                <span className="fa-text-align">Run Query</span>
              </button>
            </Col>
            <Col xs="auto">
              <button onClick={this.onResetQueryClick} className="query-section-button" size="sm" title="Reset" aria-label="Reset">
                <span aria-hidden className="fa-icon-size" ><i className="fa fa-undo"></i></span>
                <span className="fa-text-align">Reset</span>
              </button>
            </Col>
            <Col xs="auto">
              <button onClick={this.onColumnOptionsClick} className="query-section-button" size="sm" title="Column Options" aria-label="Column Options">
                <span aria-hidden className="fa-icon-size" ><i className="fa fa-undo"></i></span>
                <span className="fa-text-align">Column Options</span>
              </button>
            </Col>
          </Row>
        </div>
        <div className="wrapper">
          <SplitterLayout primaryIndex={0} primaryMinSize={150} secondaryMinSize={200} customClassName={"query-builder-section"} vertical={true}>

                <QueryPane 
                  ref={this.queryPane} 
                  selectedOption={this.state.selectedOption} 
                  passTasksToQuerySection={this.passTasksResults} 
                  passWorkbooksResultsToQuerySection={this.passWorkbookResults} 
                  passEmployeesResultsToQuerySection={this.passEmployeesResults} />

                { this.state.isEmployee && <EmployeeResultSet ref={this.employeeResultSet} employees={this.state.employees}/>}
                { this.state.isWorkbook && <WorkbookResultSet ref={this.workbookResultSet} workbooks={this.state.workbooks}/>}
                { this.state.isTask && <TaskResultSet ref={this.taskResultSet} tasks={this.state.tasks}/>}
                
              </SplitterLayout>
        </div>
        <div>
          <SlidingPane
            columnOptionsSlideToggle = {this.columnOptionsSlideToggle}
            isPaneOpen = {this.state.isPaneOpen}
            columns= {this.state.resultSet}
          />
        </div>
      </CardBody>
    );
  }
}

export default withCookies(QuerySection);