import React from 'react';
import ReactDOM from 'react-dom';
import WorkBookDashboard from './WorkBookDashboard';

/**
 * This Class defines the jest to test
 * the WorkBookDashboard components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<WorkBookDashboard />, div);
  ReactDOM.unmountComponentAtNode(div);
});
