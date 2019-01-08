import React from 'react';
import ReactDOM from 'react-dom';
import PastDueQualification from './PastDueQualification';

/**
 * This Class defines the jest to test
 * the PastDueQualification components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<PastDueQualification />, div);
  ReactDOM.unmountComponentAtNode(div);
});
