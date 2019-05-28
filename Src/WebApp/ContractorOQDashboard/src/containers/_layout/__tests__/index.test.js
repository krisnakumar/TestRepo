// import React from 'react';
// import ReactDOM from 'react-dom';
// import index from '../index';

// /**
//  * This Class defines the jest to test
//  * the index components
//  * extending ReactDOM module.
//  */
// it('renders without crashing', () => {
//   const div = document.createElement('div');
//   ReactDOM.render(<index />, div);
//   ReactDOM.unmountComponentAtNode(div);
// });

import React from "react";
import LayoutIndex from '../index.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("LayoutIndex component", () => {
    test("renders", () => {
        const wrapper = shallow(<LayoutIndex />);
        expect(wrapper.exists()).toBe(true);
    });   
});
