/* eslint-disable */
/*
* InCompletedQualification.jsx
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
import { CardBody, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import Export from './OQDashboardExport';

/**
 * InCompletedQualificationEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-data-grid module.
 */
class InCompletedQualificationEmptyRowsView extends React.Component {
    render() {
        return (<div className="no-records-found-modal">Sorry, no records</div>)
    }
};

class InCompletedQualification extends PureComponent {

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
            rows: this.createRows(this.props.inCompletedQualifications),
            modal: this.props.modal,
            isInitial: false,
        };

        this.toggle = this.toggle.bind(this);
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
        let rows = this.createRows(newProps.inCompletedQualifications),
            isArray = Array.isArray(newProps.inCompletedQualifications),
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
        this.props.updateState("isInCompletedQualificationView");
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

    // This method is used to setting the row data in react data grid
    rowGetter = i => this.state.rows[i];

    render() {
        const { rows } = this.state;
        return (
            <div>
                <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid">
                    <ModalHeader toggle={this.toggle}>Disqualifications</ModalHeader>
                    <Export
                        data={this.state.rows}
                        heads={this.heads}
                        sheetName={"Disqualifications"}
                    />
                    <ModalBody>
                        <div className="grid-container">
                            <div className="table">
                                <ReactDataGrid
                                    ref={'completedQualificationEmptyRowsView'}
                                    onGridSort={this.handleGridSort}
                                    enableCellSelect={false}
                                    enableCellAutoFocus={false}
                                    columns={this.heads}
                                    rowGetter={this.rowGetter}
                                    rowsCount={rows.length}
                                    onGridRowsUpdated={this.handleGridRowsUpdated}
                                    rowHeight={35}
                                    minColumnWidth={100}
                                    emptyRowsView={this.state.isInitial && InCompletedQualificationEmptyRowsView} 
                                />
                            </div>
                        </div>
                    </ModalBody>
                </Modal>
            </div>
        );
    }
}

export default withCookies(InCompletedQualification);