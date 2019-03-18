import React from 'react';
import ReactDOM from 'react-dom';
import index from './index';

/**
 * This Class defines the jest to test
 * the index components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<index />, div);
  ReactDOM.unmountComponentAtNode(div);
});
