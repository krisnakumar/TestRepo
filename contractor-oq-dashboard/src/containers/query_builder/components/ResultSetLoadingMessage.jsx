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

class ResultSetLoadingMessage extends React.Component{
  render() {
    return (<div className="no-records-found-result-set">Loading...</div>)
  }
};

export default ResultSetLoadingMessage;