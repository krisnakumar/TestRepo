/* eslint-disable */
/*
* UserTaskDetail.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render User task details 
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
import * as moment from 'moment';
import Export from './CTDashboardExport';

// Import React Table
import ReactTable from "react-table";
import "react-table/react-table.css";

/**
 * UserTaskDetailEmptyRowsView Class defines the React component to render
 * the table components empty rows message if data is empty from API request
 * extending the react data grid module.
 */

class UserTaskDetail extends React.Component {

  constructor(props) {
    super(props);

    this.heads = [
      {
        key: 'taskCode',
        name: 'Task Code',
        sortable: true,
        editable: false,
        width: 150,
        cellClass: "text-left"
      },
      {
        key: 'taskName',
        name: 'Task Name',
        sortable: true,
        editable: false,
        width: 600,
        cellClass: "text-left"
      },
      {
        key: 'expirationDate',
        name: 'Expires',
        sortable: true,
        width: 200,
        editable: false,
        cellClass: "text-center"
      },
      {
        key: 'status',
        name: 'Status',
        sortable: true,
        editable: false,
        width: 150,
        cellClass: "text-center last-column"
      }
    ];

    this.state = {
      modal: this.props.modal,
      rows: this.createRows(this.props.taskDetails || {}),
      isInitial: false,
      title: this.props.title || ""
    };
    this.toggle = this.toggle.bind(this);
    this.customCell = this.customCell.bind(this);
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
  * @param taskDetails
  * @returns rows
  */
  createRows = (taskDetails) => {
    const rows = [],
      length = taskDetails ? taskDetails.length : 0;
    for (let i = 0; i < length; i++) {
      rows.push({
        taskCode: taskDetails[i].TaskCode || "",
        taskName: taskDetails[i].TaskName || "",
        status: taskDetails[i].Status || "",
        expirationDate: taskDetails[i].ExpirationDate || "",

      });
    }

    return rows;
  };

  /**
   * @method
   * @name - componentWillReceiveProps
   * This method will invoked whenever the props or state
   * is update to this component class
   * @param newProps
   * @returns none
   */
  componentWillReceiveProps(newProps) {
    let rows = this.createRows(newProps.taskDetails),
      isArray = Array.isArray(newProps.taskDetails),
      isInitial = isArray;

    if (rows) {
      this.state.modal = newProps.modal;
      this.state.rows = rows;
      this.state.isInitial = isInitial;
      this.state.title = newProps.title || "";
    } else {
      this.setState({
        modal: newProps.modal,
        rows: rows,
        isInitial: isInitial,
        title: newProps.title || ""
      });
    }
  }

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
   * @name - toggle
   * This method will update the current of modal window
   * @param workbooks
   * @returns none
   */
  toggle() {
    this.setState({
      modal: !this.state.modal
    });
    this.props.updateState("isTaskDetailsModal");
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

  customCell(props) {
    let self = this;
    return (
      props.value && <span onClick={e => { e.preventDefault(); self.handleCellClick(props.column.id, props.original); }} className={"text-clickable"}>
        {props.value}
      </span> || <span>{props.value}</span>
    );
  }

  render() {
    const { rows, title } = this.state;
    let titleText = title || "";
    let pgSize = (rows.length > 10) ? rows.length : 10;
    // pgSize = (pgSize > 20) ? 20 : pgSize;
    return (
      <div>
        <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-grid grid-modal-popup">
          <ModalHeader className="text-left" toggle={this.toggle}>
            {titleText}
            <p className="section-info-description">This level will display the contractors training tasks and their respective status</p>
            <p className="section-info-description"> </p>
          </ModalHeader>
          <div>
            <div className="export-menu-one">

            </div>
            <div className="export-menu-two">
              <Export
                data={this.state.rows}
                heads={this.heads}
                sheetName={titleText}
              />
            </div>
          </div>
          <ModalBody>
            <div className="grid-container">
              <div className="table">                
                <ReactTable
                  minRows={1}
                  data={rows}
                  columns={[
                    {
                      Header: "Task Code",
                      id: "taskCode",
                      accessor: "taskCode",
                      headerClassName: 'header-wordwrap',
                      minWidth: 150,
                      maxWidth: 200,
                      className: 'text-left'
                    },
                    {
                      Header: "Task Name",
                      id: "taskName",
                      accessor: "taskName",
                      headerClassName: 'header-wordwrap',
                      minWidth: 300,
                      className: 'text-left'
                    },
                    {
                      Header: "Expires",
                      id: "expirationDate",
                      accessor: "expirationDate",
                      headerClassName: 'header-wordwrap',
                      minWidth: 100,
                      maxWidth: 200,
                      className: 'text-center',
                      render: props => <span>{moment(props.value).format('MM/DD/YYYY')}</span>,
                      sortMethod: (a, b) => {
                        var a1 = new Date(a).getTime();
                        var b1 = new Date(b).getTime();
                        if (a1 <= b1)
                          return 1;
                        else if (a1 >= b1)
                          return -1;
                        else
                          return 0;
                      }
                    },
                    {
                      Header: "Status",
                      id: "status",
                      accessor: "status",
                      headerClassName: 'header-wordwrap',
                      minWidth: 150,
                      maxWidth: 200,
                      className: 'text-center'
                    }
                  ]
                  }
                  resizable={false}
                  className="-striped -highlight"
                  showPagination={false}
                  showPaginationTop={false}
                  showPaginationBottom={false}
                  showPageSizeOptions={false}
                  pageSizeOptions={[5, 10, 20, 25, 50, 100]}
                  pageSize={!this.state.isInitial ? 10 : pgSize}
                  loading={!this.state.isInitial}
                  loadingText={''}
                  noDataText={!this.state.isInitial ? '' : 'Sorry, no records'}
                  defaultSorted={[
                    {
                      id: "taskName",
                      desc: false
                    }
                  ]}
                  style={{
                    maxHeight: "550px"
                  }}
                />
              </div>
            </div>
          </ModalBody>
        </Modal>
      </div>
    );
  }
}

export default UserTaskDetail;