
import React from 'react';
import ReactDOM from 'react-dom';
import AssignedWorkBook from './AssignedWorkBook';

/**
 * This Class defines the jest to test
 * the AssignedWorkBook components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<AssignedWorkBook />, div);
  ReactDOM.unmountComponentAtNode(div);
});
