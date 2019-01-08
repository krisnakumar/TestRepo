import React from 'react';
import ReactDOM from 'react-dom';
import WorkBookRepetition from './WorkBookRepetition';

/**
 * This Class defines the jest to test
 * the WorkBookProgress components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<WorkBookRepetition />, div);
  ReactDOM.unmountComponentAtNode(div);
});