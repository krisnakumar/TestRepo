import React from 'react';
import ReactDOM from 'react-dom';
import CompletedQualification from '../CompletedQualification';

/**
 * This Class defines the jest to test
 * the CompletedQualification components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<CompletedQualification />, div);
  ReactDOM.unmountComponentAtNode(div);
});
