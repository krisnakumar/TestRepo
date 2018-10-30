/* eslint-disable */
import React, { PureComponent } from 'react';
import { Card, CardBody, Col, Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import MyEmployees from './MyEmployees';
import AssignedWorkBook from './AssignedWorkBook';
import Loader from '../../_layout/loader/Loader';

import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';


class DataTableEmptyRowsView extends React.Component{

  render() {
    return (<div className="no-records-found">Sorry, no records</div>)
  }
};

class WorkBookDashboard extends PureComponent {

  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor() {
    super();
    this.heads = [
      {
        key: 'employee',
        name: 'Employee',
        sortable: true,
        width: 300,
        editable: false,
        cellClass: "text-left text-clickable"
      },
      {
        key: 'role',
        name: 'Role',
        sortable: true,
        editable: false,
        cellClass: "text-left"
      },
      {
        key: 'assignedWorkBooks',
        name: 'Assigned WorkBook',
        sortable: true,
        editable: false,
        cellClass: "text-right text-clickable"
      },
      {
        key: 'inDueWorkBooks',
        name: 'WorkBook Due',
        sortable: true,
        editable: false,
        cellClass: "text-right"
      },
      {
        key: 'pastDueWorkBooks',
        name: 'Past Due WorkBooks',
        sortable: true,
        editable: false,
        cellClass: "text-right"
      },
      {
        key: 'completedWorkBooks',
        name: 'Completed WorkBook',
        sortable: true,
        editable: false,
        cellClass: "text-right text-clickable"
      },
      {
        key: 'total',
        name: 'Total',
        sortable: true,
        editable: false,
        cellClass: "text-right text-clickable last-column"
      },
    ];

    this.employees = [];

    this.state = {
      rows: this.createRows(this.employees),
      pageOfItems: [],
      levelTwoWB: false,
      level3WB: false,
      myEmployees: {},
      assignedWorkBooks: {}
    };
  }


  componentDidCatch(error, info) {
    // Display fallback UI
    //this.setState({ hasError: true });
    // You can also log the error to an error reporting service
    console.log(error, info);
  }

  
  updateModalState = (modelName) => {
    let value = !this.state[modelName];
    this.setState({
      [modelName]: value
    });
  };

  componentDidMount = () => {
    this.getEmployees(6);
  };

  getEmployees = (userId) => {
    const { cookies } = this.props;
    let _self = this,
         url = "https://fp34gqm7i7.execute-api.us-west-2.amazonaws.com/test/users/"+ userId +"/employees";
    fetch(url,  {headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      "Authorization": cookies.get('IdentityToken')
    }})
    .then(function(response) {
      return response.json()
    }).then(function(json) { 
        let rows = _self.createRows(json);
        _self.setState({ rows: rows});
        _self.onChangePage([]);
        return json
    }).catch(function(ex) {
      console.log('parsing failed', ex)
    })
  };

  getMyEmployees = (userId) => {
    const { cookies } = this.props;
    let _self = this,
        url = "https://fp34gqm7i7.execute-api.us-west-2.amazonaws.com/test/users/"+ userId +"/employees";
    fetch(url,  {headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      "Authorization": cookies.get('IdentityToken')
    }})
    .then(function(response) {
      return response.json()
    }).then(function(json) { 
        let myEmployees = json,
        levelTwoWB = _self.state.levelTwoWB;
        levelTwoWB = true;
        _self.setState({ ..._self.state, levelTwoWB, myEmployees });
        return json
    }).catch(function(ex) {
      console.log('parsing failed', ex)
    })
  };

  getAssignedWorkbooks = (userId) => {
    const { cookies } = this.props;
    let _self = this,
        url = "https://fp34gqm7i7.execute-api.us-west-2.amazonaws.com/test/users/"+ userId +"/workbooks/assigned";
    fetch(url,  {headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      "Authorization": cookies.get('IdentityToken')
    }})
    .then(function(response) {
      return response.json()
    }).then(function(json) { 
        let assignedWorkBooks = json,
        level3WB = _self.state.level3WB;
        level3WB = true;
        _self.setState({ ..._self.state, level3WB, assignedWorkBooks });
        return json
    }).catch(function(ex) {
      console.log('parsing failed', ex)
    })
  };

  onChangePage = (pageOfItems) => {
    this.setState({ pageOfItems });
  };

  getRandomDate = (start, end) => new Date(start.getTime() + (Math.random() * (end.getTime()
    - start.getTime()))).toLocaleDateString();

  createRows = (employees) => {
    var assignedWorkBooksCount = 0;
    var inDueWorkBooksCount = 0;
    var pastDueWorkBooksCount = 0;
    var completedWorkBooksCount = 0;
    var totalEmpCount = 0;
    const rows = [], 
          length = employees ? employees.length : 0;
    for (let i = 0; i < length; i++) {
      assignedWorkBooksCount += parseInt(employees[i].AssignedWorkBooks);
      inDueWorkBooksCount += parseInt(employees[i].InDueWorkBooks);
      pastDueWorkBooksCount += parseInt(employees[i].PastDueWorkBooks);
      completedWorkBooksCount += parseInt(employees[i].CompletedWorkBooks);
      totalEmpCount += parseInt(employees[i].EmployeeCount)
      rows.push({
        userId: employees[i].UserId,
        employee: employees[i].FirstName + ' ' + employees[i].LastName,
        role: employees[i].Role,
        assignedWorkBooks: employees[i].AssignedWorkBooks,
        inDueWorkBooks: employees[i].InDueWorkBooks,
        pastDueWorkBooks: employees[i].PastDueWorkBooks,
        completedWorkBooks: employees[i].CompletedWorkBooks,
        total: employees[i].EmployeeCount,
      });
    }

    if(length > 0)
    rows.push({employee: "Total", role: "", assignedWorkBooks:assignedWorkBooksCount, inDueWorkBooks: inDueWorkBooksCount , pastDueWorkBooks:pastDueWorkBooksCount, completedWorkBooks:completedWorkBooksCount, total:totalEmpCount});

    return rows;
  };

  handleGridRowsUpdated = ({ fromRow, toRow, updated }) => {
    const rows = this.state.rows.slice();

    for (let i = fromRow; i <= toRow; i += 1) {
      const rowToUpdate = rows[i];
      rows[i] = update(rowToUpdate, { $merge: updated });
    }

    this.setState({ rows });
  };

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

  handleCellFocus = (args) => {
    if(args.idx == 0 || args.idx == 6){
      let userId = this.state.rows[args.rowIdx].userId;

      if(userId)
      this.getMyEmployees(userId);      
    } else if(args.idx == 2){
      let userId = this.state.rows[args.rowIdx].userId;

      if(userId)
      this.getAssignedWorkbooks(userId);
    }
    this.refs.reactDataGrid.deselect();
  };

  rowGetter = i => this.state.rows[i];

  render() {
      const { rows } = this.state;
    return (         
          <CardBody>
            {/* <Loader/>  */}
            <MyEmployees
              updateState={this.updateModalState.bind(this)}
              modal={this.state.levelTwoWB}
              myEmployees={this.state.myEmployees}
            />
            <AssignedWorkBook
              updateState={this.updateModalState.bind(this)}
              modal={this.state.level3WB}
              assignedWorkBooks={this.state.assignedWorkBooks}
            />
            <div className="card__title">
             <div className="pageheader">
              <img src="https://d2tqbrn06t95pa.cloudfront.net/img/topnav_reports.png?v=2"/> Workbook Dashboard
            </div>
            </div>
            <div className="grid-container">
              <div className="table">
                  <ReactDataGrid
                      ref={'reactDataGrid'}
                      onGridSort={this.handleGridSort}
                      enableCellSelect={false}
                      enableCellAutoFocus={false}
                      columns={this.heads}
                      rowGetter={this.rowGetter}
                      rowsCount={rows.length}
                      onGridRowsUpdated={this.handleGridRowsUpdated}
                      rowHeight={44}
                      minColumnWidth={100}
                      onCellSelected={(args) => { this.handleCellFocus(args) }}
                      emptyRowsView={DataTableEmptyRowsView} 
                  />
              </div>
            </div>
          </CardBody>
    );
  }
}

export default withCookies(WorkBookDashboard);