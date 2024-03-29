/* eslint-disable */
/*
* PastDueQualification.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Past due qualifications details on COQ Dashboard
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
class PastDueQualification extends PureComponent {
    constructor(props) {
        super(props);
        this.heads = [
            {
                key: 'taskCode',
                name: 'Task Code',
                sortable: true,
                editable: false,
                formatter: this.cellFormatter,
                cellClass: "text-left"
            },
            {
                key: 'oQTask',
                name: 'OQ Task',
                sortable: true,
                editable: false,
                formatter: this.cellFormatter,
                cellClass: "text-left"
            },
            {
                key: 'employee',
                name: 'Employee',
                sortable: true,
                editable: false,
                formatter: this.cellFormatter,
                cellClass: "text-left"
            },
            {
                key: 'dueDate',
                name: 'Expiration Date',
                sortable: true,
                editable: false,
                formatter: this.cellFormatter,
                cellClass: "text-center last-column"
            }
        ];

        this.state = {
            rows: this.createRows(this.props.pastDueQualifications),
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
                dueDate: qualifications[i].ExpirationDate
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
        let rows = this.createRows(newProps.pastDueQualifications),
            isArray = Array.isArray(newProps.pastDueQualifications),
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
        this.props.updateState("isPastDueQualificationView");
    }

    customCell(props) {
        let self = this;
        return (
            props.value && <span onClick={e => { e.preventDefault(); self.handleCellClick(props.column.id, props.original); }} className={"text-clickable"}>
                {props.value}
            </span> || <span>{props.value}</span>
        );
    };

    render() {
        const { rows } = this.state;
        let pgSize = (rows.length > 10) ? rows.length : 10;
        return (
            <div>
                <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid grid-modal-popup">
                    <ModalHeader toggle={this.toggle}>Expired Qualifications</ModalHeader>
                    <div>
                        <div className="export-menu-one">

                        </div>
                        <div className="export-menu-two">
                            <Export
                                data={this.state.rows}
                                heads={this.heads}
                                sheetName={"Expired Qualifications"}
                            />
                        </div>
                    </div>
                    <ModalBody>
                        <div className="grid-container">
                            <div className="table">
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
                                            accessor: "employee",
                                            headerClassName: 'header-wordwrap',
                                            minWidth: 100,
                                            maxWidth: 300,
                                            className: 'text-left'
                                        },
                                        {
                                            Header: "Expiration Date",
                                            accessor: "dueDate",
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

export default PastDueQualification;