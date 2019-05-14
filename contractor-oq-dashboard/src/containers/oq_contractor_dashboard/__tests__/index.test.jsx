import React from "react";
import COQIndex from '../index.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("COQIndex component", () => {
    test("renders", () => {
        const wrapper = shallow(<COQIndex />);
        expect(wrapper.exists()).toBe(true);
    });   
});
