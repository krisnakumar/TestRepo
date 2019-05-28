
import React from "react";
import TopbarNotification from '../TopbarNotification.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("TopbarNotification component", () => {
    test("renders", () => {
        const wrapper = shallow(<TopbarNotification />);
        expect(wrapper.exists()).toBe(true);
    });
    it("check state[modal] is false on mounting", () => {
        const wrapper = shallow(<TopbarNotification />);
        expect(wrapper.state('collapse')).toEqual(false);
    });
    it('should call toggle', () => {
        const wrapper = shallow(<TopbarNotification />);
        const spy = jest.spyOn(wrapper.instance(), 'toggle');
        //update the instance with the new spy
        wrapper.instance().toggle();
        //invoke toggle
        expect(spy).toBeCalled();
      });
});