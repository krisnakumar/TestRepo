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
import { Input, Table, CardBody, Button, Container, Row, Col } from 'reactstrap';
import Select from 'react-select';
import FieldData from './../data';

class QueryClause extends PureComponent {

    static propTypes = {
        cookies: instanceOf(Cookies).isRequired
    };

    constructor(props) {
        super(props);
        // create a ref to store the textInput DOM element
        this.buttonRef = [];
        this.state = {
            fieldData: this.props.fieldData,
            formattedData: this.formatRowData(this.props.fieldData)
            
        };
        this.handleChange = this.handleChange.bind(this); 
        this.handleInputChange = this.handleInputChange.bind(this); 
        this.handleAddClause = this.handleAddClause.bind(this);        
        this.handleDeleteClause = this.handleDeleteClause.bind(this);
        this.unBlurElement = this.unBlurElement.bind(this);
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


    formatRowData(unFormattedData){
        let formattedData = [];

        unFormattedData.map(function (field, index) {

            let obj = {};

            obj.label = field.label;
            obj.type = field.type;
            obj.validation = field.validation;
            obj.value = field.value;
            obj.combinators = FieldData.combinators;
            obj.fields = FieldData.field.employees;
            obj.operators = FieldData.operator.int;
            obj.valueSelected = "";
            obj.isFocus = false;
            obj.combinatorsSelected = FieldData.combinators[0];
            obj.fieldsSelected = FieldData.field.employees[index];
            obj.operatorsSelected = FieldData.operator.int[0];

            formattedData.push(obj);
        });

        return formattedData;
    }
    
    handleChange(index, key, selectedOption){
        let formattedData = this.state.formattedData;
        formattedData[index][key] = selectedOption;
        this.setState({ ...this.state, formattedData });
        this.forceUpdate();
    }

    handleInputChange(index, key, selectedOption, ele){
        ele.preventDefault();
        let formattedData = this.state.formattedData;
        formattedData[index][key] = ele.target.value || "";
        this.setState({ ...this.state, formattedData });
        this.forceUpdate();
    }

    handleAddClause(index){

        if(index != "n")
            this.buttonRef[index].blur();

        let formattedData = this.state.formattedData;

            formattedData.forEach(function(element, index) {
                formattedData[index].isFocus = false;
            });

        let obj = {};
            obj.label = "";
            obj.type = "int";
            obj.validation = "NONE";
            obj.value = "";
            obj.combinators = FieldData.combinators;
            obj.fields = FieldData.field.employees;
            obj.operators = FieldData.operator.int;
            obj.valueSelected = "";
            obj.isFocus = true;
            obj.combinatorsSelected = FieldData.combinators[0];
            obj.fieldsSelected = null;
            obj.operatorsSelected = null;

        if(index == "n"){
            formattedData.push(obj);
        } else {
            formattedData.splice(index, 0, obj);
            // inserts at 1st index position
        }
        
        this.setState({ ...this.state, formattedData });
        this.forceUpdate();
        
      };

      handleDeleteClause(index){
        let currentIndex = index + 1,
             formattedData = this.state.formattedData,
             formattedDataLength = formattedData.length,
             obj = {};
             
        // Delete by Index from Array
        formattedData = formattedData.slice(0, currentIndex-1).concat(formattedData.slice(currentIndex, formattedData.length));

        if(index == 0 && formattedDataLength <= 1){

            obj.label = "";
            obj.type = "int";
            obj.validation = "NONE";
            obj.value = "";
            obj.combinators = FieldData.combinators;
            obj.fields = FieldData.field.employees;
            obj.operators = FieldData.operator.int;
            obj.valueSelected = "";
            obj.isFocus = true;
            obj.combinatorsSelected = FieldData.combinators[0];
            obj.fieldsSelected = null;
            obj.operatorsSelected = null;

            formattedData.push(obj);
        }

        this.setState({ ...this.state, formattedData });
        this.forceUpdate();
      };

      unBlurElement = (event, e, a, data) => {
            // access to e.target here
            // event.currentTarget.preventDefault();
           
            console.log(event, e, a, data);
        }

    render() {
        const { formattedData } = this.state;
        let _self = this;
        return (
            <tbody>
                {
                    formattedData &&
                    formattedData.map(function (field, index) {
                        return (                           
                           <tr key={index} className={"query-clause-row-"+index}>
                                <td scope="row">
                                    <button ref={(input) => { _self.buttonRef[index] = input; }} onClick={_self.handleAddClause.bind(_self, index)} title="Insert new filter line" className="query-action-btn add"><i className="fa fa-plus"></i></button>
                                    <button onClick={_self.handleDeleteClause.bind(_self, index)} title="Remove this filter line" className="query-action-btn delete"><i className="fa fa-times"></i></button>
                                </td>
                                <td>
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
                                            placeholder={""}
                                        /> 
                                    }
                                </td>
                                <td> 
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
                                        placeholder={""}
                                    /> 
                                </td>
                                <td> 
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
                                        placeholder={""}
                                    /> 
                                </td>
                                <td>
                                    <Input 
                                        type="text" 
                                        name={field.label} 
                                        id={field.value}
                                        value={field.valueSelected}
                                        onChange={_self.handleInputChange.bind("", index, "valueSelected", this)}
                                    />
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