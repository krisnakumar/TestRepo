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
            NumberOfDropdowns: 0
        };
        this.addColumns = this.addColumns.bind(this);
        this.removeColumns = this.removeColumns.bind(this);
        this.requestClose = this.requestClose.bind(this);
    }

    componentDidMount() {
        Modal.setAppElement(this.el);
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
            columnOptions: newProps.columns,
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
    * @name - addColumns
    * This method will invoked whenever the props or state
    *  is update to this component class
    * @param newProps
    * @returns none
    */
    addColumns() {
        let { columnOptions, NumberOfDropdowns } = this.state;
        NumberOfDropdowns = 0;
        columnOptions.map(function (field, index) {
            columnOptions[index].isDefault ? NumberOfDropdowns = NumberOfDropdowns + 1 : NumberOfDropdowns
        });
        columnOptions[NumberOfDropdowns].isDefault = true;
        NumberOfDropdowns = NumberOfDropdowns + 1;
        this.setState({ columnOptions, NumberOfDropdowns });
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
        let { columnOptions } = this.state;
        columnOptions[position].isDefault = false;
        let tempObj = columnOptions[position];
        columnOptions.splice(position, 1);
        columnOptions.push(tempObj)
        this.setState({ columnOptions });
        this.forceUpdate();
    }

    render() {
        let columnsOptions = this.state.columnOptions || [];
        let tempDropdownCount = 0
        columnsOptions.map(function (field, index) {
            if (field.isDefault) {
                tempDropdownCount = tempDropdownCount + 1
            }
        })
        this.state.NumberOfDropdowns = tempDropdownCount;
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
                        columnsOptions.map(function (field, index) {
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
                                            value={columnsOptions[index].value}
                                            autoFocus={false}
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
                            NumberOfDropdowns < columnsOptions.length &&
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
                        <button className="col-opt-action-btn" size="sm" title="Ok" aria-label="Ok"> Ok </button>
                        <button className="col-opt-action-btn" size="sm" title="Cancel" aria-label="Cancel" onClick={self.requestClose.bind()}> Cancel </button>
                    </div>
                </div>
            </SlidingPane>
        </div>
    };
}

export default SlidePane;
