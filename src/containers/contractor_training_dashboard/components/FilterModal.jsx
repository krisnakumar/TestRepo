/* eslint-disable */
/*
* FilterModal.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used show the filter section
* Template: React.Component
* Prerequisites: React and babel

METHODS
--------
*/
import React from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter, Row, Col, Input } from 'reactstrap';
import { instanceOf, PropTypes } from 'prop-types';
import { withCookies, Cookies } from 'react-cookie';

class FilterModal extends React.Component {

    static propTypes = {
        cookies: instanceOf(Cookies).isRequired
    };

    constructor(props) {
        super(props);

        this.state = {
            modal: this.props.modal,
            title: this.props.title || ""
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
     * @name - componentWillReceiveProps
     * This method will invoked whenever the props or state
     * is update to this component class
     * @param newProps
     * @returns none
     */
    componentWillReceiveProps(newProps) {
        this.setState({
            modal: newProps.modal,
            title: newProps.title || ""
        });
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
        this.props.updateState("isFilterModal");
    };

    render() {
        const { title } = this.state;
        let titleText = title || "";
        return (
            <div>
                <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-filter">
                    <ModalHeader className="center" toggle={this.toggle}>
                        {titleText}
                    </ModalHeader>
                    <ModalBody className="custom-modal-filter-body">
                        <Row className="custom-modal-filter-search">
                            <Col sm={{ size: 1, offset: 3 }}><label>Search</label></Col>
                            <Col sm={{ size: 4 }}><Input value="" className="" /></Col>
                            <Col sm={{ size: 2 }}><button className="btn-as-text" onClick={console.log(this)} >Refresh List</button></Col>
                        </Row>
                        <div className="grid-container">

                        </div>
                    </ModalBody>
                    <ModalFooter>
                        <button color="primary" className="custom-modal-filter-btn" onClick={this.toggle}>OK</button>{' '}
                        <button color="secondary" className="custom-modal-filter-btn" onClick={this.toggle}>Cancel</button>
                    </ModalFooter>
                </Modal>
            </div>
        );
    }
}

export default withCookies(FilterModal);