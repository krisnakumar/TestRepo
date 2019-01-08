/* eslint-disable */
/*
* EmployeeView.jsx
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

/**
 * EmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-data-grid module.
 */
class EmployeeViewEmptyRowsView extends React.Component {
    render() {
        return (<div className="no-records-found-modal">Sorry, no records</div>)
    }
};

class EmployeeView extends PureComponent {

    static propTypes = {
        cookies: instanceOf(Cookies).isRequired
    };

    constructor(props) {
        super(props);
        this.heads = [
            {
                key: 'employee',
                name: 'Employee',
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: (props) => this.qualificationsFormatter("total", props),
                cellClass: "text-left"
            },
            {
                key: 'assignedQualification',
                name: 'Assigned Qualification',
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: (props) => this.qualificationsFormatter("assignedQualification", props),
                cellClass: "text-right"
            },
            {
                key: 'completedQualification',
                name: 'Completed Qualification',
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: (props) => this.qualificationsFormatter("completedQualification", props),
                cellClass: "text-right"
            },
            {
                key: 'inCompletedQualification',
                name: 'Incomplete Qualification',
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: (props) => this.qualificationsFormatter("inCompletedQualification", props),
                cellClass: "text-right"
            },
            {
                key: 'pastDue',
                name: 'Past Due (30 Days)',
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: (props) => this.qualificationsFormatter("pastDue", props),
                cellClass: "text-right"
            },
            {
                key: 'comingDue',
                name: 'Due in 30 Days',
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: (props) => this.qualificationsFormatter("comingDue", props),
                cellClass: "text-right"
            },
            {
                key: 'total',
                name: 'Total Employees',
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: (props) => this.qualificationsFormatter("total", props),
                cellClass: "text-right last-column"
            },
        ];

        this.state = {
            rows: this.createRows(this.props.employeeQualifications),
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
                employee: qualifications[i].contractors,
                role: qualifications[i].role,
                assignedQualification: qualifications[i].assignedQualification,
                completedQualification: qualifications[i].completedQualification,
                inCompletedQualification: qualifications[i].inCompletedQualification,
                pastDue: qualifications[i].pastDue,
                comingDue: qualifications[i].comingDue,
                total: qualifications[i].total,
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
        let rows = this.createRows(newProps.employeeQualifications),
            isArray = Array.isArray(newProps.employeeQualifications),
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
        this.props.updateState("isEmployeeView");
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
                return (a[sortColumn] > b[sortColumn]) ? 1 : -1;
            } else if (sortDirection === 'DESC') {
                return (a[sortColumn] < b[sortColumn]) ? 1 : -1;
            }
        };

        const sortRows = this.state.rows.slice(0);
        const rows = sortDirection === 'NONE' ? this.state.rows.slice(0, 10) : sortRows.sort(comparer).slice(0, 10);

        this.setState({ rows });
    };

    // This method is used to setting the row data in react data grid
    rowGetter = i => this.state.rows[i];

    /**
     * @method
     * @name - qualificationsFormatter
     * This method will format the qualification Data Grid
     * @param type
     * @param props
     * @returns none
     */
    qualificationsFormatter = (type, props) => {
        if (props.dependentValues[type] <= 0 || props.dependentValues.contractors == "Total") {
            return (
                <span>{props.value}</span>
            );
        } else {
            return (
                <span onClick={e => { e.preventDefault(); this.handleCellClick(type, props.dependentValues); }} className={"text-clickable"}>
                    {props.value}
                </span>
            );
        }
    }

    /**
    * @method
    * @name - handleCellClick
    * This method will trigger the event of API's respective to cell clicked Data Grid
    * @param type
    * @param args
    * @returns none
    */
    handleCellClick = (type, args) => {
        let userId = 0;
        switch (type) {
            case "contractors":
            case "total":
                alert("Work in progress!");
                console.log("contractors-", type, args);
                break;
            case "assignedQualification":
                alert("Work in progress!");
                console.log("assignedQualification-", type, args);
                break;
            case "completedQualification":
                alert("Work in progress!");
                console.log("completedQualification-", type, args);
                break;
            case "inCompletedQualification":
                alert("Work in progress!");
                console.log("inCompletedQualification-", type, args);
                break;
            case "pastDue":
                alert("Work in progress!");
                console.log("pastDue-", type, args);
                break;
            case "comingDue":
                alert("Work in progress!");
                console.log("comingDue-", type, args);
                break;
            default:
                console.log("default-", type, args);
                break;
        }
        this.refs.employeeViewReactDataGrid.deselect();
    };

    render() {
        const { rows } = this.state;
        return (
            <div>
                <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid">
                    <ModalHeader toggle={this.toggle}>Employee View</ModalHeader>
                    <ModalBody>
                        <div className="grid-container">
                            <div className="table">
                                <ReactDataGrid
                                    ref={'employeeViewReactDataGrid'}
                                    onGridSort={this.handleGridSort}
                                    enableCellSelect={false}
                                    enableCellAutoFocus={false}
                                    columns={this.heads}
                                    rowGetter={this.rowGetter}
                                    rowsCount={rows.length}
                                    onGridRowsUpdated={this.handleGridRowsUpdated}
                                    rowHeight={35}
                                    minColumnWidth={100}
                                    // emptyRowsView={this.state.isInitial && WorkBookRepetitionEmptyRowsView} 
                                />
                            </div>
                        </div>
                    </ModalBody>
                </Modal>
            </div>
        );
    }
}

export default withCookies(EmployeeView);