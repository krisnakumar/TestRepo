import React from 'react';
import ReactDOM from 'react-dom';
import LockedOutQualification from './LockedOutQualification';

/**
 * This Class defines the jest to test
 * the LockedOutQualification components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<LockedOutQualification />, div);
  ReactDOM.unmountComponentAtNode(div);
});
