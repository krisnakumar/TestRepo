import React from 'react';
import ReactDOM from 'react-dom';
import QueryPane from './QueryPane';

/**
 * This Class defines the jest to test
 * the QueryPane components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<QueryPane />, div);
  ReactDOM.unmountComponentAtNode(div);
});
