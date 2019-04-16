/* eslint-disable */
/*
* SessionPopup.jsx
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
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';
import _ from "lodash";

class SessionPopup extends React.Component {
    static propTypes = {
        cookies: instanceOf(Cookies).isRequired
    };

    constructor(props) {
        super(props);

        this.state = {
            modal: this.props.modal
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
      modal: newProps.modal
    });
  }

    /**
   * @method
   * @name - toggle
   * This method used set state of modal to open and close
   * @param none
   * @returns none
  */
    toggle() {
        this.setState({
            modal: false
        });
    };

    autoLogout() {
        window.location = window.location.origin + "/Logout.aspx"; //Need to be window.location.origin after integrating with LMS Site
    };

    render() {
        const { rows } = this.state;
        return (
            <div>
                <Modal backdrop={"static"} isOpen={this.state.modal} toggle={this.toggle} fade={false} centered={true} className="auto-logout-modal">
                    <ModalHeader> Alert</ModalHeader>
                    <ModalBody>Your session has expired. Please login again</ModalBody>
                    <ModalFooter>
                        <button color="primary" onClick={this.autoLogout}>Go to Login</button>{' '}
                    </ModalFooter>
                </Modal>
            </div>
        );
    }
}

export default withCookies(SessionPopup);