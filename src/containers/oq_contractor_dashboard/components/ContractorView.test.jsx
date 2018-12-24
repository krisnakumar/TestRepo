import React from 'react';
import ReactDOM from 'react-dom';
import ContractorView from './ContractorView';

/**
 * This Class defines the jest to test
 * the ContractorView components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<ContractorView />, div);
  ReactDOM.unmountComponentAtNode(div);
});
