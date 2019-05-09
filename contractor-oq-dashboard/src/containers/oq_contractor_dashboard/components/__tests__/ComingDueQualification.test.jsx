import React from 'react';
import ReactDOM from 'react-dom';
import ComingDueQualification from '../ComingDueQualification';

/**
 * This Class defines the jest to test
 * the ComingDueQualification components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<ComingDueQualification />, div);
  ReactDOM.unmountComponentAtNode(div);
});
