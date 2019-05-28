// import React from 'react';
// import ReactDOM from 'react-dom';
// import TopbarNavLink from '../TopbarNavLink.jsx';

/**
 * This Class defines the jest to test
 * the TopbarNavLink components
 * extending ReactDOM module.
 */
// it('renders without crashing', () => {
//   const div = document.createElement('div');
//   ReactDOM.render(<TopbarNavLink />, div);
//   ReactDOM.unmountComponentAtNode(div);
// });


import React from "react";
import TopbarNAVLink from '../TopbarNavLink.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("TopbarNAVLink component", () => {
    test("renders", () => {
        const wrapper = shallow(<TopbarNAVLink />);
        expect(wrapper.exists()).toBe(true);
    });
});