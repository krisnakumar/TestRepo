/* eslint-disable */
import Workbook from 'react-excel-workbook'
import React, { Component } from 'react';

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
            isExcelData = excelData.length > 0 ? true : false;

        return (isExcelData && <Workbook filename={entity+".xlsx"} element={
            <button className="query-section-button" size="sm" title={"Open in Excel"} aria-label="Open in Excel">
                <span aria-hidden className="fa-icon-size" ><i className="fa fa-undo"></i></span>
                <span className="fa-text-align">Open in Excel</span>
            </button>
        }>
            <Workbook.Sheet data={() => excelData} name={entity}>
                <Workbook.Column label="Task Id" value="TaskId" />
                <Workbook.Column label="Task Name" value="TaskName" />
                <Workbook.Column label="Assigned To" value="AssignedTo" />
                <Workbook.Column label="Evaluator Name" value="EvaluatorName" />
                <Workbook.Column label="Expiration Date" value="ExpirationDate" />
            </Workbook.Sheet>
        </Workbook>);
    }
}

export default TaskExport;
