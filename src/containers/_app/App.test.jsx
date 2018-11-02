import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';

/**
 * This Class defines the jest to test
 * the App components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<App />, div);
  ReactDOM.unmountComponentAtNode(div);
});
