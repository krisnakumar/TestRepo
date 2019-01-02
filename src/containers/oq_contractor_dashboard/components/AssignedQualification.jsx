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
import { CardBody, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';

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
                taskCode: qualifications[i].taskCode,
                oQTask: qualifications[i].oQTask,
                employee: qualifications[i].employee,
                assignedDate: qualifications[i].assignedDate
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
                return (a[sortColumn] > b[sortColumn]) ? 1 : -1;
            } else if (sortDirection === 'DESC') {
                return (a[sortColumn] < b[sortColumn]) ? 1 : -1;
            }
        };

        const sortRows = this.state.rows.slice(0),
              rowsLength = this.state.rows.length || 0;
        const rows = sortDirection === 'NONE' ? this.state.rows.slice(0, rowsLength) : sortRows.sort(comparer).slice(0, rowsLength);

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
            default:
                console.log("default-", type, args);
                break;
        }
        this.refs.assignedQualificationReactDataGrid.deselect();
    };

    render() {
        const { rows } = this.state;
        return (
            <div>
                <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid">
                    <ModalHeader toggle={this.toggle}>Assigned Qualification</ModalHeader>
                    <ModalBody>
                        <div className="grid-container">
                            <div className="table">
                                <ReactDataGrid
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