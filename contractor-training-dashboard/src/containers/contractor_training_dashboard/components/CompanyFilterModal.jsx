/* eslint-disable */
/*
* CompanyFilterModal.jsx
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
import { WithContext as ReactTags } from 'react-tag-input';
import Picky from "react-picky";
import "react-picky/dist/picky.css";

class CompanyFilterModal extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            modal: this.props.modal,
            title: this.props.title || "",
            options: this.props.filterOptionsCompanies || [],
            arrayValue: [],
            companyFilterSearchValue: "",
            tags: [],
            lastSelectedValue: []
        };

        this.toggle = this.toggle.bind(this);
        this.selectMultipleOption = this.selectMultipleOption.bind(this);
        this.refreshList = this.refreshList.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleDelete = this.handleDelete.bind(this);
        this.passSelectedData = this.passSelectedData.bind(this);
        this.resetSelectedData = this.resetSelectedData.bind(this);
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
            options: newProps.filterOptionsCompanies,
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
     * This method will update the current parent of modal window
     * @param workbooks
     * @returns none
     */
    toggle() {
        this.setState({
            modal: !this.state.modal
        });
        this.props.updateState("isCompanyFilterModal");
    };

    /**
    * @method
    * @name - selectMultipleOption
    * This method will update selected multiple role state
    * @param isParentUpdate
    * @param value
    * @returns none
    */
    selectMultipleOption(isParentUpdate, component, value) {
        // patch to set the only one value for the filter
        // this will be removed 
        if (value.length > 1)
            value = value.splice(1, 1); // Patch for filter, it will retain the last selected

        if (isParentUpdate) {
            this.setState({ arrayValue: value, tags: value, lastSelectedValue: value });
        } else {
            this.setState({ arrayValue: value, tags: value });
        }
    }

    /**
     * @method
     * @name - refreshList
     * This method will update search filter based on search query
     * @param none
     * @returns none
    */
    refreshList() {
        let element = document.getElementById('companyFilterSearchInput'),
            value = "";
            if(element && element.value){
                value = element.value;
            }
        this.setState({
            companyFilterSearchValue: value.trim()
        });
    };

    /**
     * @method
     * @name - handleInputChange
     * This method will triggered on input change to update the value in state
     * @param ele
     * @returns none
    */
    handleInputChange(event) {
        event.preventDefault();
        let name = event.target.name;
        let value = event.target.value;
        this.setState({ [name]: value });
    }

    /**
    * @method
    * @name - handleDelete
    * This method will delete the selected roles and update it on state
    * @param i
    * @returns none
   */
    handleDelete(i) {
        const { tags, arrayValue } = this.state;
        this.setState({
            tags: tags.filter((tag, index) => index !== i),
            arrayValue: arrayValue.filter((tag, index) => index !== i),
        });
    }

    /**
     * @method
     * @name - passSelectedData
     * This method will update the current of selected roles and pass it to parent component
     * @param none
     * @returns none
    */
    passSelectedData() {
        let selectedData = this.state.tags;
        this.setState({ lastSelectedValue: selectedData });
        this.props.updateCompanySelectedData(selectedData);
        this.toggle();
    };

    /**
     * @method
     * @name - resetSelectedData
     * This method will reset the current of selected roles and pass it to parent component
     * @param none
     * @returns none
    */
    resetSelectedData() {
        let lastSelectedValue = this.state.lastSelectedValue;
        this.setState({ arrayValue: lastSelectedValue, tags: lastSelectedValue });
        this.props.updateCompanySelectedData(lastSelectedValue);
        this.toggle();
    };

    /**
     * @method
     * @name - filterOptions
     * This method will generate the possible suggestions options based on filter text value
     * @param textInputValue
     * @param possibleSuggestionsArray
     * @returns none
    */
    filterOptions(textInputValue, possibleSuggestionsArray) {
        let lowerCaseQuery = textInputValue.toLowerCase().trim();

        return possibleSuggestionsArray.filter(function (suggestion) {
            return suggestion.text.toLowerCase().includes(lowerCaseQuery)
        })
    };

    render() {
        const { title } = this.state;
        let titleText = title || "";

        let { options, companyFilterSearchValue, tags, arrayValue } = this.state;

        let possibleOptionsArray = this.filterOptions(companyFilterSearchValue, options);

        return (
            <div>
                <Modal backdropClassName={this.props.backdropClassName} backdrop={"static"} isOpen={this.state.modal} fade={false} toggle={this.toggle} centered={true} className="custom-modal-filter">
                    <ModalHeader className="center" toggle={this.resetSelectedData}>
                        {titleText}
                    </ModalHeader>
                    <ModalBody className="custom-modal-filter-body">
                        <Row className="custom-modal-filter-search">
                            <Col sm={{ size: 1, offset: 3 }}><label>Search</label></Col>
                            <Col sm={{ size: 4 }}><Input value={companyFilterSearchValue} id="companyFilterSearchInput" name="companyFilterSearchValue" onChange={event => this.handleInputChange(event)} /></Col>
                            <Col sm={{ size: 2 }}><button className="btn-as-text" onClick={this.refreshList} >Refresh List</button></Col>
                        </Row>
                        <div className="row">
                            <div className="col">
                                <ReactTags
                                    tags={tags}
                                    handleDelete={this.handleDelete}
                                    handleDrag={console.log()}
                                />
                            </div>
                        </div>
                        <div className="grid-container border-none">
                            <div className="row">
                                <div className="col">
                                    <Picky
                                        className="custom-picky"
                                        value={arrayValue}
                                        options={possibleOptionsArray}
                                        onChange={this.selectMultipleOption.bind("", false, this)}
                                        open={true}
                                        keepOpen={true}
                                        valueKey="id"
                                        labelKey="text"
                                        multiple={true}
                                        includeSelectAll={false}
                                        includeFilter={false}
                                        dropdownHeight={250}
                                    />
                                </div>
                            </div>
                        </div>
                    </ModalBody>
                    <ModalFooter>
                        <button color="primary" className="custom-modal-filter-btn" onClick={this.passSelectedData}>OK</button>{' '}
                        <button color="secondary" className="custom-modal-filter-btn" onClick={this.resetSelectedData}>Cancel</button>
                    </ModalFooter>
                </Modal>
            </div>
        );
    }
}

export default CompanyFilterModal;
