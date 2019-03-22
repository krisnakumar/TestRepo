import React from 'react';
import ReactDOM from 'react-dom';
import CompanyFilterModal from './CompanyFilterModal';

/**
 * This Class defines the jest to test
 * the CompanyFilterModal components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<CompanyFilterModal />, div);
  ReactDOM.unmountComponentAtNode(div);
});
