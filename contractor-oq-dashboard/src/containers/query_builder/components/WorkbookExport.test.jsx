import React from 'react';
import ReactDOM from 'react-dom';
import WorkbookExport from './WorkbookExport';

/**
 * This Class defines the jest to test
 * the TaskExport components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<WorkbookExport />, div);
  ReactDOM.unmountComponentAtNode(div);
});
