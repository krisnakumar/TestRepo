import React from 'react';
import ReactDOM from 'react-dom';
import CTDashboardIndex from '../index';

/**
 * This Class defines the jest to test
 * the CompanyFilterModal components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<CTDashboardIndex />, div);
  ReactDOM.unmountComponentAtNode(div);
});
