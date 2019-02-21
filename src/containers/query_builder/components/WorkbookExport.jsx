/* eslint-disable */
import Workbook from 'react-excel-workbook'
import React, { Component } from 'react';

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
            isExcelData = excelData.length > 0 ? true : false;

        return (isExcelData && <Workbook filename={entity+".xlsx"} element={
            <button className="query-section-button" size="sm" title={"Open in Excel"} aria-label="Open in Excel">
                <span aria-hidden className="fa-icon-size" ><i className="fa fa-undo"></i></span>
                <span className="fa-text-align">Open in Excel</span>
            </button>
        }>
             <Workbook.Sheet data={() => excelData} name={entity}>
                <Workbook.Column label="Workbook Id" value="WorkBookId" />
                <Workbook.Column label="Workbook" value="WorkBookName" />
                <Workbook.Column label="Description" value="Description" />
                <Workbook.Column label="Created By" value="CreatedBy" />
                <Workbook.Column label="Days To Complete" value="DaysToComplete" />
            </Workbook.Sheet>
        </Workbook>);
    }
}

export default WorkbookExport;
