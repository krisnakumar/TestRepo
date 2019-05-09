import React from 'react';
import ReactDOM from 'react-dom';
import OQDashboard from '../OQDashboard';

/**
 * This Class defines the jest to test
 * the OQDashboard components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<OQDashboard />, div);
  ReactDOM.unmountComponentAtNode(div);
});
