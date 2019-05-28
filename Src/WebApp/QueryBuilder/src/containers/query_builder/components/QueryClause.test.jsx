import React from 'react';
import ReactDOM from 'react-dom';
import QueryClause from './QueryClause';

/**
 * This Class defines the jest to test
 * the QueryClause components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<QueryClause />, div);
  ReactDOM.unmountComponentAtNode(div);
});
