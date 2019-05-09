import React from 'react';
import ReactDOM from 'react-dom';
import Footer from '../Footer';

/**
 * This Class defines the jest to test
 * the Footer components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<Footer />, div);
  ReactDOM.unmountComponentAtNode(div);
});
