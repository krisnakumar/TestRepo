import React from "react";
import LoginIndex from '../index.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("Login Index component", () => {
    test("renders", () => {
        const wrapper = shallow(<LoginIndex />);
        expect(wrapper.exists()).toBe(true);
    });   
});
