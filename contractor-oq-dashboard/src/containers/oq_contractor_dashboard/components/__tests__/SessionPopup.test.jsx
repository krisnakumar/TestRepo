import React from "react";
import SessionPopup from '../SessionPopup.jsx';
import { shallow, mount } from 'enzyme';
import { create } from "react-test-renderer";
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
    it('sets loading state to true on save press', () => {
        const component = shallow(<SessionPopup
            modal={false} sessionPopupType={"API"} />);
        const spy = jest.spyOn(SessionPopup.prototype, 'reloadWindow');
        component.instance().reloadWindow();
        expect(spy).toHaveBeenCalled();
        component.unmount();
    });
    it('check state[modal] is false on after toggle()', () => {
        const component = shallow(<SessionPopup
            modal={false} sessionPopupType={"SESSION"} />);
        const result = component.instance().toggle();
        expect(component.state('modal')).toEqual(false);
        component.unmount();
    });
    it('sets loading state to true on save press', () => {
        const component = shallow(<SessionPopup
            modal={false} sessionPopupType={"API"} />);
        const spy = jest.spyOn(SessionPopup.prototype, 'componentWillReceiveProps');
        component.instance().componentWillReceiveProps({ modal: true, sessionPopupType: "API"});   
        expect(component.state('modal')).toEqual(true);
        component.unmount();
    });
});
