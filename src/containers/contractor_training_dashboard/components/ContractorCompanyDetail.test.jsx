import React from 'react';
import ReactDOM from 'react-dom';
import ContractorCompanyDetail from './ContractorCompanyDetail';

/**
 * This Class defines the jest to test
 * the ContractorCompanyDetail components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<ContractorCompanyDetail />, div);
  ReactDOM.unmountComponentAtNode(div);
});
