import React from "react";
import MainWrapper from "../MainWrapper";
import { shallow, mount } from 'enzyme';

import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("MainWrapper component", () => {
    test("renders", () => {
        const wrapper = shallow(<MainWrapper />);
        expect(wrapper.exists()).toBe(true);
    });
});