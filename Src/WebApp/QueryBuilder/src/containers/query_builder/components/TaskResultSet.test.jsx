import React from 'react';
import ReactDOM from 'react-dom';
import TaskResulstSet from './EmployeeResultSet';

/**
 * This Class defines the jest to test
 * the TaskResulstSet components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<TaskResulstSet />, div);
  ReactDOM.unmountComponentAtNode(div);
});
