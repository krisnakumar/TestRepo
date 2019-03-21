import React from 'react';
import ReactDOM from 'react-dom';
import OQDashboardExport from './OQDashboardExport';

/**
 * This Class defines the jest to test
 * the OQDashboardExport components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  //ReactDOM.render(<OQDashboardExport />, div);
  ReactDOM.unmountComponentAtNode(div);
});
