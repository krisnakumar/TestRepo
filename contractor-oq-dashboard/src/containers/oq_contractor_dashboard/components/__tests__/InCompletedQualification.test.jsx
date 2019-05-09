import React from 'react';
import ReactDOM from 'react-dom';
import InCompletedQualification from '../InCompletedQualification';

/**
 * This Class defines the jest to test
 * the InCompletedQualification components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<InCompletedQualification />, div);
  ReactDOM.unmountComponentAtNode(div);
});
