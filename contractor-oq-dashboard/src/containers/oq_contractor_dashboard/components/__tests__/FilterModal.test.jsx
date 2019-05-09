import React from 'react';
import ReactDOM from 'react-dom';
import FilterModal from '../FilterModal';

/**
 * This Class defines the jest to test
 * the FilterModal components
 * extending ReactDOM module.
 */
it('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<FilterModal />, div);
  ReactDOM.unmountComponentAtNode(div);
});
