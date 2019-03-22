/* eslint-disable */
/*
* CTDashboardExport.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render CTDashboardExport details into xlsx file
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

const ExcelFile = ReactExport.ExcelFile;
const ExcelSheet = ReactExport.ExcelFile.ExcelSheet;
const ExcelColumn = ReactExport.ExcelFile.ExcelColumn;

class CTDashboardExport extends Component {
    constructor(props) {
        super(props);
        this.state = {
            data: this.formatData(this.props.data, this.props.heads),
            heads: this.props.heads || [],
            sheetName: this.props.sheetName || "",
        };
        this.formatData = this.formatData.bind(this);
    };

    /**
     * @method
     * @name - componentWillReceiveProps
     * This method will be invoked whenever the props or state
     *  is update to this component class
     * @param newProps
     * @returns none
    */
    componentWillReceiveProps(newProps) {
        this.setState({
            sheetName: newProps.sheetName || "",
            data: this.formatData(newProps.data, newProps.heads),
            heads: newProps.heads || []
        });
    };

    /**
     * @method
     * @name - formatData
     * This method used to format the employees data to export in excel
     * @param employeeData
     * @returns multiDataSet
    */
    formatData(data, heads) {
        const { cookies } = this.props;
        let runByUser = cookies.get('UserName') || "",
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

        if (data.length > 0) {

            let columnKeys = [],
                columnNames = [],
                dataSet = [];

            heads.forEach(function (column, empIndex) {
                columnNames.push(column.name);
                columnKeys.push(column.key);
            });

            data.forEach(function (value, index) {
                let tempDataSet = [];
                columnKeys.forEach(function (keyValue, keyIndex) {
                    tempDataSet.push(data[index][keyValue]);
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
        let { sheetName } = this.state,
            excelData = this.state.data,
            hasExcelData = excelData[1].data.length > 0 ? true : false,
            date = moment().format('MM.DD.YYYY');

        return (
            hasExcelData && <ExcelFile element={
                <div className="export-menu-right">
                    <a href="javascript:void(0);" id="ctl00_exportExcel" className="exportExcel"><img src="https://d2vkqsz7y0fh3j.cloudfront.net/img/excel_icon.jpg" />Excel</a>  
                </div>
                    } filename={"Industrial Training Services, Inc. Training Dashboard " + date} fileExtension="xlsx">
                <ExcelSheet dataSet={excelData} name={sheetName} />
            </ExcelFile>
        );
    }
}

export default withCookies(CTDashboardExport);
