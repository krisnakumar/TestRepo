import React from 'react';
import ReactDOM from 'react-dom';
import CTDashboard from './CTDashboard';

/**
 * This Class defines the jest to test
 * the CTDashboard components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<CTDashboard />, div);
  ReactDOM.unmountComponentAtNode(div);
});
