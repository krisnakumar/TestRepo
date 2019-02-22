/* eslint-disable */
/*
* QueryClause.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details to list the workbooks 
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------
createRows(employees)
toggle()
handleGridRowsUpdated(fromRow, toRow, updated)
handleGridSort(sortColumn, sortDirection)
updateModalState(modelName)
handleCellFocus(args) 
*/
import React, { PureComponent } from 'react';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import { Input } from 'reactstrap';
import Select from 'react-select';
import _ from 'lodash'
import 'whatwg-fetch'
import * as API from '../../../shared/utils/APIUtils';
import FieldData from './../data';
import classNames from 'classnames';
import FormValidator from '../../../shared/utils/FormValidator';
import * as Constants from '../../../shared/constants';
import SelectPlus from 'react-select-plus';
import 'react-select-plus/dist/react-select-plus.css';

// React Dates Import here
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';

import DayPickerInput from "react-day-picker/DayPickerInput";
import DayPicker, { DateUtils } from "react-day-picker";
import "react-day-picker/lib/style.css";
import { formatDate, parseDate } from "react-day-picker/moment";
import { Container, Row, Col, Button, ButtonGroup } from 'reactstrap';
import * as moment from 'moment';

class QueryClause extends PureComponent {

    static propTypes = {
        cookies: instanceOf(Cookies).isRequired
    };

    constructor(props) {
        super(props);
        // create a ref to store the textInput DOM element
        this.buttonRef = [];

        this.validator = new FormValidator(this.formatValidationData(this.formatRowData(this.props.fieldData || [])));

        this.state = {
            fieldData: this.props.fieldData || [],
            formattedData: this.formatRowData(this.props.fieldData || []),
            validation: this.validator.valid(),
            entity: this.props.entity,
            options: [
                { value: 'ME', label: '@Me' },
                { value: 'myEmployee', label: '@MyEmployee' },
                { value: 'mySupervisor', label: '@MySupervisor' }
            ],
            multi: false,
            multiValue: [],
            startDate: null,
            endDate: null,
            focusedInput: null,
            from: undefined,
            to: undefined,
            queryClause: {}
        };

        this.submitted = false;
        this.toInputs = [];

        this.handleChange = this.handleChange.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleAddClause = this.handleAddClause.bind(this);
        this.handleDeleteClause = this.handleDeleteClause.bind(this);
        this.onOpenClose = this.onOpenClose.bind(this);
        this.onDatePickerOpen = this.onDatePickerOpen.bind(this);
        this.handleOnChange = this.handleOnChange.bind(this);
        this.handleOnInputChange = this.handleOnInputChange.bind(this);
        this.getDateByValue = this.getDateByValue.bind(this);
        this.handleFromChange = this.handleFromChange.bind(this);
        this.handleToChange = this.handleToChange.bind(this);
        this.reloadQuery = this.reloadQuery.bind(this);
    }

    /**
     * @method
     * @name - componentWillReceiveProps
     * This method will invoked whenever the props or state
     *  is update to this component class
     * @param newProps
     * @returns none
     */
    componentWillReceiveProps(newProps) {
        if (this.state.entity != newProps.entity) {
            this.state.entity = newProps.entity;
            this.changeQueryClause(newProps.entity);
        }
    }

    /**
     * @method
     * @name - checkQueryState
     * This method will check if any query is added/deleted than default
     * @param none
     * @returns isSame
     */
    checkQueryState() {
        let initialFormattedData = this.formatRowData(this.props.fieldData),
            currentFormattedData = this.state.formattedData,
            isSame = _.isEqual(initialFormattedData, currentFormattedData);// returns false if different

        return isSame;
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
     * @name - resetQueryClause
     * This method reset the query clause selection to initial state
     * @param none
     * @returns none
    */
    resetQueryClause() {
        const initialState = {
            entity: this.state.entity,
            fieldData: FieldData.field[this.state.entity].slice(0, 2)
        };

        let fieldData = initialState.fieldData,
            formattedData = this.formatRowData(fieldData),
            formValidatorData = this.formatValidationData(formattedData);

        this.validator = new FormValidator(formValidatorData);
        this.submitted = false;

        this.setState({
            fieldData: fieldData,
            formattedData: formattedData,
            validation: this.validator.valid()
        });
        this.forceUpdate();
    }

    /**
    * @method
    * @name - changeQueryClause
    * This method change the query clause selection to initial state 
    * @param none
    * @returns none
   */
    changeQueryClause(selectedEntity) {
        const initialState = {
            entity: selectedEntity,
            fieldData: FieldData.field[selectedEntity].slice(0, 2)
        };

        let fieldData = initialState.fieldData,
            formattedData = this.formatRowData(fieldData),
            formValidatorData = this.formatValidationData(formattedData);

        this.validator = new FormValidator(formValidatorData);
        this.submitted = false;
        this.setState({
            fieldData: fieldData,
            formattedData: formattedData,
            validation: this.validator.valid(),
            entity: selectedEntity
        });
        this.forceUpdate();
    }

    /**
    * @method
    * @name - formatValidationData
    * This method format the data to get Validation Data
    * @param formattedData
    * @returns validationData
   */
    formatValidationData(formattedData) {
        let validationData = [];
        formattedData.forEach(function (element, index) {
            let obj = {
                field: index + "+" + element.value,
                method: element.validation,
                isSkipValidation: element.isSkipValidation || false,
                validWhen: false,
                message: element.errormessage || Constants.FIELD_ERROR_MESSAGE
            };
            validationData.push(obj);
        });
        return validationData;
    }

    /**
     * @method
     * @name - formatRowData
     * This method format the input data to get formatted data which is used by dynamic Query Clause selection 
     * @param unFormattedData
     * @returns formattedData
    */
    formatRowData(unFormattedData) {
        let formattedData = [],
            entity = this.state ? this.state.entity : this.props.entity;

        unFormattedData.map(function (field, index) {
            let obj = {},
                type = field.type,
                smartParams = field.smartParam;
            obj.label = field.label;
            obj.type = field.type;
            obj.placeholder = field.placeholder || "";
            obj.isSkipValidation = field.isSkipValidation || false;
            obj.validation = field.validation;
            obj.value = field.value;
            obj.combinators = FieldData.combinators;
            obj.fields = FieldData.field[entity];
            obj.operators = FieldData.operator[type];
            obj.valueSelected = "";
            obj.isFocus = false;
            obj.combinatorsSelected = FieldData.combinators[0];
            obj.fieldsSelected = FieldData.field[entity][index];
            obj.operatorsSelected = FieldData.operator[type][0];
            obj.smartParam = FieldData.params[smartParams];
            obj.hasSmartParams = field.hasSmartParams;

            formattedData.push(obj);
        });
        return formattedData;
    }

    /**
     * @method
     * @name - handleChange
     * This method will triggered on select option change to update the selected value in state
     * @param index
     * @param key
     * @param selectedOption
     * @returns none
    */
    handleChange(index, key, selectedOption) {
        let formattedData = this.state.formattedData;
        formattedData[index][key] = selectedOption;
        let isSameType = formattedData[index].type == selectedOption.type ? true : false;
        switch (key) {
            case "fieldsSelected":
                let type = selectedOption ? selectedOption.type : "others",
                    smartParamType = selectedOption.smartParam || "NONE";
                formattedData[index].valueSelected = isSameType ? formattedData[index].valueSelected : "";
                formattedData[index].operators = FieldData.operator[type];
                formattedData[index].placeholder = selectedOption.placeholder || "";
                formattedData[index].isSkipValidation = true;
                formattedData[index].operatorsSelected = formattedData[index].type == type ? formattedData[index].operatorsSelected : FieldData.operator[type][0];
                formattedData[index].value = selectedOption.value || "";
                formattedData[index].type = selectedOption.type || "";
                formattedData[index].label = selectedOption.label || "";
                formattedData[index].smartParam = FieldData.params[smartParamType];
                formattedData[index].hasSmartParams = selectedOption.hasSmartParams;
                break;
            case "operatorsSelected":
                let operatorType = selectedOption ? selectedOption.value : "=",
                    isDateType = formattedData[index].type;
                if (isDateType == "date" && operatorType == "Between") {
                    formattedData[index].hasSmartParams = false;
                    formattedData[index].value = "";
                } else if (isDateType == "date") {
                    formattedData[index].hasSmartParams = true;
                    formattedData[index].value = "";
                }

                break;
            default:
            // Do nothing 
        }
        this.setState({ ...this.state, formattedData });
        this.forceUpdate();
    }

    /**
     * @method
     * @name - handleInputChange
     * This method will triggered on input change to update the value in state
     * @param index
     * @param key
     * @param selectedOption
     * @param ele
     * @returns none
    */
    handleInputChange(index, key, selectedOption, ele) {
        ele.preventDefault();
        let formattedData = this.state.formattedData,
            value = ele.target.value.replace(/^\s+/g, '') || "";
        formattedData[index][key] = value;
        this.setState({ ...this.state, formattedData });
        this.forceUpdate();
    }

    /**
     * @method
     * @name - handleAddClause
     * This method will add a query clause above the selected position
     * @param index
     * @returns none
    */
    handleAddClause(index) {

        if (index != "n")
            this.buttonRef[index].blur();

        let formattedData = this.state.formattedData,
            entity = this.state ? this.state.entity : this.props.entity;

        formattedData.forEach(function (element, index) {
            formattedData[index].isFocus = false;
        });

        let obj = {},
            type = FieldData.field[entity][0].type,
            smartParamType = FieldData.field[entity][0].smartParam;
        obj.label = "";
        obj.type = type;
        obj.validation = "isEmpty";
        obj.value = "";
        obj.placeholder = "";
        obj.isSkipValidation = true;
        obj.combinators = FieldData.combinators;
        obj.fields = FieldData.field[entity];
        obj.operators = FieldData.operator[type];
        obj.valueSelected = "";
        obj.isFocus = false;
        obj.combinatorsSelected = FieldData.combinators[0];
        obj.fieldsSelected = FieldData.field[entity][0];
        obj.operatorsSelected = FieldData.operator[type][0];
        obj.smartParam = FieldData.params[smartParamType];
        obj.hasSmartParams = FieldData.field[entity][0].hasSmartParams;

        if (index == "n") {
            formattedData.push(obj);
        } else {
            formattedData.splice(index, 0, obj);
            // inserts at 1st position
        }
        this.validator = new FormValidator(this.formatValidationData(this.formatRowData(formattedData)));

        this.setState({ ...this.state, formattedData });
        this.forceUpdate();

    };

    /**
     * @method
     * @name - handleDeleteClause
     * This method will delete a selected query clause
     * @param index
     * @returns none
    */
    handleDeleteClause(index) {
        let currentIndex = index + 1,
            formattedData = this.state.formattedData,
            formattedDataLength = formattedData.length,
            obj = {},
            entity = this.state ? this.state.entity : this.props.entity;

        // Delete by Index from Array
        formattedData = formattedData.slice(0, currentIndex - 1).concat(formattedData.slice(currentIndex, formattedData.length));

        if (index == 0 && formattedDataLength <= 1) {
            let type = FieldData.field[entity][0].type,
                smartParamType = FieldData.field[entity][0].smartParam;
            obj.label = "";
            obj.type = type;
            obj.validation = "isEmpty";
            obj.value = "";
            obj.placeholder = "";
            obj.isSkipValidation = true;
            obj.combinators = FieldData.combinators;
            obj.fields = FieldData.field[entity];
            obj.operators = FieldData.operator[type];
            obj.valueSelected = "";
            obj.isFocus = true;
            obj.combinatorsSelected = FieldData.combinators[0];
            obj.fieldsSelected = FieldData.field[entity][0];
            obj.operatorsSelected = FieldData.operator[type][0];
            obj.smartParam = FieldData.params[smartParamType];
            obj.hasSmartParams = FieldData.field[entity][0].hasSmartParams;

            formattedData.push(obj);
        }
        this.validator = new FormValidator(this.formatValidationData(this.formatRowData(formattedData)));
        this.setState({ ...this.state, formattedData });
        this.forceUpdate();
    };

    /**
     * @method
     * @name - buildQuery
     * This method will create a query from selected query clause and conditions and trigger the API
     * @param index
     * @returns none
    */
    buildQuery() {
        let tempFormattedData = this.state.formattedData;

        tempFormattedData.forEach(function (element, index) {
            tempFormattedData[index].isSkipValidation = false;
        });
        const { cookies } = this.props;

        this.state.formattedData = tempFormattedData;

        this.validator = new FormValidator(this.formatValidationData(this.formatRowData(this.state.formattedData)));

        const validation = this.validator.validate(this.state.formattedData);
        this.setState({ validation });
        this.submitted = true;

        if (validation.isValid) {
            // handle actual form submission here
            let queryClause = [],
                _self = this;

            this.state.formattedData.forEach(function (element, index) {
                let queryObj = {},
                    fieldValue = _self.state.formattedData[index].valueSelected,
                    fieldType = _self.state.formattedData[index].type,
                    hasSmartParams = _self.state.formattedData[index].hasSmartParams;

                if (fieldType == "date" && !hasSmartParams) {
                    queryObj.Value = typeof fieldValue === 'object' ? formatDate(fieldValue.from, 'L', 'en') + " and " + formatDate(fieldValue.to, 'L', 'en') : fieldValue;
                } else if (fieldType == "date" && hasSmartParams) {
                    queryObj.Value = typeof fieldValue === 'object' ? _self.getDateByValue(fieldValue.value) : fieldValue;
                } else {
                    queryObj.Value = typeof fieldValue === 'object' ? fieldValue.value : fieldValue;
                }
                queryObj.Operator = _self.state.formattedData[index].operatorsSelected.value;
                queryObj.Name = _self.state.formattedData[index].fieldsSelected.field;
                if (fieldType == "date" && hasSmartParams) {
                    if (fieldValue.value == "today" || fieldValue.value == "yesterday") {
                        queryObj.Operator = _self.state.formattedData[index].operatorsSelected.value;
                    } else {
                        queryObj.Operator = "Between";
                    }
                }
                if (index == 0) {
                    queryObj.Bitwise = "";
                } else {
                    queryObj.Bitwise = _self.state.formattedData[index] ? _self.state.formattedData[index].combinatorsSelected.value : "";
                }

                queryClause.push(queryObj);

                if (_self.state.formattedData[index].fieldsSelected.smartParam == "user" && (queryObj.Value == "ME" || queryObj.Value == "ME_AND_DIRECT_SUBORDINATES" || queryObj.Value == "DIRECT_SUBORDINATES")) {
                    let queryObjUser = {};
                    queryObjUser.Value = cookies.get('UserId');
                    queryObjUser.Operator = "=";
                    queryObjUser.Name = "CURRENT_USER";
                    queryObjUser.BitWise = "AND";
                    queryClause.push(queryObjUser);
                }
            })
            this.state.queryClause = queryClause;
            switch (this.state.entity) {
                case 'employees':
                    this.getEmployeesResults(queryClause);
                    break;
                case 'workbooks':
                    this.getWorkbooksResults(queryClause);
                    break;
                case 'tasks':
                    this.getTasksResults(queryClause);
                    break;
                default:
                    console.log('Sorry, we are out of options');
            }
        }
    };


    getDateByValue(dateValue) {
        let date = dateValue;
        switch (dateValue) {
            case 'today':
                date = formatDate(moment(), 'L', 'en');
                break;
            case 'yesterday':
                date = formatDate(moment().subtract(1, 'days'), 'L', 'en');
                break;
            case 'thisWeek':
                date = formatDate(moment().startOf('week'), 'L', 'en') + " and " + formatDate(moment().endOf('week'), 'L', 'en');
                break;
            case 'lastWeek':
                date = formatDate(moment().subtract(1, 'weeks').startOf('week'), 'L', 'en') + " and " + formatDate(moment().subtract(1, 'weeks').endOf('week'), 'L', 'en');
                break;
            case 'thisMonth':
                date = formatDate(moment().startOf('month'), 'L', 'en') + " and " + formatDate(moment().endOf('month'), 'L', 'en');
                break;
            case 'lastMonth':
                date = formatDate(moment().subtract(1, 'months').startOf('month'), 'L', 'en') + " and " + formatDate(moment().subtract(1, 'months').endOf('month'), 'L', 'en');
                break;
            case 'thisYear':
                date = formatDate(moment().startOf('year'), 'L', 'en') + " and " + formatDate(moment().endOf('year'), 'L', 'en');
                break;
            case 'lastYear':
                date = formatDate(moment().subtract(1, 'years').startOf('year'), 'L', 'en') + " and " + formatDate(moment().subtract(1, 'years').endOf('year'), 'L', 'en');
                break;
            default:
                date = dateValue;
        }
        return date;
    };

    reloadQuery(param) {
        let { queryClause } = this.state;
        switch (this.state.entity) {
            case 'employees':
                this.getEmployeesResults(queryClause);
                break;
            case 'workbooks':
                this.getWorkbooksResults(queryClause);
                break;
            case 'tasks':
                this.getTasksResults(queryClause);
                break;
            default:
                console.log('Sorry, we are out of options');
        }
    };

    /**
     * @method
     * @name - getClassName
     * This method will used to get Class name from validation
     * @param index
     * @returns Class name or ""
    */
    getClassName(index) {
        const { formattedData } = this.state;
        let validation = this.submitted ?               // if the form has been submitted at least once
            this.validator.validate(formattedData) :   // then check validity every time we render
            this.state.validation;

        if (formattedData[index].isSkipValidation) {
            return classNames('form-group-has-validation');
        } else {
            return classNames('form-group-has-validation', { 'has-error': validation[index] ? validation[index].isInvalid : false });
        }
    };

    /**
     * @method
     * @name - getMessage
     * This method will used to get error Message from validation
     * @param index
     * @returns Message or ""
    */
    getMessage(index) {
        const { formattedData } = this.state;
        let validation = this.submitted ?               // if the form has been submitted at least once
            this.validator.validate(formattedData) :   // then check validity every time we render
            this.state.validation;

        if (formattedData[index].isSkipValidation) {
            return "";
        } else {
            return validation[index] ? validation[index].message : "";
        }
    };

    /**
     * @method
     * @name - getEmployeesResults
     * This method will used to get Employees Results from the Query Clause
     * @param requestData
     * @returns none
    */
    async getEmployeesResults(requestData) {
        const { cookies } = this.props;

        let payLoad = {
            "Fields": requestData,
            "ColumnList": ["EMPLOYEE_NAME", "ROLE", "USER_ID", "USERNAME", "ALTERNATE_USERNAME", "TOTAL_EMPLOYEES", "EMAIL", "ADDRESS"]
        };

        let token = cookies.get('IdentityToken'),
            companyId = cookies.get('CompanyId'),
            url = "/company/" + companyId + "/employees",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

        this.props.passEmployeesResults(response);
    };

    /**
    * @method
    * @name - getWorkbooksResults
    * This method will used to get Workbook Results from the Query Clause
    * @param requestData
    * @returns none
   */
    async getWorkbooksResults(requestData) {
        const { cookies } = this.props;

        let payLoad = { "Fields": requestData, "ColumnList": ["WORKBOOK_ID", "WORKBOOK_NAME", "DESCRIPTION", "WORKBOOK_CREATED_BY", "DAYS_TO_COMPLETE"] };

        let token = cookies.get('IdentityToken'),
            companyId = cookies.get('CompanyId'),
            url = "/company/" + companyId + "/workbooks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

        this.props.passWorkbooksResults(response);
    };

    /**
    * @method
    * @name - getTasksResults
    * This method will used to get Tasks Results from the Query Clause
    * @param requestData
    * @returns none
   */
    async getTasksResults(requestData) {
        const { cookies } = this.props;

        let payLoad = { "Fields": requestData, "ColumnList": ["TASK_ID", "TASK_NAME", "ASSIGNED_TO", "EVALUATOR_NAME", "EXPIRATION_DATE"] };

        let token = cookies.get('IdentityToken'),
            companyId = cookies.get('CompanyId'),
            url = "/company/" + companyId + "/tasks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

        this.props.passTasksResults(response);
    };

    /**
     * @method
     * @name - onOpenClose
     * This method will used to set the css props to select menu outer to positioning
     * @param none
     * @returns none
    */
    onOpenClose() {
        let inputWrapper = document.querySelector(".is-open").getBoundingClientRect();
        document.querySelector(".Select-menu").style.width = inputWrapper.width + 'px';
        document.querySelector(".Select-menu").style.top = inputWrapper.top + inputWrapper.height + 'px';
    };

    /**
    * @method
    * @name - onDatePickerOpen
    * This method will used to set the css props to select menu outer to positioning
    * @param none
    * @returns none
   */
    onDatePickerOpen() {
        let inputWrapper = document.querySelector(".DayPickerInput-OverlayWrapper").previousElementSibling.getBoundingClientRect();
        document.querySelector(".DayPickerInput-Overlay").style.position = 'fixed';
        document.querySelector(".DayPickerInput-Overlay").style.left = inputWrapper.left + 'px';
        document.querySelector(".DayPickerInput-Overlay").style.display = 'block';
    };

    handleOnChange(index, type, c, value) {
        let formattedData = this.state.formattedData;
        formattedData[index][type] = value;
        this.setState({ ...this.state, formattedData, multiValue: value });
        this.forceUpdate();
    };

    handleOnInputChange(index, type, c, value) {
        let formattedData = this.state.formattedData;
        value = value.replace(/^\s+/g, '') || "";

        let selectValue = { value: value, label: value, className: "Select-create-option-placeholder" };
        formattedData[index][type] = selectValue;
        this.setState({ ...this.state, formattedData, multiValue: selectValue });
        this.forceUpdate();
    };

    componentDidUpdate(prevProps, prevState, snapshot) {
        // If we have a snapshot value, we've just added new items.
        // Adjust scroll so these new items don't push the old ones out of view.
        // (snapshot here is the value returned from getSnapshotBeforeUpdate)
        let _self = this,
            nodeList = document.querySelectorAll('.DayPickerInput');
        if (nodeList.length > 0) {
            for (var i = 0, len = nodeList.length; i < len; i++) {
                nodeList[i].addEventListener('click', function (event) {
                    _self.onDatePickerOpen();
                });
            }
        }
    };


    CustomOverlay(index, { classNames, selectedDay, children, ...props }, excess) {
        let position = index.split("@"),
            fieldIndex = position[0],
            fieldType = position[1];
        return (
            <div
                className={classNames.overlayWrapper}
                style={{ marginLeft: -100 }}
                {...props}
            >
                <div className={classNames.overlay}>
                    <Row>
                        <Col xs="4" className="padding-rt-0">
                            <div className="date-picker-btn-group">
                                <Button className="date-picker-btn">Today</Button>
                                <Button className="date-picker-btn">Yesterday</Button>
                                <Button className="date-picker-btn">This Week</Button>
                                <Button className="date-picker-btn">Last Week</Button>
                                <Button className="date-picker-btn">This Month</Button>
                                <Button className="date-picker-btn">Last Month</Button>
                                <Button className="date-picker-btn">This Year</Button>
                                <Button className="date-picker-btn">Last Year</Button>
                            </div>
                        </Col>
                        <Col xs="8" className="padding-lf-0"> {children}</Col>
                    </Row>
                </div>
            </div>
        );
    };

    handleFromChange(index, type, b, value) {
        // Change the from date and focus the "to" input field
        let formattedData = this.state.formattedData;
        typeof formattedData[index][type] === 'object' ? formattedData[index][type].from = value : formattedData[index][type] = {};
        formattedData[index][type].from = value;
        this.setState({ ...this.state, formattedData });
        this.forceUpdate();
    }

    handleToChange(index, type, b, value) {
        let formattedData = this.state.formattedData;
        typeof formattedData[index][type] === 'object' ? formattedData[index][type].to = value : formattedData[index][type] = {};
        formattedData[index][type].to = value;
        this.setState({ ...this.state, formattedData });
        this.forceUpdate();
    }

    render() {
        const { formattedData } = this.state;
        let _self = this,
            formattedDataLength = formattedData.length;

        const { multi, multiValue, options } = this.state;
        return (
            <tbody className="query-section-table tbody">
                {
                    this.state.formattedData &&
                    this.state.formattedData.map(function (field, index) {
                        let fromDate = field.valueSelected ? field.valueSelected.from : "";
                        let toDate = field.valueSelected ? field.valueSelected.to : "";
                        let modifiers = { start: fromDate, end: toDate };
                        const ref = React.createRef();
                        return (
                            <tr key={index} className={"query-clause-row-" + index}>
                                <td scope="row" className={"query-clause-firstrow tableWidth-5"}>
                                    {
                                        formattedDataLength < 7 && <button ref={(input) => { _self.buttonRef[index] = input; }} onClick={_self.handleAddClause.bind(_self, index)} title="Insert new filter line" className="query-action-btn add"><i className="fa fa-plus"></i></button>

                                        ||

                                        <button ref={(input) => { _self.buttonRef[index] = input; }} title="Insert new filter line" className="query-action-btn add" type="button" disabled><i className="fa fa-plus"></i></button>
                                    }
                                    {
                                        formattedDataLength > 1 && <button onClick={_self.handleDeleteClause.bind(_self, index)} title="Remove this filter line" className="query-action-btn delete"><i className="fa fa-times"></i></button>

                                        ||

                                        <button title="Remove this filter line" className="query-action-btn delete" disabled><i className="fa fa-times"></i></button>
                                    }
                                </td>
                                <td className={"tableWidth-10"}>
                                    {
                                        index != 0 && <Select
                                            onOpen={_self.onOpenClose.bind()}
                                            clearable={false}
                                            autosize={false}
                                            isRtl={true}
                                            isSearchable={false}
                                            searchable={false}
                                            openOnClick={true}
                                            autoFocus={false}
                                            value={field.combinatorsSelected}
                                            options={field.combinators}
                                            onChange={_self.handleChange.bind("", index, "combinatorsSelected")}
                                            backspaceRemoves={false}
                                            deleteRemoves={false}
                                            placeholder={""}
                                        />
                                    }
                                </td>
                                <td className={"tableWidth-20"}>
                                    <Select
                                        onOpen={_self.onOpenClose.bind()}
                                        clearable={false}
                                        autosize={false}
                                        isRtl={true}
                                        className={"menu-outer-top"}
                                        menuShouldScrollIntoView={true}
                                        isSearchable={false}
                                        openOnClick={true}
                                        value={field.fieldsSelected}
                                        autoFocus={field.isFocus}
                                        maxMenuHeight={100}
                                        options={field.fields}
                                        menuPortalTarget={document.body}
                                        onChange={_self.handleChange.bind("", index, "fieldsSelected")}
                                        backspaceRemoves={false}
                                        deleteRemoves={false}
                                        placeholder={""}
                                    />
                                </td>
                                <td className={"tableWidth-20"}>
                                    <Select
                                        onOpen={_self.onOpenClose.bind()}
                                        clearable={false}
                                        autosize={false}
                                        isRtl={true}
                                        isSearchable={false}
                                        openOnClick={true}
                                        value={field.operatorsSelected}
                                        autoFocus={false}
                                        options={field.operators}
                                        onChange={_self.handleChange.bind("", index, "operatorsSelected")}
                                        backspaceRemoves={false}
                                        deleteRemoves={false}
                                        placeholder={""}
                                    />
                                </td>
                                <td>
                                    {
                                        (field.hasSmartParams == false && field.type != "date") && <div className={"query-value-input " + _self.getClassName(index)}>
                                            <Input
                                                type="text"
                                                name={field.label}
                                                placeholder={field.placeholder || ""}
                                                id={field.value}
                                                value={field.valueSelected}
                                                onChange={_self.handleInputChange.bind("", index, "valueSelected", this)}
                                                className="inputQueryInput"
                                            />
                                            <span className="help-block">{_self.getMessage(index)}</span>
                                        </div>

                                        ||

                                        (field.hasSmartParams == false && field.type == "date") && <div className={"day-picker-div " + index}>
                                            <DayPickerInput
                                                value={fromDate}
                                                placeholder="From"
                                                format="L"
                                                formatDate={formatDate}
                                                parseDate={parseDate}
                                                // overlayComponent={_self.CustomOverlay.bind(index, index+"@from")}
                                                keepFocus={false}
                                                component={
                                                    props =>
                                                        <Input
                                                            type="text"
                                                            className="inputQueryInput form-control" {...props} />
                                                }
                                                dayPickerProps={{
                                                    selectedDays: [fromDate, { from: fromDate, to: toDate }],
                                                    modifiers,
                                                    numberOfMonths: 1
                                                }}
                                                onDayChange={_self.handleFromChange.bind("", index, "valueSelected", this)}
                                            />
                                            {" "}—{" "}
                                            <DayPickerInput
                                                ref={el => (_self.toInputs[index] = el)}
                                                value={toDate}
                                                placeholder="To"
                                                format="L"
                                                formatDate={formatDate}
                                                parseDate={parseDate}
                                                // overlayComponent={_self.CustomOverlay.bind(index, index+"@to")}
                                                keepFocus={false}
                                                component={
                                                    props =>
                                                        <Input
                                                            type="text"
                                                            className="inputQueryInput form-control" {...props} />
                                                }
                                                dayPickerProps={{
                                                    selectedDays: [fromDate, { from: fromDate, to: toDate }],
                                                    modifiers,
                                                    numberOfMonths: 1
                                                }}
                                                onDayChange={_self.handleToChange.bind("", index, "valueSelected", this)}
                                            />
                                        </div>

                                        ||

                                        (field.hasSmartParams == true) && <SelectPlus.Creatable
                                            onOpen={_self.onOpenClose.bind()}
                                            autocomplete="off"
                                            clearable={false}
                                            autosize={false}
                                            backspaceRemoves={false}
                                            deleteRemoves={false}
                                            multi={multi}
                                            className="inputQueryInput"
                                            options={field.smartParam}
                                            onChange={_self.handleOnChange.bind("", index, "valueSelected", this)}
                                            name={field.label}
                                            placeholder={field.placeholder || ""}
                                            id={field.value}
                                            value={field.valueSelected}
                                        />
                                    }
                                </td>
                            </tr>
                        )
                    })
                }
                {
                    formattedDataLength < 7 && <tr key={"n"} className={"query-clause-row-n"}>
                        <td scope="row" className="tableWidth-5">
                            <button onClick={this.handleAddClause.bind(this, "n")} title="Add new clause" className="query-action-btn add"><i className="fa fa-plus"></i></button>
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                }
            </tbody>
        );
    }
}

export default withCookies(QueryClause); 