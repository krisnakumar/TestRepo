import React from 'react';
import ReactDOM from 'react-dom';
import EmployeeExport from './EmployeeExport';

/**
 * This Class defines the jest to test
 * the EmployeeExport components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<EmployeeExport />, div);
  ReactDOM.unmountComponentAtNode(div);
});
