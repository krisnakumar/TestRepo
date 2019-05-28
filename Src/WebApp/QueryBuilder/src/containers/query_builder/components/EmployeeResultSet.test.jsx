import React from 'react';
import ReactDOM from 'react-dom';
import EmployeeResultSet from './EmployeeResultSet';

/**
 * This Class defines the jest to test
 * the EmployeeResultSet components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<EmployeeResultSet />, div);
  ReactDOM.unmountComponentAtNode(div);
});
