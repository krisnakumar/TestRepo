/* eslint-disable */
/*
* ResultSetGuidanceMessage.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render guidance message result set table
* Template: React.Component
* Prerequisites: React and babel
*/
import React from 'react';
import * as Constants from '../../../shared/constants';

class ResultSetGuidanceMessage extends React.Component{
  render() {
    return (<div className="no-records-found-result-set">{Constants.QUERY_BULIDER_GUIDANCE_MESSAGE}</div>)
  }
};

export default ResultSetGuidanceMessage;