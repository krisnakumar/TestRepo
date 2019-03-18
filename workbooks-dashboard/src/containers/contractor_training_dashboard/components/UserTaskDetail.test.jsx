import React from 'react';
import ReactDOM from 'react-dom';
import UserTaskDetail from './UserTaskDetail';

/**
 * This Class defines the jest to test
 * the UserTaskDetail components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<UserTaskDetail />, div);
  ReactDOM.unmountComponentAtNode(div);
});
