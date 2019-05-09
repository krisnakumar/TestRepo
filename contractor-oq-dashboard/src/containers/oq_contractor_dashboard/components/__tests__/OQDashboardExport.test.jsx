// import React from 'react';
// import ReactDOM from 'react-dom';
// import OQDashboardExport from '../OQDashboardExport';

// jest.disableAutomock();

// /**
//  * This Class defines the jest to test
//  * the OQDashboardExport components
//  * extending ReactDOM module.
//  */
// it('renders without crashing', () => {
//   const div = document.createElement('div');
//   ReactDOM.render(<OQDashboardExport />, div);
//   // ReactDOM.render(<div />, div);
//   ReactDOM.unmountComponentAtNode(div);
// });


import React from "react";
import OQDashboardExport from '../OQDashboardExport';
import { shallow, mount } from 'enzyme';
import { create } from "react-test-renderer";
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("OQDashboardExport component", () => {
    test("renders", () => {
        const wrapper = shallow(<OQDashboardExport />);
        expect(wrapper.exists()).toBe(true);
    });
});