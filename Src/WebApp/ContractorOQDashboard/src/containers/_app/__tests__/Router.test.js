import React from "react";
import Router from "../Router";
import { shallow, mount } from 'enzyme';

import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("Router component", () => {
    test("renders", () => {
        const wrapper = shallow(<Router />);
        expect(wrapper.exists()).toBe(true);
    });
});