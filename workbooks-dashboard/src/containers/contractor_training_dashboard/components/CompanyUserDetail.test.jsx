import React from 'react';
import ReactDOM from 'react-dom';
import CompanyUserDetail from './CompanyUserDetail';

/**
 * This Class defines the jest to test
 * the CompanyUserDetail components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<CompanyUserDetail />, div);
  ReactDOM.unmountComponentAtNode(div);
});
