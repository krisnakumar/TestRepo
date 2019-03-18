import React from 'react';
import ReactDOM from 'react-dom';
import AssignedQualification from './AssignedQualification';

/**
 * This Class defines the jest to test
 * the AssignedQualification components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<AssignedQualification />, div);
  ReactDOM.unmountComponentAtNode(div);
});
