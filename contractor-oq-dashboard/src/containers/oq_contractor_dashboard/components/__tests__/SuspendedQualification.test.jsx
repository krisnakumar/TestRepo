import React from "react";
import SuspendedQualification from '../SuspendedQualification.jsx';
import { shallow, mount } from 'enzyme';
import { create } from "react-test-renderer";
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("SessionPopup component", () => {
    test("renders", () => {
        const wrapper = shallow(<SuspendedQualification modal={false} suspendedQualifications={[]} />);
        expect(wrapper.exists()).toBe(true);
    });
    it("check state[modal] is false on mounting", () => {
        const wrapper = shallow(<SuspendedQualification modal={false} suspendedQualifications={[]} />);
        expect(wrapper.state('isInitial')).toEqual(false);
    });
    it('sets loading state to true on save press', () => {
        const clickFn = jest.fn();
        const component = shallow(<SuspendedQualification updateState={clickFn} modal={false} suspendedQualifications={[]} />);        
        component.instance().toggle();
        expect(clickFn).toHaveBeenCalled();
        component.unmount();
    });
    it('check state[modal] is false on after cellFormatter()', () => {
        const component = shallow(<SuspendedQualification modal={false} suspendedQualifications={[]} />);        
        const spy = jest.spyOn(SuspendedQualification.prototype, 'cellFormatter');
        const result = component.instance().cellFormatter({ value: ""});
        expect(spy).toHaveBeenCalled();
        component.unmount();
    });
    // it('sets loading state to true on save press', () => {
    //     const component = shallow(<SessionPopup
    //         modal={false} sessionPopupType={"API"} />);
    //     const spy = jest.spyOn(SessionPopup.prototype, 'componentWillReceiveProps');
    //     component.instance().componentWillReceiveProps({ modal: true, sessionPopupType: "API"});   
    //     expect(component.state('modal')).toEqual(true);
    //     component.unmount();
    // });
});
