/* eslint-disable */
/*
* TaskExport.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Task Export details to list the Task 
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------
componentWillReceiveProps(newProps)
*/
import Workbook from 'react-excel-workbook'
import React, { Component } from 'react';
import ReactExport from "react-data-export";
import * as moment from 'moment';

const ExcelFile = ReactExport.ExcelFile;
const ExcelSheet = ReactExport.ExcelFile.ExcelSheet;
const ExcelColumn = ReactExport.ExcelFile.ExcelColumn;

class TaskExport extends Component {
    constructor(props) {
        super(props);
        this.state = {
            tasks: this.props.tasks,
            entity: this.props.entity
        };
    };

    /**
     * @method
     * @name - componentWillReceiveProps
     * This method will invoked whenever the props or state
     *  is update to this component class
     * @param newProps
     * @returns none
     */
    componentWillReceiveProps(newProps) {       
        this.setState({
            tasks: newProps.tasks,
            entity: newProps.entity
        });
    };

    render() {
        let { entity } = this.state,
            excelData = this.state[entity],
            isExcelData = excelData.length > 0 ? true : false,
            date = moment().format('MM.DD.YYYY');        
        
        return (isExcelData && <ExcelFile element={
            <button className="query-section-button" size="sm" title="Export" aria-label="Export">
                <span aria-hidden className="fa-icon-size" ><i className="fa fa-file-excel-o"></i></span>
                <span className="fa-text-align">Export</span>
            </button>
        } filename={"Industrial Training Services, Inc. -Task Report " + date} fileExtension="xlsx">
        <ExcelSheet data={excelData} name="OnBoard LMS Task Report">
            <ExcelColumn label="Task Id" value="TaskId" />
            <ExcelColumn label="Task Name" value="TaskName" />
            <ExcelColumn label="Assigned To" value="AssignedTo" />
            <ExcelColumn label="Evaluator Name" value="EvaluatorName" />
            <ExcelColumn label="Expiration Date" value="ExpirationDate" />
        </ExcelSheet>
    </ExcelFile>);
    }
}

export default TaskExport;
