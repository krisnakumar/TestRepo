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
import { withCookies, Cookies } from 'react-cookie';
import FieldData from './../data';

const ExcelFile = ReactExport.ExcelFile;
const ExcelSheet = ReactExport.ExcelFile.ExcelSheet;
const ExcelColumn = ReactExport.ExcelFile.ExcelColumn;

class WorkbookExport extends Component {
    constructor(props) {
        super(props);
        this.state = {
            workbooks: this.formatData(this.props.workbooks),
            entity: this.props.entity,
            columnOptions: this.props.columnOptions
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
            workbooks: this.formatData(newProps.workbooks),
            entity: newProps.entity,
            columnOptions: newProps.columnOptions
        });
    };

    /**
     * @method
     * @name - formatData
     * This method used to format the workbook data to export in excel
     * @param workbookData
     * @returns multiDataSet
    */
    formatData(workbookData) {
        const { cookies } = this.props;
        let fieldDataColumns = FieldData.columns.workbooks;
        let workbooks = workbookData || [],
            runByUser = cookies.get('UserName') || "",
            runByDateTime = moment().format('MM/DD/YYYY hh:mm:ss A'),
            userDetails = "Run By " + runByUser + " " + runByDateTime;

        let multiDataSet = [
            {
                columns: [userDetails],
                data: []
            },
            {
                xSteps: 0, // Will start putting cell with 1 empty cell on left most
                ySteps: 1, //will put space of 5 rows,
                columns: [],
                data: []
            }
        ];

        if (workbooks.length > 0) {

            let columns = [],
                columnNames = [],
                dataSet = [],
                currentColumnOptions = this.state.columnOptions;

            currentColumnOptions.forEach(function (column, empIndex) {
                fieldDataColumns.forEach(function (value, index) {
                    if (column == value.fields) {
                        columns.push(value.id);
                        columnNames.push(value.label);
                    }
                    return;
                });
            });

            workbooks.forEach(function (workbookValue, workbookIndex) {
                let tempDataSet = [];
                columns.forEach(function (value, index) {
                    tempDataSet.push(workbooks[workbookIndex][value]);
                    return;
                });
                dataSet.push(tempDataSet);
            });

            multiDataSet[1].columns = columnNames;
            multiDataSet[1].data = dataSet;
        }

        return multiDataSet;
    };

    render() {
        let { entity } = this.state,
            excelData = this.state[entity],
            hasExcelData = excelData[1].data.length > 0 ? true : false,
            date = moment().format('MM.DD.YYYY');

        return (
            hasExcelData && <ExcelFile element={
                <button className="query-section-button" size="sm" title="Export" aria-label="Export">
                    <span aria-hidden className="fa-icon-size" ><i className="fa fa-file-excel-o"></i></span>
                    <span className="fa-text-align">Export</span>
                </button>
            } filename={"Industrial Training Services, Inc. Workbook Report " + date} fileExtension="xlsx">
                <ExcelSheet dataSet={excelData} name="OnBoard LMS Workbook Report" />
            </ExcelFile>
        );
    }
}

export default withCookies(WorkbookExport);
