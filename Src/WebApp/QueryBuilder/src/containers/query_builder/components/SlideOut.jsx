/* eslint-disable */

import React, { Component } from 'react';
import { render } from 'react-dom';
import Modal from 'react-modal';
import SlidingPane from 'react-sliding-pane';
import 'react-sliding-pane/dist/react-sliding-pane.css';
import Select from 'react-select';
import _ from 'lodash';

class SlidePane extends Component {
    constructor(props) {
        super(props);
        this.state = {
            entity: this.props.entity,
            isPaneOpen: this.props.isPaneOpen,
            isPaneOpenLeft: this.props.isPaneOpenLeft,
            columnOptions: this.props.columns,
            columnDropDowns: JSON.parse(JSON.stringify(this.buildDropDownOptions(this.props.columns))) || [],
            selectedColumnOptions: this.buildOptions(this.props.columns),
            lastSelectedColumnOptions: this.buildOptions(this.props.columns),
            NumberOfDropdowns: 0
        };
        this.columns = JSON.parse(JSON.stringify(this.buildDropDownOptions(this.props.columns))) || [];
        this.addColumns = this.addColumns.bind(this);
        this.removeColumns = this.removeColumns.bind(this);
        this.requestClose = this.requestClose.bind(this);
        this.requestOk = this.requestOk.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.optionRenderer = this.optionRenderer.bind(this);
    };

    /**
  * @method
  * @name - componentDidMount
  * This method will catch all the exceptions in this class
  * @param none
  * @returns none
  */
    componentDidMount() {
        Modal.setAppElement(this.el);
    };

    buildDropDownOptions(options) {
        let dropDownOptions = options;
        dropDownOptions.map(function (column, index) {
            column.isDisabled = column.isDefault;
            column.disabled = column.isDefault;
        });
        return dropDownOptions;
    }

    /**
     * @method
     * @name - buildOptions
     * This method will build the options dynamically from state
     * @param columns
     * @returns columns
     */
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
        if (newProps.entity == this.state.entity) {
            this.setState({
                isPaneOpen: newProps.isPaneOpen,
                NumberOfDropdowns: 0,
                columnOptions: newProps.columns
            });
        } else {
            this.setState({
                isPaneOpen: newProps.isPaneOpen,
                NumberOfDropdowns: 0,
                entity: newProps.entity,
                columnOptions: newProps.columns,
                selectedColumnOptions: this.buildOptions(newProps.columns),
                lastSelectedColumnOptions: this.buildOptions(newProps.columns),
                columnDropDowns: JSON.parse(JSON.stringify(this.buildDropDownOptions(newProps.columns))) || [],
            });
        }
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
        let selectedColumnOptions = this.state.selectedColumnOptions,
            lastSelectedColumnOptions = this.state.lastSelectedColumnOptions,
            isSame = _.isEqual(selectedColumnOptions, lastSelectedColumnOptions);// returns false if different
        let tempSelectedColumnOptions = JSON.parse(JSON.stringify(lastSelectedColumnOptions));
        if (!isSame) {
            this.setState({ isPaneOpen: false, selectedColumnOptions: tempSelectedColumnOptions });
        } else {
            this.setState({ isPaneOpen: false });
        }
        this.forceUpdate();
        this.props.columnOptionsSlideToggle();
    }

    /**
     * @method
     * @name - requestOk
     * This method will invoked whenever the props or state
     *  is update to this component class
     * @param none
     * @returns none
    */
    requestOk() {
        let selectedColumnOptions = this.state.selectedColumnOptions;
        let lastSelectedColumnOptions = JSON.parse(JSON.stringify(selectedColumnOptions));
        this.setState({ isPaneOpen: false, lastSelectedColumnOptions });
        this.forceUpdate();
        this.props.changeColumnOptions(selectedColumnOptions);
    }

    /**
    * @method
    * @name - addColumns
    * This method will invoked whenever the props or state
    *  is update to this component class
    * @param none
    * @returns none
    */
    addColumns() {
        let { selectedColumnOptions, NumberOfDropdowns, columnOptions, columnDropDowns } = this.state;
        columnDropDowns.map(function (c, i) {
            columnDropDowns[i].isDisabled = false;
            columnDropDowns[i].disabled = false;
        });
        NumberOfDropdowns = selectedColumnOptions.length;
        columnOptions[NumberOfDropdowns].isDefault = true;
        columnOptions[NumberOfDropdowns].valueSelected = "";
        selectedColumnOptions.push(columnOptions[NumberOfDropdowns]);
        selectedColumnOptions.map(function (selectedColumn, selectedColumnIndex) {
            columnDropDowns.map(function (column, columnIndex) {
                if (selectedColumn.id == column.id) {
                    columnDropDowns[columnIndex].isDisabled = true;
                    columnDropDowns[columnIndex].disabled = true;
                    return;
                }
            });
        });
        this.setState({ selectedColumnOptions, NumberOfDropdowns, columnOptions });
        this.forceUpdate();
    };

    /**
   * @method
   * @name - removeColumns
   * This method will invoked whenever the props or state
   *  is update to this component class
   * @param el
   * @returns none
   */
    removeColumns(el) {
        let position = el.currentTarget.dataset.position;
        let { selectedColumnOptions, NumberOfDropdowns, columnDropDowns } = this.state;
        columnDropDowns.map(function (c, i) {
            columnDropDowns[i].isDisabled = false;
            columnDropDowns[i].disabled = false;
        });
        selectedColumnOptions.splice(position, 1);
        NumberOfDropdowns = selectedColumnOptions.length - 1;
        selectedColumnOptions.map(function (selectedColumn, selectedColumnIndex) {
            columnDropDowns.map(function (column, columnIndex) {
                if (selectedColumn.id == column.id) {
                    columnDropDowns[columnIndex].isDisabled = true;
                    columnDropDowns[columnIndex].disabled = true;
                    return;
                }
            });
        });
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
            columnDropDowns = this.state.columnDropDowns,
            value = selectedOption.value.replace(/^\s+/g, '') || "",
            fields = selectedOption.fields.replace(/^\s+/g, '') || "",
            label = selectedOption.label.replace(/^\s+/g, '') || "",
            id = selectedOption.id.replace(/^\s+/g, '') || "";

        columnDropDowns.map(function (c, i) {
            columnDropDowns[i].isDisabled = false;
            columnDropDowns[i].disabled = false;
        });
        selectedColumnOptions[index][key] = value;
        selectedColumnOptions[index].fields = fields;
        selectedColumnOptions[index].label = label;
        selectedColumnOptions[index].value = value;
        selectedColumnOptions[index].id = id;
        selectedColumnOptions.map(function (selectedColumn, selectedColumnIndex) {
            columnDropDowns.map(function (column, columnIndex) {
                if (selectedColumn.id == column.id) {
                    columnDropDowns[columnIndex].isDisabled = true;
                    columnDropDowns[columnIndex].disabled = true;
                    return;
                }
            });
        });
        this.setState({ ...this.state, selectedColumnOptions, columnDropDowns });
        this.forceUpdate();
    };

    optionRenderer(option) {
        return option.label;
    }

    render() {
        let columnsOptions = this.state.columnOptions || [],
            selectedColumnOptions = this.state.selectedColumnOptions || [];
        let { NumberOfDropdowns } = this.state,
            self = this;
        return <div className="slide-pane" ref={ref => this.el = ref}>
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
                                            options={self.state.columnDropDowns}
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
                                            optionRenderer={self.optionRenderer}
                                        />
                                        {
                                            (selectedColumnOptions.length > 1) && <button data-position={index} onClick={self.removeColumns.bind()} className={"col-opt-close remove-" + index} size="sm" title="Remove Column" aria-label="Remove Column">
                                                <span aria-hidden className=""><i className="fa fa-times"></i></span>
                                            </button>
                                            ||
                                            <button data-position={index} className={"col-opt-info remove-" + index} size="sm" title="At least one column is required" aria-label="At least one column is required" disabled>
                                                <span aria-hidden className=""><i className="fa fa-info-circle"></i></span>
                                            </button>
                                        }

                                    </div>
                                )
                            }
                        })
                    }
                    <div>
                        {
                            (selectedColumnOptions.length) < (columnsOptions.length - 1) &&
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
