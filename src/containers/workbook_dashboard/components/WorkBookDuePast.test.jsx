import React from 'react';
import ReactDOM from 'react-dom';
import WorkBookDuePast from './WorkBookDuePast';

/**
 * This Class defines the jest to test
 * the WorkBookDuePast components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<WorkBookDuePast />, div);
  ReactDOM.unmountComponentAtNode(div);
});
