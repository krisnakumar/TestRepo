/* eslint-disable */

import React, { Component } from 'react';
import { render } from 'react-dom';
import Modal from 'react-modal';
import SlidingPane from 'react-sliding-pane';
import 'react-sliding-pane/dist/react-sliding-pane.css';
import Select from 'react-select';

class SlidePane extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isPaneOpen: this.props.isPaneOpen,
            isPaneOpenLeft: this.props.isPaneOpenLeft,
            columnOptions: this.props.columns,
            selectedColumnOptions: this.buildOptions(this.props.columns),
            NumberOfDropdowns: 0
        };
        this.addColumns = this.addColumns.bind(this);
        this.removeColumns = this.removeColumns.bind(this);
        this.requestClose = this.requestClose.bind(this);
        this.requestOk = this.requestOk.bind(this);
        this.handleChange = this.handleChange.bind(this);
    };

    componentDidMount() {
        Modal.setAppElement(this.el);
    };

    buildOptionsTest = options =>
        options.map((option, i) => ({
            ...option,
            valueSelected: ""
        }));


    buildOptions = options => {
        let obj = [];
        options.map((option, i) => {
            if (option.isDefault) {
                option.valueSelected = "";
                obj.push(option);
            }
        });

        return obj;
    }
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
            isPaneOpen: newProps.isPaneOpen,
            NumberOfDropdowns: 0
        });
    };

    /**
   * @method
   * @name - requestClose
   * This method will invoked whenever the props or state
   *  is update to this component class
   * @param newProps
   * @returns none
   */
    requestClose() {
        this.setState({ isPaneOpen: false });
        this.props.columnOptionsSlideToggle();
    }

    /**
     * @method
     * @name - requestOk
     * This method will invoked whenever the props or state
     *  is update to this component class
     * @param newProps
     * @returns none
    */
    requestOk() {
        this.setState({ isPaneOpen: false });
        this.props.changeColumnOptions(this.state.selectedColumnOptions);
    }

    /**
    * @method
    * @name - addColumns
    * This method will invoked whenever the props or state
    *  is update to this component class
    * @param newProps
    * @returns none
    */
    addColumns() {
        let { selectedColumnOptions, NumberOfDropdowns, columnOptions } = this.state;
        NumberOfDropdowns = selectedColumnOptions.length;
        columnOptions[NumberOfDropdowns].isDefault = true;
        columnOptions[NumberOfDropdowns].valueSelected = "";
        selectedColumnOptions.push(columnOptions[NumberOfDropdowns]);
        this.setState({ selectedColumnOptions, NumberOfDropdowns, columnOptions });
        this.forceUpdate();
    };

    /**
   * @method
   * @name - addColumns
   * This method will invoked whenever the props or state
   *  is update to this component class
   * @param newProps
   * @returns none
   */
    removeColumns(el) {
        let position = el.currentTarget.dataset.position;
        let { selectedColumnOptions, NumberOfDropdowns } = this.state;
        selectedColumnOptions.splice(position, 1);
        NumberOfDropdowns = selectedColumnOptions.length - 1;
        this.setState({ selectedColumnOptions, NumberOfDropdowns });
        this.forceUpdate();
    }

    /**
     * @method
     * @name - handleChange
     * This method will triggered on select option change to update the selected value in state
     * @param index
     * @param key
     * @param selectedOption
     * @returns none
    */
    handleChange(index, key, selectedOption) {
        let selectedColumnOptions = this.state.selectedColumnOptions,
            value = selectedOption.value.replace(/^\s+/g, '') || "",
            fields = selectedOption.fields.replace(/^\s+/g, '') || "",
            label = selectedOption.label.replace(/^\s+/g, '') || "";
        selectedColumnOptions[index][key] = value;
        selectedColumnOptions[index].fields = fields;
        selectedColumnOptions[index].label = label;
        selectedColumnOptions[index].value = value;
        this.setState({ ...this.state, selectedColumnOptions });
        this.forceUpdate();
    }

    render() {
        let columnsOptions = this.state.columnOptions || [],
            selectedColumnOptions = this.state.selectedColumnOptions || [];
        let { NumberOfDropdowns } = this.state,
            self = this;
        return <div className="slidepane1" ref={ref => this.el = ref}>
            <SlidingPane
                className='some-custom-class'
                width='25%'
                overlayClassName='some-custom-overlay-class'
                isOpen={this.state.isPaneOpen}
                onRequestClose={() => {
                    // triggered on "<" on left top click or on outside click
                    this.setState({ isPaneOpen: false });
                    this.props.columnOptionsSlideToggle();
                }}>
                <div>
                    <div className="headerSection">
                        <button onClick={self.requestClose.bind()} className="" size="sm" title="Close" aria-label="">
                            <span aria-hidden className=""><i className="fa fa-times"></i></span>
                        </button>
                    </div>
                    <div>
                        <h4 className="colOptionsTitle" >Column Options</h4>
                    </div>
                    {
                        selectedColumnOptions.map(function (field, index) {
                            let valueSelected = field.valueSelected == "" || undefined ? selectedColumnOptions[index].value : field.valueSelected;
                            if (field.isDefault) {
                                return (
                                    <div key={index} className={"row-col" + index + " columnOptionsSelect"}>
                                        <Select
                                            options={columnsOptions}
                                            clearable={false}
                                            autosize={false}
                                            isRtl={true}
                                            isSearchable={false}
                                            openOnClick={true}
                                            value={valueSelected}
                                            autoFocus={false}
                                            onChange={self.handleChange.bind("", index, "valueSelected")}
                                            backspaceRemoves={false}
                                            deleteRemoves={false}
                                            placeholder={""}
                                            className="col-options-dropdown"
                                        />
                                        <button data-position={index} onClick={self.removeColumns.bind()} className={"col-opt-close remove-" + index} size="sm" title="Remove Column" aria-label="Remove Column">
                                            <span aria-hidden className=""><i className="fa fa-times"></i></span>
                                        </button>

                                    </div>
                                )
                            }
                        })
                    }
                    <div>
                        {
                            NumberOfDropdowns < (columnsOptions.length - 1) &&
                            <button onClick={this.addColumns.bind()} className="query-section-button col-opt-add-column" size="sm" title="Add Column" aria-label="Add Column">
                                <span aria-hidden className=""><i className="fa fa-plus"></i></span>
                                <span className="fa-text-align">Add Column</span>
                            </button>
                            ||
                            <button className="query-section-button col-opt-add-column" size="sm" title="Add Column" aria-label="Add Column" disabled>
                                <span aria-hidden className=""><i className="fa fa-plus"></i></span>
                                <span className="fa-text-align">Add Column</span>
                            </button>
                        }
                    </div>
                    <div className="col-opt-action-section"  >
                        <button className="col-opt-action-btn" size="sm" title="Ok" aria-label="Ok" onClick={self.requestOk.bind()}> Ok </button>
                        <button className="col-opt-action-btn" size="sm" title="Cancel" aria-label="Cancel" onClick={self.requestClose.bind()}> Cancel </button>
                    </div>
                </div>
            </SlidingPane>
        </div>
    };
}

export default SlidePane;
