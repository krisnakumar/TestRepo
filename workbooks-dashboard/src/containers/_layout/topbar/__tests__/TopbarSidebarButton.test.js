
import React from "react";
import TopbarMenuLink from '../TopbarSidebarButton.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("TopbarSidebarButton component", () => {   
    test("renders", () => {
        const wrapper = shallow(<TopbarMenuLink title={""} icon={""} path={""}/>);
        expect(wrapper.exists()).toBe(true);
    });   
});