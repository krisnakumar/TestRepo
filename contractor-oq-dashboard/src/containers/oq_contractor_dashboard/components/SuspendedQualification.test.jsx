import React from 'react';
import ReactDOM from 'react-dom';
import SuspendedQualification from './SuspendedQualification';

/**
 * This Class defines the jest to test
 * the SuspendedQualification components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<SuspendedQualification />, div);
  ReactDOM.unmountComponentAtNode(div);
});
