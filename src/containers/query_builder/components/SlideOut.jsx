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
            columnOptions: this.props.columns
        };
        this.addColumns = this.addColumns.bind(this);
        this.removeColumns = this.removeColumns.bind(this);
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
        columnOptions: newProps.columns
      });
  };

   /**
   * @method
   * @name - addColumns
   * This method will invoked whenever the props or state
   *  is update to this component class
   * @param newProps
   * @returns none
   */
  addColumns() {
      debugger;
      let { columnOptions } = this.state,
      newColumnOptionsLength = 0
      columnOptions.map(function (field, index) {
        columnOptions[index].isDefault ? newColumnOptionsLength = newColumnOptionsLength + 1 : newColumnOptionsLength
        });
        columnOptions[newColumnOptionsLength].isDefault = true;
        this.setState({ columnOptions });
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
        columnOptions.push(columnOptions[position])
        this.setState({ columnOptions });
        this.forceUpdate();
    }


    render() {
        let columnsOptions = this.state.columnOptions || [],
        self = this;
        return <div className= "slidepane1" ref={ref => this.el = ref}>
            <SlidingPane
                className='some-custom-class'
                overlayClassName='some-custom-overlay-class'
                isOpen={ this.state.isPaneOpen }
                onRequestClose={ () => {
                    // triggered on "<" on left top click or on outside click
                    this.setState({ isPaneOpen: false });
                    this.props.columnOptionsSlideToggle();
                } }>
                <div>
                <div>
                    <h4 className= "colOptionsTitle" >Column Options</h4>
                </div>
                {                   
                    columnsOptions.map(function (field, index) {
                        if(field.isDefault) {
                            return (  
                            <div key={index} className={"row-col" + index+ " columnOptionsSelect"}>
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
                                        />
                                <button data-position={index} onClick={self.removeColumns.bind()} className={"col-opt-close remove-" + index} size="sm" title="Run Query" aria-label="Run Query">
                                    <span aria-hidden className=""><i className="fa fa-times"></i></span>
                                </button>

                            </div>
                            )
                        }
                    })
                }
                <div>
                    <button onClick={this.addColumns.bind()} className="query-section-button" size="sm" title="Run Query" aria-label="Run Query">
                        <span aria-hidden className=""><i className="fa fa-plus"></i></span>
                        <span className="fa-text-align">Add Column</span>
                    </button>
                </div>
                    
                </div>
            </SlidingPane>
        </div>;
    }
} 

export default SlidePane;