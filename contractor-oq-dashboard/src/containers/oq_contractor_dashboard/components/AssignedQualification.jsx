/* eslint-disable */
/*
* AssignedQualification.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details to list the workbooks 
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------
createRows(employees)
handleGridRowsUpdated(fromRow, toRow, updated)
handleGridSort(sortColumn, sortDirection)
*/
import React, { PureComponent } from 'react';
import { Modal, ModalHeader, ModalBody } from 'reactstrap';
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import Export from './OQDashboardExport';
import _ from "lodash";

// Import React Table
import ReactTable from "react-table";
import "react-table/react-table.css";

/**
 * AssignedQualificationEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-data-grid module.
 */
class AssignedQualificationEmptyRowsView extends React.Component {
    render() {
        return (<div className="no-records-found-modal">Sorry, no records</div>)
    }
};

class AssignedQualification extends PureComponent {

    static propTypes = {
        cookies: instanceOf(Cookies).isRequired
    };

    constructor(props) {
        super(props);
        this.heads = [
            {
                key: 'taskCode',
                name: 'Task Code',
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: this.cellFormatter,
                cellClass: "text-left"
            },
            {
                key: 'oQTask',
                name: 'OQ Task',
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: this.cellFormatter,
                cellClass: "text-left"
            },
            {
                key: 'employee',
                name: 'Employee',
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: this.cellFormatter,
                cellClass: "text-left"
            },
            {
                key: 'assignedDate',
                name: 'Assigned Date',
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: this.cellFormatter,
                cellClass: "text-center last-column"
            }
        ];

        this.state = {
            rows: this.createRows(this.props.assignedQualifications),
            modal: this.props.modal,
            isInitial: false,
        };

        this.toggle = this.toggle.bind(this);
        this.customCell = this.customCell.bind(this);
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
            <span>{props.value}</span>
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
    * @param qualifications
    * @returns rows
    */
    createRows = (qualifications) => {
        const rows = [],
            length = qualifications ? qualifications.length : 0;
        for (let i = 0; i < length; i++) {
            rows.push({
                taskCode: qualifications[i].TaskCode,
                oQTask: qualifications[i].TaskName,
                employee: qualifications[i].EmployeeName,
                assignedDate: qualifications[i].AssignedDate
            });
        }
        return rows;
    };

    /**
     * @method
     * @name - componentWillReceiveProps
     * This method will invoked whenever the props or state
     * is update to this component class
     * @param newProps
     * @returns none
    */
    componentWillReceiveProps(newProps) {
        let rows = this.createRows(newProps.assignedQualifications),
            isArray = Array.isArray(newProps.assignedQualifications),
            isInitial = isArray;
        this.setState({
            modal: newProps.modal,
            rows: rows,
            isInitial: isInitial
        });
    }

    /**
     * @method
     * @name - toggle
     * This method will update the current of modal window
     * @param workbooks
     * @returns none
     */
    toggle() {
        this.setState({
            modal: !this.state.modal
        });
        this.props.updateState("isAssignedQualificationView");
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

    customCell(props) {
        let self = this;
        return (
            props.value && <span onClick={e => { e.preventDefault(); self.handleCellClick(props.column.id, props.original); }} className={"text-clickable"}>
                {props.value}
            </span> || <span>{props.value}</span>
        );
    }

    // This method is used to setting the row data in react data grid
    rowGetter = i => this.state.rows[i];

    render() {
        const { rows } = this.state;
        let pgSize = (rows.length > 10) ? rows.length : 10;
        return (
            <div>
                <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid grid-modal-popup">
                    <ModalHeader className="text-left" toggle={this.toggle}>
                        Assigned Qualifications
                    <p className="section-info-description">Shows the qualification detail of the chosen company</p>
                    </ModalHeader>
                    <div>
                        <div className="export-menu-one">

                        </div>
                        <div className="export-menu-two">
                            <Export
                                data={this.state.rows}
                                heads={this.heads}
                                sheetName={"Assigned Qualifications"}
                            />
                        </div>
                    </div>
                    <ModalBody>
                        <div className="grid-container">
                            <div className="table">
                                {/* <ReactDataGrid
                                    ref={'assignedQualificationReactDataGrid'}
                                    onGridSort={this.handleGridSort}
                                    enableCellSelect={false}
                                    enableCellAutoFocus={false}
                                    columns={this.heads}
                                    rowGetter={this.rowGetter}
                                    rowsCount={rows.length}
                                    onGridRowsUpdated={this.handleGridRowsUpdated}
                                    rowHeight={35}
                                    minColumnWidth={100}
                                    emptyRowsView={this.state.isInitial && AssignedQualificationEmptyRowsView}
                                /> */}
                                <ReactTable
                                    minRows={1}
                                    data={rows}
                                    columns={[
                                        {
                                            Header: "Task Code",
                                            accessor: "taskCode",
                                            headerClassName: 'header-wordwrap',
                                            minWidth: 120,
                                            maxWidth: 200,
                                            className: 'text-left'
                                        },
                                        {
                                            Header: "OQ Task",
                                            accessor: "oQTask",
                                            headerClassName: 'header-wordwrap',
                                            minWidth: 250,
                                            className: 'text-left'
                                        },
                                        {
                                            Header: "Employee",
                                            id: "employee",
                                            accessor: d => d.employee,
                                            headerClassName: 'header-wordwrap',
                                            minWidth: 100,
                                            maxWidth: 300,
                                            className: 'text-left'
                                        },
                                        {
                                            Header: "Assigned Date",
                                            accessor: "assignedDate",
                                            headerClassName: 'header-wordwrap',
                                            minWidth: 100,
                                            maxWidth: 150,
                                            className: 'text-center'
                                        }
                                    ]
                                    }
                                    resizable={false}
                                    className="-striped -highlight"
                                    showPagination={false}
                                    showPaginationTop={false}
                                    showPaginationBottom={false}
                                    showPageSizeOptions={false}
                                    pageSizeOptions={[5, 10, 20, 25, 50, 100]}
                                    pageSize={!this.state.isInitial ? 5 : pgSize}
                                    loading={!this.state.isInitial}
                                    loadingText={''}
                                    noDataText={!this.state.isInitial ? '' : 'Sorry, no records'}
                                    // defaultSorted={[
                                    //   {
                                    //     id: "role",
                                    //     desc: false
                                    //   }
                                    // ]}
                                    style={{
                                        maxHeight: "550px"
                                    }}
                                />
                            </div>
                        </div>
                    </ModalBody>
                </Modal>
            </div>
        );
    }
}

export default withCookies(AssignedQualification);