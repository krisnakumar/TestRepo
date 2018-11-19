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
import WorkBookRepetition from './WorkBookRepetition';
import * as API from '../../../shared/utils/APIUtils';


/**
 * WorkBookProgressEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react-table module.
 */
class WorkBookProgressEmptyRowsView extends React.Component{
  render() {
    return (<div className="no-records-found-modal">Sorry, no records</div>)
  }
};

class WorkBookProgress extends React.Component {
  static propTypes = {
    cookies: instanceOf(Cookies).isRequired
  };

  constructor(props) {
    super(props);
    this.heads = [
      {
        key: 'taskCode',
        name: 'Task Code',
        sortable: true,
        width: 300,
        editable: false,
        cellClass: "text-left"
        },
        {
        key: 'taskName',
        name: 'Task Name',
        sortable: true,
        editable: false,
        cellClass: "text-left"
        },
        {
        key: 'completedTasksCount',
        name: 'Completed / Total Tasks',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.workbookFormatter("completedTasksCount", props),
        cellClass: "text-center"
        },
        {
        key: 'incompletedTasksCount',
        name: 'Incomplete Tasks',
        sortable: true,
        editable: false,
        getRowMetaData: row => row,
        formatter: (props) => this.workbookFormatter("incompletedTasksCount", props),
        cellClass: "text-center"
        },
        {
        key: 'completionPrecentage',
        name: 'Percentage Completed',
        sortable: true,
        editable: false,
        cellClass: "text-center last-column"
        }
      ];
    
    this.state = {
      modal: this.props.modal,      
      rows: this.createRows(this.props.workBooksProgress),
      pageOfItems: [],
      isWorkBookRepetitionModal: false,
      workBooksRepetition: {},
      isInitial: false
    };
    this.toggle = this.toggle.bind(this);
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
        userId: workbooks[i].UserId,
        workBookId: workbooks[i].WorkbookId,
        taskId: workbooks[i].TaskId,
        taskCode: workbooks[i].TaskCode,
        taskName: workbooks[i].TaskName,
        completedTasksCount: workbooks[i].CompletedTasksCount + "/" + workbooks[i].TotalTasks,
        incompletedTasksCount: workbooks[i].IncompletedTasksCount,
        completionPrecentage: workbooks[i].CompletionPrecentage + "%"
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
      let rows = this.createRows(newProps.workBooksProgress),
          isArray = Array.isArray(newProps.workBooksProgress),
          isInitial = isArray;
      this.setState({
        modal: newProps.modal,
        rows: rows,
        isInitial: isInitial
      });
  }

  /**
   * @method
   * @name - getWorkbookRepetitions
   * This method will used to get Workbook Repetitions details
   * @param userId
   * @param workBookId
   * @param taskId
   * @returns none
   */
  async getWorkbookRepetitions(userId, workBookId, taskId){
    const { cookies } = this.props;

    let isWorkBookRepetitionModal = this.state.isWorkBookRepetitionModal,
        workBooksRepetition = {};
    isWorkBookRepetitionModal = true;
    this.setState({ isWorkBookRepetitionModal, workBooksRepetition });

    let token = cookies.get('IdentityToken'),
        url = "https://klrg45ssob.execute-api.us-west-2.amazonaws.com/dev/users/"+ userId +"/assigned-workbooks/"+ workBookId +"/tasks/" + taskId + "/attempts",
        response = await API.ProcessAPI(url, "", token, false, "GET", true);

    workBooksRepetition = response;
    isWorkBookRepetitionModal = true;
    this.setState({ ...this.state, isWorkBookRepetitionModal, workBooksRepetition });
  };

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
    this.props.updateState("isWorkBookProgressModal");
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

  /**
   * @method
   * @name - updateModalState
   * This method will update the modal window state of parent
   * @param modelName
   * @returns none
   */
  updateModalState = (modelName) => {
    let value = !this.state[modelName];
    this.setState({
      [modelName]: value
    });
  };

   /**
   * @method
   * @name - handleCellClick
   * This method will trigger the event of API's respective to cell clicked Data Grid
   * @param type
   * @param args
   * @returns none
   */
  handleCellClick = (type, args) => {
    let userId = 0,
        workBookId = 0,
        taskId = 0;
    switch(type) {
      case "incompletedTasksCount":
      case "completedTasksCount":
          userId = args.userId;
          workBookId = args.workBookId;
          taskId = args.taskId;
          if(userId && workBookId && taskId)
            this.getWorkbookRepetitions(userId, workBookId, taskId);
          break;
      default:
          break;
    }
    this.refs.reactDataGrid.deselect();
  };

  workbookFormatter = (type, props) => {
    if(props.dependentValues.employee == "Total"){
      return (
        <span>{props.value}</span>
      );
    } else {
      return (
       <span onClick={e => { e.preventDefault(); this.handleCellClick(type, props.dependentValues); }} className={"text-clickable"}>    
        {props.value}
      </span>
      );
    }
  };

  // This method is used to setting the row data in react data grid
  rowGetter = i => this.state.rows[i];

  render() {
    const { rows } = this.state;
    return (
      <div>
         <WorkBookRepetition
            backdropClassName={"no-backdrop"}
            updateState={this.updateModalState.bind(this)}
            modal={this.state.isWorkBookRepetitionModal}
            workBooksRepetition={this.state.workBooksRepetition}
          />
        <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal}  fade={false}  toggle={this.toggle} centered={true} className="custom-modal-grid">
          <ModalHeader toggle={this.toggle}>Total Tasks and Completed Percentage</ModalHeader>      
          <ModalBody>
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
                      emptyRowsView={this.state.isInitial && WorkBookProgressEmptyRowsView} 
                  />
              </div>
            </div>
          </ModalBody>
        </Modal>
      </div>
    );
  }
}

export default withCookies(WorkBookProgress);