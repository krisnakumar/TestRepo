import React from "react";
import LogInForm from "../LogInForm.jsx";
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("LogInForm component", () => {
    test("renders", () => {
        const wrapper = shallow(<LogInForm />);
        expect(wrapper.exists()).toBe(true);
    });
    
    it('should call componentDidCatch', () => {
        const wrapper = shallow(<LogInForm />);
        const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
        //update the instance with the new spy
        wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
        //invoke componentDidCatch
        expect(spy).toBeCalled();
    });

    it('should call render with isReloadWindow false', () => {
        const wrapper = shallow(<LogInForm />);
        wrapper.setState({ hasSessionCookie: false });
        wrapper.setState({ isReloadWindow: false });
        const spy = jest.spyOn(wrapper.instance(), 'render');
        //update the instance with the new spy
        wrapper.instance().render();
        //invoke render
        expect(spy).toBeCalled();
    });
    
    it('should call render with isReloadWindow true', () => {
        const wrapper = shallow(<LogInForm />);
        wrapper.setState({ hasSessionCookie: false });
        wrapper.setState({ isReloadWindow: true });
        const spy = jest.spyOn(wrapper.instance(), 'render');
        //update the instance with the new spy
        wrapper.instance().render();
        //invoke render
        expect(spy).toBeCalled();
    });
});