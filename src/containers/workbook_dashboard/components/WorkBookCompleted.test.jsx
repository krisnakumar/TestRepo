import React from 'react';
import ReactDOM from 'react-dom';
import WorkBookCompleted from './WorkBookCompleted';

/**
 * This Class defines the jest to test
 * the WorkBookComingDue components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<WorkBookCompleted />, div);
  ReactDOM.unmountComponentAtNode(div);
});
