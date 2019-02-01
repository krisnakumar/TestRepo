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
import { Modal, ModalHeader, ModalBody } from 'reactstrap';
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import OQDashboardMock from '../components/OQDashboardMock.json';
import AssignedQualification from '../components/AssignedQualification';
import CompletedQualification from '../components/CompletedQualification';
import InCompletedQualification from '../components/InCompletedQualification';
import PastDueQualification from '../components/PastDueQualification';
import ComingDueQualification from '../components/ComingDueQualification';
import * as API from '../../../shared/utils/APIUtils';
import * as Constants from '../../../shared/constants';

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
                key: 'role',
                name: 'Role',
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: this.cellFormatter,
                cellClass: "text-left"
            },
            {
                key: 'assignedQualification',
                name: 'Assigned Qualifications',
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: (props) => this.qualificationsFormatter("assignedQualification", props),
                cellClass: "text-right"
            },
            {
                key: 'completedQualification',
                name: 'Qualifications',
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: (props) => this.qualificationsFormatter("completedQualification", props),
                cellClass: "text-right"
            },
            {
                key: 'inCompletedQualification',
                name: 'Disqualifications',
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: (props) => this.qualificationsFormatter("inCompletedQualification", props),
                cellClass: "text-right"
            },
            {
                key: 'pastDue',
                name: 'Expired Qualifications (30 Days)',
                width: 200,
                sortable: true,
                editable: false,
                getRowMetaData: row => row,
                formatter: (props) => this.qualificationsFormatter("pastDue", props),
                cellClass: "text-right"
            },
            {
                key: 'comingDue',
                name: 'Expires in 30 Days',
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
            rows: this.createRows(this.props.employeesQualificationsArray || []),
            modal: this.props.modal,
            isInitial: false,
            sortColumn: "",
            sortDirection: "NONE",
            contractorsNames: this.props.contractorsNames || [],
            employeesQualificationsArray: this.props.employeesQualificationsArray || [],
            isAssignedQualificationView: false,
            isCompletedQualificationView: false,
            isInCompletedQualificationView: false,
            isPastDueQualificationView: false,
            isComingDueQualificationView: false,
            assignedQualifications: {},
            completedQualifications: {},
            inCompletedQualifications: {},
            pastDueQualifications: {},
            comingDueQualifications: {}
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
    createRows = (qualificationsArray) => {
        let qualificationsArrayLength = qualificationsArray.length - 1;
        let qualifications = qualificationsArray[qualificationsArrayLength];
        const rows = [],
            length = qualifications ? qualifications.length : 0;
        for (let i = 0; i < length; i++) {
            rows.push({
                userId: qualifications[i].UserId,
                companyId: qualifications[i].CompanyId,
                employee: qualifications[i].EmployeeName,
                role: qualifications[i].Role || "",
                assignedQualification: qualifications[i].AssignedQualification,
                completedQualification: qualifications[i].CompletedQualification,
                inCompletedQualification: qualifications[i].IncompleteQualification,
                pastDue: qualifications[i].PastDueQualification,
                comingDue: qualifications[i].InDueQualification,
                total: qualifications[i].TotalEmployees,
            });
        }
        
        if (length > 0) {
            this.state.employeesQualificationsArray = qualificationsArray;
        }

        return rows;
    };

    /**
    * @method
    * @name - updateModalState
    * This method will update the modal window state of parent
    * @param modelName
    * @returns none
   */
    updateModalState = (modelName) => {
        let value = !this.state[modelName];
        this.setState({
            [modelName]: value
        });
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
        const { sortColumn, sortDirection } = this.state;
        let rows = this.createRows(newProps.employeesQualificationsArray || []),
            isArray = Array.isArray(newProps.employeesQualificationsArray || []),
            isRows = newProps.employeesQualificationsArray.length > 0 ? true : false;

        let isInitial = false;

        if (isArray && isRows) {
            isInitial = rows.length > 0 ? false : true;
        }
        
        if(sortColumn != "" && sortDirection != "NONE"){
            this.state.modal = newProps.modal;
            this.state.rows = rows;
            this.state.isInitial = isInitial;
            this.state.contractorsNames = newProps.contractorsNames || [];
            this.handleGridSort(sortColumn, sortDirection);
        } else {
            this.setState({
                modal: newProps.modal,
                rows: rows,
                isInitial: isInitial,
                contractorsNames: newProps.contractorsNames || []
            });
        }
    };

    /**
     * @method
     * @name - toggle
     * This method will update the current of modal window
     * @param workbooks
     * @returns none
     */
    toggle() {
        let employeesQualificationsArray = this.state.employeesQualificationsArray,
            length = employeesQualificationsArray.length;

        if (length == 1 || length == 0 || length == undefined) {
            this.setState({
                modal: !this.state.modal
            });
            this.props.updateState("isEmployeeView");
        } else if (length >= 1) {
            this.props.popEmployeesQualificationsArray();
        }
    };

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
        this.state.sortColumn = sortColumn;
        this.state.sortDirection = sortDirection;

        const comparer = (a, b) => {
            if (sortDirection === 'ASC') {
                return (a[sortColumn] >= b[sortColumn]) ? 1 : -1;
            } else if (sortDirection === 'DESC') {
                return (a[sortColumn] <= b[sortColumn]) ? 1 : -1;
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
        let userId = args.userId || 0,
            companyId = args.companyId || 0;
        switch (type) {
            case "contractors":
            case "total":
                this.getMyEmployees(userId, companyId, args);
                break;
            case "assignedQualification":
                this.getAssignedQualifications(userId, companyId);
                break;
            case "completedQualification":
                this.getCompletedQualifications(userId, companyId);
                break;
            case "inCompletedQualification":
                this.getInCompletedQualifications(userId, companyId);
                break;
            case "pastDue":
                this.getPastDueQualifications(userId, companyId);
                break;
            case "comingDue":
                this.getComingDueQualifications(userId, companyId);
                break;
            default:
                console.log("default-", type, args);
                break;
        }
        this.refs.employeeViewReactDataGrid.deselect();
    };

     /**
     * @method
     * @name - getMyEmployees
     * This method will used to get My Employees details supervisior
     * @param userId
     * @param supervisor
     * @returns none
    */
    async getMyEmployees(userId, companyId, args) {
        const { cookies } = this.props;
        const payLoad = {
            "Fields": [
                { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }
            ],
            "ColumnList": Constants.GET_EMPLOYEE_QUALIFICATION_COLUMNS
        };

        let token = cookies.get('IdentityToken'),
            url = "/company/" + companyId + "/tasks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true),
            myEmployees = response;

        this.props.updateEmployeesQualificationsArray(myEmployees, args);
    };

    /**
     * @method
     * @name - getAssignedQualifications
     * This method will used to get Assigned Qualifications
     * @param userId
     * @returns none
    */
    async getAssignedQualifications(userId, companyId) {
        const { cookies } = this.props;
        const payLoad = {
            "Fields": [
                { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" }
            ],
            "ColumnList": Constants.GET_ASSIGNED_QUALIFICATION_COLUMNS
        };

        let isAssignedQualificationView = this.state.isAssignedQualificationView,
            assignedQualifications = {};
        isAssignedQualificationView = true;
        this.setState({ isAssignedQualificationView, assignedQualifications });

        let token = cookies.get('IdentityToken'),
            url = "/company/" + companyId + "/tasks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);
        assignedQualifications = response;
        isAssignedQualificationView = true;
        this.setState({ ...this.state, isAssignedQualificationView, assignedQualifications });
    };

    /**
    * @method
    * @name - getCompletedQualifications
    * This method will used to get Completed Qualifications
    * @param userId
    * @returns none
    */
    async getCompletedQualifications(userId, companyId) {
        const { cookies } = this.props;
        const payLoad = {
            "Fields": [
                { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" },
                { "Name": "COMPLETED", "Bitwise": "and", "Value": "true", "Operator": "=" }
            ],
            "ColumnList": Constants.GET_COMPLETED_QUALIFICATION_COLUMNS
        };

        let isCompletedQualificationView = this.state.isCompletedQualificationView,
            completedQualifications = {};
        isCompletedQualificationView = true;
        this.setState({ isCompletedQualificationView, completedQualifications });

        let token = cookies.get('IdentityToken'),
            url = "/company/" + companyId + "/tasks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);
        completedQualifications = response;
        isCompletedQualificationView = true;
        this.setState({ ...this.state, isCompletedQualificationView, completedQualifications });
    };

    /**
    * @method
    * @name - getInCompletedQualifications
    * This method will used to get InCompleted Qualifications
    * @param userId
    * @returns none
    */
    async getInCompletedQualifications(userId, companyId) {
        const { cookies } = this.props;
        const payLoad = {
            "Fields": [
                { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" },
                { "Name": "IN_COMPLETE", "Bitwise": "and", "Value": "true", "Operator": "=" }
            ],
            "ColumnList": Constants.GET_IN_COMPLETED_QUALIFICATION_COLUMNS
        };

        let isInCompletedQualificationView = this.state.isInCompletedQualificationView,
            inCompletedQualifications = {};
        isInCompletedQualificationView = true;
        this.setState({ isInCompletedQualificationView, inCompletedQualifications });

        let token = cookies.get('IdentityToken'),
            url = "/company/" + companyId + "/tasks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);
        inCompletedQualifications = response;
        isInCompletedQualificationView = true;
        this.setState({ ...this.state, isInCompletedQualificationView, inCompletedQualifications });
    };

    /**
    * @method
    * @name - getPastDueQualifications
    * This method will used to get Past Due Qualifications
    * @param userId
    * @returns none
    */
    async getPastDueQualifications(userId, companyId) {
        const { cookies } = this.props;
        const payLoad = {
            "Fields": [
                { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" },
                { "Name": "PAST_DUE", "Bitwise": "and", "Value": "30", "Operator": "=" }
            ],
            "ColumnList": Constants.GET_PAST_DUE_QUALIFICATION_COLUMNS
        };

        let isPastDueQualificationView = this.state.isPastDueQualificationView,
            pastDueQualifications = {};
        isPastDueQualificationView = true;
        this.setState({ isPastDueQualificationView, pastDueQualifications });

        let token = cookies.get('IdentityToken'),
            url = "/company/" + companyId + "/tasks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);
        pastDueQualifications = response;
        isPastDueQualificationView = true;
        this.setState({ ...this.state, isPastDueQualificationView, pastDueQualifications });
    };

    /**
    * @method
    * @name - getComingDueQualifications
    * This method will used to get Coming due qualifications
    * @param userId
    * @returns none
    */
    async getComingDueQualifications(userId, companyId) {
        const { cookies } = this.props;
        const payLoad = {
            "Fields": [
                { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=" },
                { "Name": "IN_DUE", "Bitwise": "and", "Value": "30", "Operator": "=" }
            ],
            "ColumnList": Constants.GET_COMING_DUE_QUALIFICATION_COLUMNS
        };

        let isComingDueQualificationView = this.state.isComingDueQualificationView,
            comingDueQualifications = {};
        isComingDueQualificationView = true;
        this.setState({ isComingDueQualificationView, comingDueQualifications });

        let token = cookies.get('IdentityToken'),
            url = "/company/" + companyId + "/tasks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);
        comingDueQualifications = response;
        isComingDueQualificationView = true;
        this.setState({ ...this.state, isComingDueQualificationView, comingDueQualifications });
    };

    render() {
        const { rows , contractorsNames} = this.state;
        let contractorsNamesLength = contractorsNames.length > 0 ? contractorsNames.length - 1 : contractorsNames.length;
        let contractorsName = contractorsNames[contractorsNamesLength] ? ' - ' + contractorsNames[contractorsNamesLength].name : "";
        return (
            <div>
                <AssignedQualification
                    backdropClassName={"no-backdrop"}
                    updateState={this.updateModalState.bind(this)}
                    modal={this.state.isAssignedQualificationView}
                    assignedQualifications={this.state.assignedQualifications}
                />
                <CompletedQualification
                    backdropClassName={"no-backdrop"}
                    updateState={this.updateModalState.bind(this)}
                    modal={this.state.isCompletedQualificationView}
                    completedQualifications={this.state.completedQualifications}
                />
                <InCompletedQualification
                    backdropClassName={"no-backdrop"}
                    updateState={this.updateModalState.bind(this)}
                    modal={this.state.isInCompletedQualificationView}
                    inCompletedQualifications={this.state.inCompletedQualifications}
                />
                <PastDueQualification
                    backdropClassName={"no-backdrop"}
                    updateState={this.updateModalState.bind(this)}
                    modal={this.state.isPastDueQualificationView}
                    pastDueQualifications={this.state.pastDueQualifications}
                />
                <ComingDueQualification
                    backdropClassName={"no-backdrop"}
                    updateState={this.updateModalState.bind(this)}
                    modal={this.state.isComingDueQualificationView}
                    comingDueQualifications={this.state.comingDueQualifications}
                />
                <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid">
                    <ModalHeader toggle={this.toggle}>Employee View{contractorsName}</ModalHeader>
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
                                    emptyRowsView={this.state.isInitial && EmployeeViewEmptyRowsView}
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