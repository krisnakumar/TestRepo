/* eslint-disable */
import Workbook from 'react-excel-workbook'
import React, { Component } from 'react';

class EmployeeExport extends Component {
    constructor(props) {
        super(props);
        this.state = {
            employees: this.props.employees,
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
            employees: newProps.employees,
            entity: newProps.entity
        });
    };

    render() {
        let { entity } = this.state,
            excelData = this.state[entity];

        return (<Workbook filename={entity+".xlsx"} element={
            <button className="query-section-button" size="sm" title={"Open in Excel"+entity} aria-label="Open in Excel">
                <span aria-hidden className="fa-icon-size" ><i className="fa fa-undo"></i></span>
                <span className="fa-text-align">Open in Excel</span>
            </button>
        }>
            <Workbook.Sheet data={() => excelData} name={entity}>
                <Workbook.Column label="Employee Name" value="EmployeeName" />
                <Workbook.Column label="Role" value="Role" />
                <Workbook.Column label="User Id" value="UserId" />
                <Workbook.Column label="Username" value="UserName" />
                <Workbook.Column label="Email" value="Email" />
                <Workbook.Column label="Alternative Name" value="AlternateName" />
                <Workbook.Column label="Total Employees" value="TotalEmployees" />
            </Workbook.Sheet>
        </Workbook>);
    }
}

export default EmployeeExport;
