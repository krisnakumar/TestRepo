/* eslint-disable */
import React, { PureComponent } from 'react';
import { Card, CardBody, Col } from 'reactstrap';
import EditTable from '../../../shared/components/table/EditableTable';
import Pagination from '../../../shared/components/pagination/Pagination';

export default class DataTable extends PureComponent {
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
        sortable: true,
      },
      {
        key: 'pastDueWorkBooks',
        name: 'Past Due WorkBooks',
        sortable: true,
      },
      {
        key: 'completedWorkBooks',
        name: 'Completed WorkBook',
        sortable: true,
      },
      {
        key: 'total',
        name: 'Total',
        sortable: true,
      },
    ];

    this.employees = [
        {
          FirstName: 'Shoba',
          LastName: 'Eswar',
          WorkbookName: 'TestWorkbook',
          Role: 'Tester',
          AssignedWorkBooks: 1,
          InDueWorkBooks: 0,
          PastDueWorkBooks: 1,
          CompletedWorkBooks: 0,
          EmployeeCount: 0,
          Code: 0
        },
        {
          FirstName: 'Lee',
          LastName: 'Bush',
          WorkbookName: '',
          Role: '',
          AssignedWorkBooks: 0,
          InDueWorkBooks: 0,
          PastDueWorkBooks: 0,
          CompletedWorkBooks: 0,
          EmployeeCount: 0,
          Code: 0
        }
      ];

    this.state = {
      rows: this.createRows(this.employees),
      pageOfItems: [],
    };
  }
  

  onChangePage = (pageOfItems) => {
    this.setState({ pageOfItems });
  };

  getRandomDate = (start, end) => new Date(start.getTime() + (Math.random() * (end.getTime()
    - start.getTime()))).toLocaleDateString();

  createRows = (employees) => {
    const rows = [],
          length = employees.length;
    for (let i = 0; i < length; i++) {
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
    return rows;
  };

  render() {
    return (
      <Col md={12} lg={12}>
        <Card>
          <CardBody>
            <div className="card__title">
              <h5 className="bold-text">data table</h5>
              <h5 className="subhead">Use table with column's option <span className="red-text">sortable</span></h5>
            </div>
            <p>Show
              <select className="select-options">
                <option value="10">10</option>
                <option value="20">20</option>
                <option value="30">30</option>
              </select>
              entries
            </p>
            <EditTable heads={this.heads} rows={this.state.rows} />
            <Pagination items={this.state.rows} onChangePage={this.onChangePage} />
          </CardBody>
        </Card>
      </Col>
    );
  }
}
