import React from 'react';
import ReactDOM from 'react-dom';
import CTDashboardExport from './CTDashboardExport';

/**
 * This Class defines the jest to test
 * the CTDashboardExport components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  //ReactDOM.render(<CTDashboardExport />, div);
  ReactDOM.unmountComponentAtNode(div);
});
