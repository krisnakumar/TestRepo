import React from 'react';
import ReactDOM from 'react-dom';
import TopbarWithNavigation from './TopbarWithNavigation';

/**
 * This Class defines the jest to test
 * the TopbarWithNavigation components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<TopbarWithNavigation />, div);
  ReactDOM.unmountComponentAtNode(div);
});
