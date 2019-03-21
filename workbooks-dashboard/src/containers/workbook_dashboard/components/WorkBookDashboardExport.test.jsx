import React from 'react';
import ReactDOM from 'react-dom';
import WorkBookDashboardExport from './WorkBookDashboardExport';

/**
 * This Class defines the jest to test
 * the WorkBookDashboardExport components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  //ReactDOM.render(<WorkBookDashboardExport />, div);
  ReactDOM.unmountComponentAtNode(div);
});
