import React from 'react';
import ReactDOM from 'react-dom';
import WorkBookProgress from './WorkBookProgress';

/**
 * This Class defines the jest to test
 * the WorkBookProgress components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<WorkBookProgress />, div);
  ReactDOM.unmountComponentAtNode(div);
});