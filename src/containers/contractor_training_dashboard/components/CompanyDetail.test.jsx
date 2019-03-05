import React from 'react';
import ReactDOM from 'react-dom';
import CompanyDetail from './CompanyDetail';

/**
 * This Class defines the jest to test
 * the CompanyDetail components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<CompanyDetail />, div);
  ReactDOM.unmountComponentAtNode(div);
});
