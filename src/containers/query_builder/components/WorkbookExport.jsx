/* eslint-disable */
/*
* WorkbookExport.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook Export details to list the Workbook 
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------
componentWillReceiveProps(newProps)
*/
import React, { Component } from 'react';
import ReactExport from "react-data-export";
import * as moment from 'moment';

const ExcelFile = ReactExport.ExcelFile;
const ExcelSheet = ReactExport.ExcelFile.ExcelSheet;
const ExcelColumn = ReactExport.ExcelFile.ExcelColumn;

class WorkbookExport extends Component {
    constructor(props) {
        super(props);
        this.state = {
            workbooks: this.props.workbooks,
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
            workbooks: newProps.workbooks,
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
        } filename={"Industrial Training Services, Inc. -Workbook Report " + date} fileExtension="xlsx">
        <ExcelSheet data={excelData} name="OnBoard LMS Workbook Report">
            <ExcelColumn label="Workbook Id" value="WorkBookId" />
            <ExcelColumn label="Workbook" value="WorkBookName" />
            <ExcelColumn label="Description" value="Description" />
            <ExcelColumn label="Created By" value="CreatedBy" />
            <ExcelColumn label="Days To Complete" value="DaysToComplete" />
        </ExcelSheet>
    </ExcelFile>);
    }
}

export default WorkbookExport;
