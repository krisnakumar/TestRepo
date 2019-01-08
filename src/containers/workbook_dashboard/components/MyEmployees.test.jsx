import React from 'react';
import ReactDOM from 'react-dom';
import MyEmployees from './MyEmployees';

/**
 * This Class defines the jest to test
 * the MyEmployees components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<MyEmployees />, div);
  ReactDOM.unmountComponentAtNode(div);
});
