/* eslint-disable */
/*
* ResultSetEmptyMessage.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render empty message result set table
* Template: React.Component
* Prerequisites: React and babel
*/
import React from 'react';
import * as Constants from '../../../shared/constants';

class ResultSetEmptyMessage extends React.Component{
  render() {
    return (<div className="no-records-found-result-set">{Constants.RESULT_SET_EMPTY_MESSAGE}</div>)
  }
};

export default ResultSetEmptyMessage;