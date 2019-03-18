import React from 'react';
import ReactDOM from 'react-dom';
import Loader from './Loader';

/**
 * This Class defines the jest to test
 * the Loader components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<Loader />, div);
  ReactDOM.unmountComponentAtNode(div);
});
