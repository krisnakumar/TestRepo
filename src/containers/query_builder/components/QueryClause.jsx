/* eslint-disable */
/*
* QueryClause.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details to list the workbooks 
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------

*/
import React, { PureComponent } from 'react';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import { Input } from 'reactstrap';
import Select from 'react-select';
import 'whatwg-fetch'
import * as API from '../../../shared/utils/APIUtils';
import FieldData from './../data';
import classNames from 'classnames';
import FormValidator from '../../../shared/utils/FormValidator';

class QueryClause extends PureComponent {

    static propTypes = {
        cookies: instanceOf(Cookies).isRequired
    };

    constructor(props) {
        super(props);
        // create a ref to store the textInput DOM element
        this.buttonRef = [];

        this.validator = new FormValidator(this.formatValidationData(this.formatRowData(this.props.fieldData)));

        this.state = {
            fieldData: this.props.fieldData,
            formattedData: this.formatRowData(this.props.fieldData),
            validation: this.validator.valid(),
            entity: this.props.entity
        };
        
        this.submitted = false;

        this.handleChange = this.handleChange.bind(this); 
        this.handleInputChange = this.handleInputChange.bind(this); 
        this.handleAddClause = this.handleAddClause.bind(this);        
        this.handleDeleteClause = this.handleDeleteClause.bind(this);
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
        if(this.state.entity != newProps.entity){            
            this.state.entity = newProps.entity;      
            this.changeQueryClause(newProps.entity);
        }
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
    resetQueryClause(){        
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
    changeQueryClause(selectedEntity){      
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
    formatValidationData(formattedData){
        let validationData = [];
		formattedData.forEach(function(element, index) {
			let obj = { 
						field: index + "+" + element.value,
						method: element.validation, 
						validWhen: false, 
						message: element.errormessage || 'This field is required.' 
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
    formatRowData(unFormattedData){
        let formattedData = [],
            entity = this.state ? this.state.entity : this.props.entity;

        unFormattedData.map(function (field, index) {
            let obj = {},
                type = field.type != "int" ? "others" : "int";

            obj.label = field.label;
            obj.type = field.type;
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
    handleChange(index, key, selectedOption){
        let formattedData = this.state.formattedData;
        formattedData[index][key] = selectedOption;
        switch(key) {
            case "fieldsSelected":
                let type = selectedOption ? (selectedOption.type != "int" ? "others" : "int") : "int";
                formattedData[index].operators =  FieldData.operator[type];
                break;
            default:
                // Don nothing
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
    handleInputChange(index, key, selectedOption, ele){
        ele.preventDefault();
        let formattedData = this.state.formattedData;
        formattedData[index][key] = ele.target.value || "";
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
    handleAddClause(index){

        if(index != "n")
            this.buttonRef[index].blur();

        let formattedData = this.state.formattedData,
            entity = this.state ? this.state.entity : this.props.entity;

        formattedData.forEach(function(element, index) {
            formattedData[index].isFocus = false;
        });

        let obj = {};
            obj.label = "";
            obj.type = "int";
            obj.validation = "isEmpty";
            obj.value = "";
            obj.combinators = FieldData.combinators;
            obj.fields = FieldData.field[entity];
            obj.operators = FieldData.operator.int;
            obj.valueSelected = "";
            obj.isFocus = true;
            obj.combinatorsSelected = FieldData.combinators[0];
            obj.fieldsSelected = FieldData.field[entity][0];
            obj.operatorsSelected = FieldData.operator.int[0];

        if(index == "n"){
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
      handleDeleteClause(index){
        let currentIndex = index + 1,
             formattedData = this.state.formattedData,
             formattedDataLength = formattedData.length,
             obj = {},
             entity = this.state ? this.state.entity : this.props.entity;
             
        // Delete by Index from Array
        formattedData = formattedData.slice(0, currentIndex-1).concat(formattedData.slice(currentIndex, formattedData.length));

        if(index == 0 && formattedDataLength <= 1){

            obj.label = "";
            obj.type = "int";
            obj.validation = "isEmpty";
            obj.value = "";
            obj.combinators = FieldData.combinators;
            obj.fields = FieldData.field[entity];
            obj.operators = FieldData.operator.int;
            obj.valueSelected = "";
            obj.isFocus = true;
            obj.combinatorsSelected = FieldData.combinators[0];
            obj.fieldsSelected = FieldData.field[entity][0];
            obj.operatorsSelected = FieldData.operator.int[0];

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
        const validation = this.validator.validate(this.state.formattedData);
        this.setState({ validation });
        this.submitted = true;

        if (validation.isValid) {
            // handle actual form submission here
            let queryClause = [],
                _self = this;
                
            this.state.formattedData.forEach(function(element, index){
                let queryObj = {};
                queryObj.Value = _self.state.formattedData[index].valueSelected;
                queryObj.Operator = _self.state.formattedData[index].operatorsSelected.value;
                queryObj.Name = _self.state.formattedData[index].fieldsSelected.field;    
                if(index == 0){
                    queryObj.Bitwise = "";
                }else{
                    queryObj.Bitwise = _self.state.formattedData[index] ? _self.state.formattedData[index].combinatorsSelected.value : "";
                }            
                

                queryClause.push(queryObj)
            })

            switch (this.state.entity) {
                case 'employees':
                    this.getEmployeesResults(queryClause);
                    break;
                case 'workbooks':
                    this.getWorkbooksResults(queryClause);
                    break;
                default:
                  console.log('Sorry, we are out of options');
              }                       
        }
    };

    /**
     * @method
     * @name - getClassName
     * This method will used to get Class name from validation
     * @param index
     * @returns Class name or ""
    */
    getClassName(index){
        const { formattedData } = this.state;
        let validation = this.submitted ?           // if the form has been submitted at least once
        this.validator.validate(formattedData) :   // then check validity every time we render
        this.state.validation;

        return classNames('form-group-has-validation', {'has-error': validation[index] ? validation[index].isInvalid : false});
    };

    /**
     * @method
     * @name - getMessage
     * This method will used to get error Message from validation
     * @param index
     * @returns Message or ""
    */
    getMessage(index){
        const { formattedData } = this.state;
        let validation = this.submitted ?           // if the form has been submitted at least once
        this.validator.validate(formattedData) :   // then check validity every time we render
        this.state.validation;

        return  validation[index] ? validation[index].message : "";
    };

    /**
     * @method
     * @name - getEmployeesResults
     * This method will used to get Employees Results from the Query Clause
     * @param requestData
     * @returns none
    */
    async getEmployeesResults(requestData){
        const { cookies } = this.props;

        let payLoad = {"Fields": requestData,
                    "ColumnList":["EMPLOYEE_NAME","ROLE","USERNAME","ALTERNATE_USERNAME","TOTAL_EMPLOYEES","EMAIL"]};

        let token = cookies.get('IdentityToken'),
            companyId = cookies.get('CompanyId'),
            url = "https://4326ra7t2l.execute-api.us-west-2.amazonaws.com/dev/company/"+companyId+"/employees",
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
    async getWorkbooksResults(requestData){
        const { cookies } = this.props;

        let payLoad = {"Fields": requestData,"ColumnList":["WORKBOOK_ID","WORKBOOK_NAME","DESCRIPTION","WORKBOOK_CREATED_BY","DAYS_TO_COMPLETE"]};

        let token = cookies.get('IdentityToken'),
            companyId = cookies.get('CompanyId'),
            url = "https://4326ra7t2l.execute-api.us-west-2.amazonaws.com/dev/company/"+companyId+"/workbooks",
            response = await API.ProcessAPI(url, payLoad, token, false, "POST", true);

        this.props.passWorkbooksResults(response);
    };
    
    render() {
        const { formattedData } = this.state;
        let _self = this;
        
        return (
            <tbody>
                {
                    this.state.formattedData &&
                    this.state.formattedData.map(function (field, index) {
                        return (                           
                           <tr key={index} className={"query-clause-row-"+index}>
                                <td scope="row" className={"query-clause-firstrow tableWidth-5"}>
                                    <button ref={(input) => { _self.buttonRef[index] = input; }} onClick={_self.handleAddClause.bind(_self, index)} title="Insert new filter line" className="query-action-btn add"><i className="fa fa-plus"></i></button>
                                    <button onClick={_self.handleDeleteClause.bind(_self, index)} title="Remove this filter line" className="query-action-btn delete"><i className="fa fa-times"></i></button>
                                </td>
                                <td className={"tableWidth-10"}> 
                                    { 
                                        index != 0 && <Select
                                            clearable={false}
                                            autosize={false}
                                            isRtl={true}
                                            isSearchable={false}
                                            openOnClick={false}
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
                                        clearable={false}
                                        autosize={false}
                                        isRtl={true}
                                        isSearchable={false}
                                        openOnClick={false}
                                        value={field.fieldsSelected}
                                        autoFocus={field.isFocus}
                                        options={field.fields}
                                        onChange={_self.handleChange.bind("", index, "fieldsSelected")}
                                        backspaceRemoves={false}
                                        deleteRemoves={false}
                                        placeholder={""}
                                    /> 
                                </td>
                                <td className={"tableWidth-20"}> 
                                    <Select
                                        clearable={false}
                                        autosize={false}
                                        isRtl={true}
                                        isSearchable={false}
                                        openOnClick={false}
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
                                    <div className={_self.getClassName(index)}>
                                        <Input 
                                            type="text" 
                                            name={field.label} 
                                            id={field.value}
                                            value={field.valueSelected}
                                            onChange={_self.handleInputChange.bind("", index, "valueSelected", this)}
                                            className="inputQueryInput"
                                        />
                                        <span className="help-block">{_self.getMessage(index)}</span>
                                    </div>
                                </td>
                            </tr>
                        )
                    })
                }
                <tr key={"n"} className={"query-clause-row-n"}>
                    <td scope="row">
                        <button onClick={this.handleAddClause.bind(this, "n")} title="Add new clause" className="query-action-btn add"><i className="fa fa-plus"></i></button>
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
        </tbody>
        );
    }
}

export default withCookies(QueryClause);
