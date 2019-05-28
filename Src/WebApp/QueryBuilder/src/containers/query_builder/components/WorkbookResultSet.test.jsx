import React from 'react';
import ReactDOM from 'react-dom';
import WorkbookResultSet from './WorkbookResultSet';

/**
 * This Class defines the jest to test
 * the WorkbookResultSet components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<WorkbookResultSet />, div);
  ReactDOM.unmountComponentAtNode(div);
});
