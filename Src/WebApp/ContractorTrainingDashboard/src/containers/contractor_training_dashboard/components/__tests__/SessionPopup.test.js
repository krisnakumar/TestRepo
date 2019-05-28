import React from "react";
import SessionPopup from '../SessionPopup';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("SessionPopup component", () => {
    test("renders", () => {
        const wrapper = shallow(<SessionPopup modal={false} sessionPopupType={"API"} />);
        expect(wrapper.exists()).toBe(true);
    });
    it("check state[modal] is false on mounting", () => {
        const wrapper = shallow(<SessionPopup modal={false} sessionPopupType={"API"} />);  
        expect(wrapper.state('modal')).toEqual(false);
    });
    
    it('should call componentWillReceiveProps', () => {
        const component = shallow(<SessionPopup modal={false} sessionPopupType={"API"} />);
        const spy = jest.spyOn(SessionPopup.prototype, 'componentWillReceiveProps');
        component.instance().componentWillReceiveProps({ modal: true, sessionPopupType: "API"});   
        expect(component.state('modal')).toEqual(true);
        component.unmount();
    });
    
    it('check state[modal] is false on after toggle()', () => {
        const component = shallow(<SessionPopup modal={false} sessionPopupType={"SESSION"} />);
        const result = component.instance().toggle();
        expect(component.state('modal')).toEqual(false);
        component.unmount();
    });
    
    it('should call reloadWindow', () => {
        const component = shallow(<SessionPopup modal={false} sessionPopupType={"API"} />);
        const spy = jest.spyOn(SessionPopup.prototype, 'reloadWindow');
        component.instance().reloadWindow();
        expect(spy).toHaveBeenCalled();
        component.unmount();
    });

    it('should call autoLogout', () => {
        const component = shallow(<SessionPopup modal={false} sessionPopupType={"API"} />);
        const spy = jest.spyOn(SessionPopup.prototype, 'autoLogout');
        component.instance().autoLogout();
        expect(spy).toHaveBeenCalled();
        component.unmount();
    });
    
    it('should call render', () => {
        const component = shallow(<SessionPopup modal={false} sessionPopupType={"API"} />);
        const spy = jest.spyOn(component.instance(), 'render');
        //update the instance with the new spy
        component.instance().render();
        //invoke render
        expect(spy).toBeCalled();
      });
});
