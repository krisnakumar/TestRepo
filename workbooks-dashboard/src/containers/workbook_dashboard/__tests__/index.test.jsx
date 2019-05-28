import React from "react";
import OQContractorDashboardIndex from '../index.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("OQContractorDashboardIndex component", () => {
    test("renders", () => {
        const wrapper = shallow(<OQContractorDashboardIndex />);
        expect(wrapper.exists()).toBe(true);
    });
    // it('should call changeToDark', () => {
    //     const wrapper = shallow(<OQContractorDashboardIndex />);
    //     const spy = jest.spyOn(wrapper.instance(), 'changeToDark');
    //     //update the instance with the new spy
    //     wrapper.instance().changeToDark();
    //     //invoke componentDidCatch
    //     expect(spy).toBeCalled();
    // });
    // it('should call changeToLight', () => {
    //     const dispatchFn = jest.fn();
    //     const wrapper = shallow(<OQContractorDashboardIndex dispatch= {dispatchFn()} />);
    //     const spy = jest.spyOn(wrapper.instance(), 'changeToLight');
    //     //update the instance with the new spy
    //     wrapper.instance().changeToLight();
    //     //invoke componentDidCatch
    //     expect(spy).toBeCalled();
    // });
    // it('should call componentDidCatch', () => {
    //     const wrapper = shallow(<OQContractorDashboardIndex />);
    //     const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
    //     //update the instance with the new spy
    //     wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
    //     //invoke componentDidCatch
    //     expect(spy).toBeCalled();
    // });
    // it('should call reloadWindow', () => {
    //     const wrapper = shallow(<OQContractorDashboardIndex />);
    //     const spy = jest.spyOn(wrapper.instance(), 'reloadWindow');
    //     //update the instance with the new spy
    //     wrapper.instance().reloadWindow();
    //     //invoke componentDidCatch
    //     expect(spy).toBeCalled();
    // });
    it('should call render', () => {
        const wrapper = shallow(<OQContractorDashboardIndex />);
        const spy = jest.spyOn(wrapper.instance(), 'render');
        //update the instance with the new spy
        wrapper.instance().render();
        //invoke render
        expect(spy).toBeCalled();
    });
});
