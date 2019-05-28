import React from 'react';
import ReactDOM from 'react-dom';
import ResultSetEmptyMessage from './ResultSetEmptyMessage';

/**
 * This Class defines the jest to test
 * the ResultSetGuidanceMessage components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<ResultSetEmptyMessage />, div);
  ReactDOM.unmountComponentAtNode(div);
});
