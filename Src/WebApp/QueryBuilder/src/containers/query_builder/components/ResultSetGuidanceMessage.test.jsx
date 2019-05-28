import React from 'react';
import ReactDOM from 'react-dom';
import ResultSetGuidanceMessage from './ResultSetGuidanceMessage';

/**
 * This Class defines the jest to test
 * the ResultSetGuidanceMessage components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<ResultSetGuidanceMessage />, div);
  ReactDOM.unmountComponentAtNode(div);
});
