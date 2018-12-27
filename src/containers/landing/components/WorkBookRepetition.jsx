/* eslint-disable */
/*
* WorkBookProgress.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Workbook details to list the workbooks progress details
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
import React from 'react';
import { Modal, ModalHeader, ModalBody } from 'reactstrap';
import 'whatwg-fetch'
import ReactDataGrid from 'react-data-grid';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';

/**
 * WorkBookProgressEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the  react data grid module.
 */
class WorkBookRepetitionEmptyRowsView extends React.Component{
  render() {
    return (<div className="no-records-found-modal">Sorry, no records</div>)
  }
};

class WorkBookRepetition extends React.Component {
  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor(props) {
    super(props);
    this.heads = [
      {
        key: 'attempt',
        name: 'Attempt',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
        },
        {
        key: 'status',
        name: 'Complete/Incomplete',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-left"
        },
        {
        key: 'dateTime',
        name: 'Last Attempted Date',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-center"
        },
        {
        key: 'location',
        name: 'Location',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-center"
        },
        {
        key: 'evaluator',
        name: 'Evaluator',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-center"
        },
        {
        key: 'comments',
        name: 'Comments',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: this.cellFormatter,
        cellClass: "text-center last-column"
        }
      ];
    
    this.state = {
      modal: this.props.modal,      
      rows: this.createRows(this.props.workBooksProgress),
      pageOfItems: [],
      isInitial: false,
      selectedWorkbook: this.props.selectedWorkbook
    };
    this.toggle = this.toggle.bind(this);
  }

  /**
   * @method
   * @name - cellFormatter
   * This method will format the cell column other than workbooks Data Grid
   * @param props
   * @returns none
   */
  cellFormatter = (props) => {
    return (
      <span>{props.value}</span>
    );
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
   * @name - createRows
   * This method will format the input data
   * for Data Grid
   * @param workbooks
   * @returns rows
   */
  createRows = (workbooks) => {
    const rows = [], 
          length = workbooks ? workbooks.length : 0;
    for (let i = 0; i < length; i++) { 
      rows.push({
        userId:  workbooks[i].UserId || 0,
        attempt: workbooks[i].NumberofAttempts || 0,
        status: workbooks[i].Status,
        dateTime: workbooks[i].LastAttemptDate,
        location: workbooks[i].Location,
        evaluator: workbooks[i].EvaluatorName,
        comments: workbooks[i].Comments
      });
    }

    return rows;
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
      let rows = this.createRows(newProps.workBooksRepetition),
          isArray = Array.isArray(newProps.workBooksRepetition),
          isInitial = isArray;
      this.setState({
        modal: newProps.modal,
        rows: rows,
        isInitial: isInitial,
        selectedWorkbook: newProps.selectedWorkbook
      });
  }

  /**
   * @method
   * @name - toggle
   * This method will update the current of modal window
   * @param workbooks
   * @returns none
   */
  toggle() {
    this.setState({
      modal: !this.state.modal
    });
    this.props.updateState("isWorkBookRepetitionModal");
  }

  /**
   * @method
   * @name - handleGridRowsUpdated
   * This method will update the rows of grid of the current Data Grid
   * @param fromRow
   * @param toRow
   * @param updated
   * @returns none
   */
  handleGridRowsUpdated = ({ fromRow, toRow, updated }) => {
    const rows = this.state.rows.slice();

    for (let i = fromRow; i <= toRow; i += 1) {
      const rowToUpdate = rows[i];
      rows[i] = update(rowToUpdate, { $merge: updated });
    }

    this.setState({ rows });
  };

  /**
   * @method
   * @name - handleGridSort
   * This method will update the rows of grid of Data Grid after the sort
   * @param sortColumn
   * @param sortDirection
   * @returns none
   */
  handleGridSort = (sortColumn, sortDirection) => {
    const comparer = (a, b) => {
      if (sortDirection === 'ASC') {
        return (a[sortColumn] > b[sortColumn]) ? 1 : -1;
      } else if (sortDirection === 'DESC') {
        return (a[sortColumn] < b[sortColumn]) ? 1 : -1;
      }
    };

    const sortRows = this.state.rows.slice(0);
    const rows = sortDirection === 'NONE' ? this.state.rows.slice(0, 10) : sortRows.sort(comparer).slice(0, 10);

    this.setState({ rows });
  };

  // This method is used to setting the row data in react data grid
  rowGetter = i => this.state.rows[i];

  render() {
    const { rows } = this.state;
    return (
      <div>
        <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal}  fade={false}  toggle={this.toggle} centered={true} className="custom-modal-grid">
          <ModalHeader toggle={this.toggle}>Workbook Repetition</ModalHeader>
          <ModalBody>
          <div className="grid-description"> 
            <h5 className="pad-bt-10">{this.state.selectedWorkbook ? this.state.selectedWorkbook.workbookName : ""} | {this.state.selectedWorkbook ? this.state.selectedWorkbook.percentageCompleted : ""}</h5>
            <h6 className="pad-bt-10">{this.state.selectedWorkbook ? this.state.selectedWorkbook.taskCode : ""} {this.state.selectedWorkbook ? this.state.selectedWorkbook.taskName : ""}</h6>
            <h5 className="pad-bt-10">{this.state.selectedWorkbook ? this.state.selectedWorkbook.employee : ""}, {this.state.selectedWorkbook ? this.state.selectedWorkbook.role : ""}</h5>
          </div>           
          <div className="grid-container">
              <div className="table">
                  <ReactDataGrid
                      ref={'reactDataGrid'}
                      onGridSort={this.handleGridSort}
                      enableCellSelect={false}
                      enableCellAutoFocus={false}
                      columns={this.heads}
                      rowGetter={this.rowGetter}
                      rowsCount={rows.length}
                      onGridRowsUpdated={this.handleGridRowsUpdated}
                      rowHeight={35}
                      minColumnWidth={100}
                      emptyRowsView={this.state.isInitial && WorkBookRepetitionEmptyRowsView} 
                  />
              </div>
            </div>
          </ModalBody>
        </Modal>
      </div>
    );
  }
}

export default withCookies(WorkBookRepetition);