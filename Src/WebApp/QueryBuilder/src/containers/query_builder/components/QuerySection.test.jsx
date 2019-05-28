import React from 'react';
import ReactDOM from 'react-dom';
import QuerySection from './QuerySection';

/**
 * This Class defines the jest to test
 * the QuerySection components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<QuerySection />, div);
  ReactDOM.unmountComponentAtNode(div);
});
