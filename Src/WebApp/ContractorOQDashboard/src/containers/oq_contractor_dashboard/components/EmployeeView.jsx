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
import AssignedQualification from '../components/AssignedQualification';
import CompletedQualification from '../components/CompletedQualification';
import InCompletedQualification from '../components/InCompletedQualification';
import PastDueQualification from '../components/PastDueQualification';
import ComingDueQualification from '../components/ComingDueQualification';
import LockedOutQualification from '../components/LockedOutQualification';
import SuspendedQualification from '../components/SuspendedQualification';
import * as API from '../../../shared/utils/APIUtils';
import * as Constants from '../../../shared/constants';
import Export from './OQDashboardExport';
import SessionPopup from './SessionPopup';
import _ from "lodash";

// Import React Table
import ReactTable from "react-table";
import "react-table/react-table.css";
/**
 * EmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-data-grid module.
 */
class EmployeeView extends PureComponent {
    constructor(props) {
        super(props);
        this.heads = [
            {
                key: 'employee',
                name: 'Employee',
                sortable: true,
                editable: false,
                cellClass: "text-left"
            },
            {
                key: 'role',
                name: 'Role',
                sortable: true,
                editable: false,
                cellClass: "text-left"
            },
            {
                key: 'assignedQualification',
                name: 'Assigned Qualifications',
                width: 180,
                sortable: true,
                editable: false,
                cellClass: "text-right"
            },
            {
                key: 'completedQualification',
                name: 'Qualifications',
                sortable: true,
                editable: false,
                cellClass: "text-right"
            },
            {
                key: 'suspendedQualification',
                name: 'Suspensions',
                sortable: true,
                editable: false,
                cellClass: "text-right"
            },
            {
                key: 'inCompletedQualification',
                name: 'Disqualifications',
                sortable: true,
                editable: false,
                cellClass: "text-right"
            },
            {
                key: 'lockoutCount',
                name: 'Locked Out 6 Months',
                width: 200,
                sortable: true,
                editable: false,
                cellClass: "text-right"
            },
            {
                key: 'comingDue',
                name: 'Expires in 30 Days',
                sortable: true,
                editable: false,
                cellClass: "text-right last-column"
            }
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
            isLockoutQualificationView: false,
            isSuspendedQualificationView: false,
            suspendedQualifications: {},
            lockoutQualifications: {},
            assignedQualifications: {},
            completedQualifications: {},
            inCompletedQualifications: {},
            pastDueQualifications: {},
            comingDueQualifications: {},
            isSessionPopup: false,
            sessionPopupType: "API"
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
    createRows = (qualificationsArray) => {
        let qualificationsArrayLength = qualificationsArray.length - 1;
        let qualifications = qualificationsArray[qualificationsArrayLength];
        const rows = [],
            length = qualifications ? qualifications.length : 0;
        for (let i = 0; i < length; i++) {
            rows.push({
                userId: qualifications[i].UserId,
                companyId: qualifications[i].CompanyId,
                employee: qualifications[i].EmployeeName + " (" + qualifications[i].UserName + " | " + qualifications[i].UserId + ")",
                role: qualifications[i].Role || "",
                assignedQualification: qualifications[i].AssignedQualification || 0,
                completedQualification: qualifications[i].CompletedQualification || 0,
                suspendedQualification: qualifications[i].SuspendedQualification || 0,
                lockoutCount: qualifications[i].LockoutCount || 0,
                inCompletedQualification: qualifications[i].DisQualification || 0,
                pastDue: qualifications[i].PastDueQualification || 0,
                comingDue: qualifications[i].InDueQualification || 0,
                total: qualifications[i].TotalEmployees || 0,
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

        if (sortColumn != "" && sortDirection != "NONE") {
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
            case "suspendedQualification":
                this.getSuspendedQualifications(userId, companyId);
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
            case "lockoutCount":
                this.getLockedOutQualifications(userId, companyId);
                break;
            default:
                console.log("default-", type, args);
                break;
        }
    };

    /**
    * @method
    * @name - getMyEmployees
    * This method will used to get My Employees details supervisior
    * @param userId
    * @param supervisor
    * @returns none
   */
    async getMyEmployees(userId, companyIdArgs, args) {
        const { cookies } = this.props;
        let { contractorManagementDetails } = sessionStorage || '{}';
        contractorManagementDetails = JSON.parse(contractorManagementDetails);
        // get the company Id from the session storage 
        let adminId = parseInt(contractorManagementDetails.User.Id) || 0;
        let contractorCompanyId = parseInt(contractorManagementDetails.Company.Id) || 0;
        const { contractorsNames } = this.state;
        let contractorsNamesLength = contractorsNames.length > 0 ? contractorsNames.length - 1 : contractorsNames.length;
        let companyId = contractorsNames[contractorsNamesLength] ? contractorsNames[contractorsNamesLength].companyId : 0;
        const payLoad = {
            "Fields": [
                { "Name": "CONTRACTOR_COMPANY", "Value": companyId, "Operator": "=" },
                { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=", "Bitwise": "and" }
            ],
            "ColumnList": Constants.GET_EMPLOYEE_QUALIFICATION_COLUMNS,
            "AppType": "OQ_DASHBOARD"
        };

        let { dashboardAPIToken } = sessionStorage || '{}';
        dashboardAPIToken = JSON.parse(dashboardAPIToken);
        let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

        let token = idToken,
            url = "/company/" + contractorCompanyId + "/tasks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true),
            myEmployees = response;

        if (response == 401) {
            this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
        } else if (response == 'API_ERROR') {
            this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
        } else {
            this.props.updateEmployeesQualificationsArray(myEmployees, args);
        }
    };

    /**
     * @method
     * @name - getAssignedQualifications
     * This method will used to get Assigned Qualifications
     * @param userId
     * @returns none
    */
    async getAssignedQualifications(userId, companyIdArgs) {
        const { cookies } = this.props;
        let { contractorManagementDetails } = sessionStorage || '{}';
        contractorManagementDetails = JSON.parse(contractorManagementDetails);
        // get the company Id from the session storage 
        let adminId = parseInt(contractorManagementDetails.User.Id) || 0;
        let contractorCompanyId = parseInt(contractorManagementDetails.Company.Id) || 0;
        const { contractorsNames } = this.state;
        let contractorsNamesLength = contractorsNames.length > 0 ? contractorsNames.length - 1 : contractorsNames.length;
        let companyId = contractorsNames[contractorsNamesLength] ? contractorsNames[contractorsNamesLength].companyId : 0;
        const payLoad = {
            "Fields": [
                { "Name": "CONTRACTOR_COMPANY", "Value": companyId, "Operator": "=" },
                { "Name": "ADMIN_ID", "Value": adminId, "Operator": "=", "Bitwise": "and" },
                { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=", "Bitwise": "and" },
                { "Name": "ASSIGNED", "Value": "true", "Operator": "=", "Bitwise": "and" }
            ],
            "ColumnList": Constants.GET_ASSIGNED_QUALIFICATION_COLUMNS,
            "AppType": "OQ_DASHBOARD"
        };

        let isAssignedQualificationView = this.state.isAssignedQualificationView,
            assignedQualifications = {};
        isAssignedQualificationView = true;
        this.setState({ isAssignedQualificationView, assignedQualifications });

        let { dashboardAPIToken } = sessionStorage || '{}';
        dashboardAPIToken = JSON.parse(dashboardAPIToken);
        let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

        let token = idToken,
            url = "/company/" + contractorCompanyId + "/tasks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

        if (response == 401) {
            this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
        } else if (response == 'API_ERROR') {
            this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
        } else {
            assignedQualifications = response;
            isAssignedQualificationView = true;
            this.setState({ ...this.state, isAssignedQualificationView, assignedQualifications });
        }
    };

    /**
    * @method
    * @name - getCompletedQualifications
    * This method will used to get Completed Qualifications
    * @param userId
    * @returns none
    */
    async getCompletedQualifications(userId, companyIdArgs) {
        const { cookies } = this.props;
        let { contractorManagementDetails } = sessionStorage || '{}';
        contractorManagementDetails = JSON.parse(contractorManagementDetails);
        // get the company Id from the session storage 
        let adminId = parseInt(contractorManagementDetails.User.Id) || 0;
        let contractorCompanyId = parseInt(contractorManagementDetails.Company.Id) || 0;
        const { contractorsNames } = this.state;
        let contractorsNamesLength = contractorsNames.length > 0 ? contractorsNames.length - 1 : contractorsNames.length;
        let companyId = contractorsNames[contractorsNamesLength] ? contractorsNames[contractorsNamesLength].companyId : 0;
        const payLoad = {
            "Fields": [
                { "Name": "CONTRACTOR_COMPANY", "Value": companyId, "Operator": "=" },
                { "Name": "ADMIN_ID", "Value": adminId, "Operator": "=", "Bitwise": "and" },
                { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=", "Bitwise": "and" },
                { "Name": "COMPLETED", "Value": "true", "Operator": "=", "Bitwise": "and" }
            ],
            "ColumnList": Constants.GET_COMPLETED_QUALIFICATION_COLUMNS,
            "AppType": "OQ_DASHBOARD"
        };

        let isCompletedQualificationView = this.state.isCompletedQualificationView,
            completedQualifications = {};
        isCompletedQualificationView = true;
        this.setState({ isCompletedQualificationView, completedQualifications });

        let { dashboardAPIToken } = sessionStorage || '{}';
        dashboardAPIToken = JSON.parse(dashboardAPIToken);
        let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

        let token = idToken,
            url = "/company/" + contractorCompanyId + "/tasks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

        if (response == 401) {
            this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
        } else if (response == 'API_ERROR') {
            this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
        } else {
            completedQualifications = response;
            isCompletedQualificationView = true;
            this.setState({ ...this.state, isCompletedQualificationView, completedQualifications });
        }
    };

    /**
    * @method
    * @name - getInCompletedQualifications
    * This method will used to get InCompleted Qualifications
    * @param userId
    * @returns none
    */
    async getInCompletedQualifications(userId, companyIdArgs) {
        const { cookies } = this.props;
        let { contractorManagementDetails } = sessionStorage || '{}';
        contractorManagementDetails = JSON.parse(contractorManagementDetails);
        // get the company Id from the session storage 
        let adminId = parseInt(contractorManagementDetails.User.Id) || 0;
        let contractorCompanyId = parseInt(contractorManagementDetails.Company.Id) || 0;
        const { contractorsNames } = this.state;
        let contractorsNamesLength = contractorsNames.length > 0 ? contractorsNames.length - 1 : contractorsNames.length;
        let companyId = contractorsNames[contractorsNamesLength] ? contractorsNames[contractorsNamesLength].companyId : 0;
        const payLoad = {
            "Fields": [
                { "Name": "CONTRACTOR_COMPANY", "Value": companyId, "Operator": "=" },
                { "Name": "ADMIN_ID", "Value": adminId, "Operator": "=", "Bitwise": "and" },
                { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=", "Bitwise": "and" },
                { "Name": "IN_COMPLETE", "Value": "true", "Operator": "=", "Bitwise": "and" }
            ],
            "ColumnList": Constants.GET_IN_COMPLETED_QUALIFICATION_COLUMNS,
            "AppType": "OQ_DASHBOARD"
        };

        let isInCompletedQualificationView = this.state.isInCompletedQualificationView,
            inCompletedQualifications = {};
        isInCompletedQualificationView = true;
        this.setState({ isInCompletedQualificationView, inCompletedQualifications });

        let { dashboardAPIToken } = sessionStorage || '{}';
        dashboardAPIToken = JSON.parse(dashboardAPIToken);
        let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

        let token = idToken,
            url = "/company/" + contractorCompanyId + "/tasks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

        if (response == 401) {
            this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
        } else if (response == 'API_ERROR') {
            this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
        } else {
            inCompletedQualifications = response;
            isInCompletedQualificationView = true;
            this.setState({ ...this.state, isInCompletedQualificationView, inCompletedQualifications });
        }
    };

    /**
  * @method
  * @name - getSuspendedQualifications
  * This method will used to get Suspended Qualifications
  * @param userId
  * @returns none
  */
    async getSuspendedQualifications(userId, companyId) {
        let { contractorManagementDetails } = sessionStorage || '{}';
        contractorManagementDetails = JSON.parse(contractorManagementDetails);
        // get the company Id from the session storage 
        let adminId = parseInt(contractorManagementDetails.User.Id) || 0;
        let contractorCompanyId = parseInt(contractorManagementDetails.Company.Id) || 0;
        const payLoad = {
            "Fields": [
                { "Name": "CONTRACTOR_COMPANY", "Value": companyId, "Operator": "=" },
                { "Name": "IN_COMPLETE", "Value": "true", "Operator": "=", "Bitwise": "and" }
            ],
            "ColumnList": Constants.GET_IN_COMPLETED_QUALIFICATION_COLUMNS,
            "AppType": "OQ_DASHBOARD"
        };

        let isSuspendedQualificationView = this.state.isSuspendedQualificationView,
            suspendedQualifications = {};
        isSuspendedQualificationView = true;
        this.setState({ isSuspendedQualificationView, suspendedQualifications });

        let { dashboardAPIToken } = sessionStorage || '{}';
        dashboardAPIToken = JSON.parse(dashboardAPIToken);
        let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

        let token = idToken,
            url = "/company/" + contractorCompanyId + "/tasks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

        if (response == 401) {
            this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
        } else if (response == 'API_ERROR') {
            this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
        } else {
            suspendedQualifications = response;
            isSuspendedQualificationView = true;
            this.setState({ ...this.state, isSuspendedQualificationView, suspendedQualifications });
        }
    };

    /**
    * @method
    * @name - getPastDueQualifications
    * This method will used to get Past Due Qualifications
    * @param userId
    * @returns none
    */
    async getPastDueQualifications(userId, companyIdArgs) {
        const { cookies } = this.props;
        let { contractorManagementDetails } = sessionStorage || '{}';
        contractorManagementDetails = JSON.parse(contractorManagementDetails);
        // get the company Id from the session storage 
        let adminId = parseInt(contractorManagementDetails.User.Id) || 0;
        let contractorCompanyId = parseInt(contractorManagementDetails.Company.Id) || 0;
        const { contractorsNames } = this.state;
        let contractorsNamesLength = contractorsNames.length > 0 ? contractorsNames.length - 1 : contractorsNames.length;
        let companyId = contractorsNames[contractorsNamesLength] ? contractorsNames[contractorsNamesLength].companyId : 0;
        const payLoad = {
            "Fields": [
                { "Name": "CONTRACTOR_COMPANY", "Value": companyId, "Operator": "=" },
                { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=", "Bitwise": "and" },
                { "Name": "ADMIN_ID", "Value": adminId, "Operator": "=", "Bitwise": "and" },
                { "Name": "PAST_DUE", "Value": "30", "Operator": "=", "Bitwise": "and" }
            ],
            "ColumnList": Constants.GET_PAST_DUE_QUALIFICATION_COLUMNS,
            "AppType": "OQ_DASHBOARD"
        };

        let isPastDueQualificationView = this.state.isPastDueQualificationView,
            pastDueQualifications = {};
        isPastDueQualificationView = true;
        this.setState({ isPastDueQualificationView, pastDueQualifications });

        let { dashboardAPIToken } = sessionStorage || '{}';
        dashboardAPIToken = JSON.parse(dashboardAPIToken);
        let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

        let token = idToken,
            url = "/company/" + contractorCompanyId + "/tasks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

        if (response == 401) {
            this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
        } else if (response == 'API_ERROR') {
            this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
        } else {
            pastDueQualifications = response;
            isPastDueQualificationView = true;
            this.setState({ ...this.state, isPastDueQualificationView, pastDueQualifications });
        }
    };

    /**
    * @method
    * @name - getComingDueQualifications
    * This method will used to get Coming due qualifications
    * @param userId
    * @returns none
    */
    async getComingDueQualifications(userId, companyIdArgs) {
        const { cookies } = this.props;
        let { contractorManagementDetails } = sessionStorage || '{}';
        contractorManagementDetails = JSON.parse(contractorManagementDetails);
        // get the company Id from the session storage 
        let adminId = parseInt(contractorManagementDetails.User.Id) || 0;
        let contractorCompanyId = parseInt(contractorManagementDetails.Company.Id) || 0;
        const { contractorsNames } = this.state;
        let contractorsNamesLength = contractorsNames.length > 0 ? contractorsNames.length - 1 : contractorsNames.length;
        let companyId = contractorsNames[contractorsNamesLength] ? contractorsNames[contractorsNamesLength].companyId : 0;
        const payLoad = {
            "Fields": [
                { "Name": "CONTRACTOR_COMPANY", "Value": companyId, "Operator": "=" },
                { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=", "Bitwise": "and" },
                { "Name": "ADMIN_ID", "Value": adminId, "Operator": "=", "Bitwise": "and" },
                { "Name": "IN_DUE", "Value": "30", "Operator": "=", "Bitwise": "and" }
            ],
            "ColumnList": Constants.GET_COMING_DUE_QUALIFICATION_COLUMNS,
            "AppType": "OQ_DASHBOARD"
        };

        let isComingDueQualificationView = this.state.isComingDueQualificationView,
            comingDueQualifications = {};
        isComingDueQualificationView = true;
        this.setState({ isComingDueQualificationView, comingDueQualifications });

        let { dashboardAPIToken } = sessionStorage || '{}';
        dashboardAPIToken = JSON.parse(dashboardAPIToken);
        let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

        let token = idToken,
            url = "/company/" + contractorCompanyId + "/tasks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

        if (response == 401) {
            this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
        } else if (response == 'API_ERROR') {
            this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
        } else {
            comingDueQualifications = response;
            isComingDueQualificationView = true;
            this.setState({ ...this.state, isComingDueQualificationView, comingDueQualifications });
        }
    };

    /**
  * @method
  * @name - getLockedOutQualifications
  * This method will used to get LockedOut Qualifications
  * @param userId
  * @returns none
  */
    async getLockedOutQualifications(userId, companyIdArgs) {
        const { cookies } = this.props;
        let { contractorManagementDetails } = sessionStorage || '{}';
        contractorManagementDetails = JSON.parse(contractorManagementDetails);
        // get the company Id from the session storage 
        let adminId = parseInt(contractorManagementDetails.User.Id) || 0;
        let contractorCompanyId = parseInt(contractorManagementDetails.Company.Id) || 0;
        const { contractorsNames } = this.state;
        let contractorsNamesLength = contractorsNames.length > 0 ? contractorsNames.length - 1 : contractorsNames.length;
        let companyId = contractorsNames[contractorsNamesLength] ? contractorsNames[contractorsNamesLength].companyId : 0;
        const payLoad = {
            "Fields": [
                { "Name": "CONTRACTOR_COMPANY", "Value": companyId, "Operator": "=" },
                { "Name": "LOCKOUT_COUNT", "Value": "true", "Operator": "=", "Bitwise": "and" },
                { "Name": "ADMIN_ID", "Value": adminId, "Operator": "=", "Bitwise": "and" },
                { "Name": "SUPERVISOR_ID", "Value": userId, "Operator": "=", "Bitwise": "and" },
            ],
            "ColumnList": Constants.GET_COMPLETED_QUALIFICATION_COLUMNS,
            "AppType": "OQ_DASHBOARD"
        };

        let isLockoutQualificationView = this.state.isLockoutQualificationView,
            lockoutQualifications = {};
        isLockoutQualificationView = true;
        this.setState({ isLockoutQualificationView, lockoutQualifications });

        let { dashboardAPIToken } = sessionStorage || '{}';
        dashboardAPIToken = JSON.parse(dashboardAPIToken);
        let idToken = dashboardAPIToken.dashboardAPIToken.IdToken || "";

        let token = idToken,
            url = "/company/" + contractorCompanyId + "/tasks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

        if (response == 401) {
            this.setState({ isSessionPopup: true, sessionPopupType: 'SESSION' });
        } else if (response == 'API_ERROR') {
            this.setState({ isSessionPopup: true, sessionPopupType: 'API' });
        } else {
            lockoutQualifications = response;
            isLockoutQualificationView = true;
            this.setState({ ...this.state, isLockoutQualificationView, lockoutQualifications });
        }
    };

    customCell(props) {
        let self = this;
        let value = parseInt(props.value);
        return (
            value && <span onClick={e => { e.preventDefault(); self.handleCellClick(props.column.id, props.original); }} className={"text-clickable"}>
                {value}
            </span> || <span>{value}</span>
        );
    };

    render() {
        const { rows, contractorsNames } = this.state;
        let contractorsNamesLength = contractorsNames.length > 0 ? contractorsNames.length - 1 : contractorsNames.length;
        let contractorsName = contractorsNames[contractorsNamesLength] ? ' - ' + contractorsNames[contractorsNamesLength].name : "";
        let pgSize = (rows.length > 10) ? rows.length : 10;
        return (
            <div>
                <SessionPopup
                    backdropClassName={"backdrop"}
                    modal={this.state.isSessionPopup}
                    sessionPopupType={this.state.sessionPopupType}
                />
                <LockedOutQualification
                    backdropClassName={"backdrop"}
                    updateState={this.updateModalState.bind(this)}
                    modal={this.state.isLockoutQualificationView}
                    lockoutQualifications={this.state.lockoutQualifications}
                />
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
                <SuspendedQualification
                    backdropClassName={"backdrop"}
                    updateState={this.updateModalState.bind(this)}
                    modal={this.state.isSuspendedQualificationView}
                    suspendedQualifications={this.state.suspendedQualifications}
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
                <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid grid-modal-popup">
                    <ModalHeader toggle={this.toggle}>Employee View{contractorsName}</ModalHeader>
                    <div>
                        <div className="export-menu-one">

                        </div>
                        <div className="export-menu-two">
                            <Export
                                data={this.state.rows}
                                heads={this.heads}
                                sheetName={"Employee View"}
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
                                            Header: "Employee",
                                            accessor: "employee",
                                            headerClassName: 'header-wordwrap',
                                            minWidth: 250,
                                            className: 'text-left'
                                        },
                                        {
                                            Header: "Role",
                                            accessor: "role",
                                            headerClassName: 'header-wordwrap',
                                            minWidth: 120,
                                            className: 'text-left'
                                        },
                                        {
                                            Header: "Assigned Qualifications",
                                            accessor: "assignedQualification",
                                            headerClassName: 'header-wordwrap',
                                            minWidth: 150,
                                            className: 'text-center',
                                            Cell: this.customCell
                                        },
                                        {
                                            Header: "Qualifications",
                                            id: "completedQualification",
                                            accessor: "completedQualification",
                                            headerClassName: 'header-wordwrap',
                                            minWidth: 100,
                                            className: 'text-center',
                                            Cell: this.customCell
                                        },
                                        {
                                            Header: "Suspensions",
                                            accessor: "suspendedQualification",
                                            headerClassName: 'header-wordwrap',
                                            minWidth: 100,
                                            className: 'text-center',
                                            Cell: this.customCell,
                                        },
                                        {
                                            Header: "Disqualifications",
                                            accessor: "inCompletedQualification",
                                            headerClassName: 'header-wordwrap',
                                            minWidth: 100,
                                            className: 'text-center',
                                            Cell: this.customCell
                                        },
                                        {
                                            Header: "Locked Out 6 Months",
                                            accessor: "lockoutCount",
                                            headerClassName: 'header-wordwrap',
                                            minWidth: 100,
                                            className: 'text-center',
                                            Cell: this.customCell
                                        },
                                        {
                                            Header: "Expires in 30 Days",
                                            accessor: "comingDue",
                                            headerClassName: 'header-wordwrap',
                                            minWidth: 100,
                                            className: 'text-center',
                                            Cell: this.customCell
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
                                    pageSize={this.state.isInitial ? 5 : pgSize}
                                    loading={this.state.isInitial}
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

export default EmployeeView;