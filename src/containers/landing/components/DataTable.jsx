/* eslint-disable */
import React, { PureComponent } from 'react';
import { Card, CardBody, Col } from 'reactstrap';
import 'whatwg-fetch'
import EditTable from '../../../shared/components/table/EditableTable';
import Pagination from '../../../shared/components/pagination/Pagination';
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import PropTypes from 'prop-types';

export default class DataTable extends PureComponent {
    static propTypes = {
        heads: PropTypes.arrayOf(PropTypes.shape({
          key: PropTypes.string,
          name: PropTypes.string,
          editable: PropTypes.bool,
          sortable: PropTypes.bool
        })).isRequired,
        rows: PropTypes.arrayOf(PropTypes.shape()).isRequired,
      };

  constructor() {
    super();
    this.heads = [
      {
        key: 'employee',
        name: 'Employee',
        sortable: true,
      },
      {
        key: 'role',
        name: 'Role',
        sortable: true,
      },
      {
        key: 'assignedWorkBooks',
        name: 'Assigned WorkBook',
        sortable: true,
      },
      {
        key: 'inDueWorkBooks',
        name: 'WorkBook Due',
        sortable: true
      },
      {
        key: 'pastDueWorkBooks',
        name: 'Past Due WorkBooks',
        sortable: true
      },
      {
        key: 'completedWorkBooks',
        name: 'Completed WorkBook',
        sortable: true
      },
      {
        key: 'total',
        name: 'Total',
        sortable: true
      },
    ];

    this.employees = [];

    this.state = {
      rows: this.createRows(this.employees),
      pageOfItems: [],
    };
  }
  
  componentDidMount = () => {
      this.getEmployees(6);
  };

  getEmployees = (userId) => {
    let _self = this,
         url = "https://pmo427as84.execute-api.us-west-2.amazonaws.com/dev/users/"+ userId +"/employees";
    fetch(url)
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
        employee: employees[i].FirstName + ' ' + employees[i].LastName,
        role: employees[i].Role,
        assignedWorkBooks: employees[i].AssignedWorkBooks,
        inDueWorkBooks: employees[i].InDueWorkBooks,
        pastDueWorkBooks: employees[i].PastDueWorkBooks,
        completedWorkBooks: employees[i].CompletedWorkBooks,
        total: employees[i].EmployeeCount,
      });
    }
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

    const sortRows = this.state.rows.slice(0);
    const rows = sortDirection === 'NONE' ? this.state.rows.slice(0, 10) : sortRows.sort(comparer).slice(0, 10);

    this.setState({ rows });
  };

  rowGetter = i => this.state.rows[i];
  render() {
      const { rows } = this.state;
    return (
          <CardBody>
            <div className="card__title">
             <div className="pageheader">
              <img src="https://d2tqbrn06t95pa.cloudfront.net/img/topnav_reports.png?v=2"/> Workbook Dashboard
            </div>
            </div>
            <div class="grid-container">
              <p id="EntryCount">Show
                <select className="select-options">
                  <option value="10">10</option>
                  <option value="20">20</option>
                  <option value="30">30</option>
                </select>
                entries
              </p>
              <div className="table">
                  <ReactDataGrid
                      onGridSort={this.handleGridSort}
                      enableCellSelect
                      columns={this.heads}
                      rowGetter={this.rowGetter}
                      rowsCount={rows.length}
                      onGridRowsUpdated={this.handleGridRowsUpdated}
                      rowHeight={44}
                      minColumnWidth={100}
                  />
              </div>
            </div>
            {/* <Pagination items={rows} onChangePage={this.onChangePage} /> */}
          </CardBody>
    );
  }
}
