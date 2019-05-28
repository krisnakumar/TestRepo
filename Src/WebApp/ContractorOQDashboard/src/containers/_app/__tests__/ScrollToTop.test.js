import React from "react";
import ScrollToTop from "../ScrollToTop";
import { shallow, mount } from 'enzyme';

import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("ScrollToTop component", () => {
    test("renders", () => {
        const wrapper = shallow(<ScrollToTop />);
        expect(wrapper.exists()).toBe(true);
    });
});