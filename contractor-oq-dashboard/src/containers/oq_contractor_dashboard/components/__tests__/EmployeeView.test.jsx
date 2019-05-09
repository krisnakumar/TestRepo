import React from 'react';
import ReactDOM from 'react-dom';
import EmployeeView from '../EmployeeView';

/**
 * This Class defines the jest to test
 * the EmployeeView components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<EmployeeView />, div);
  ReactDOM.unmountComponentAtNode(div);
});
