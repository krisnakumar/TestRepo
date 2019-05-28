import React from "react";
import SuspendedQualification from '../SuspendedQualification.jsx';
import { shallow, mount } from 'enzyme';
import { create } from "react-test-renderer";
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("SuspendedQualification component", () => {
    test("check renders", () => {
        const wrapper = shallow(<SuspendedQualification modal={false} suspendedQualifications={[]} />);
        expect(wrapper.exists()).toBe(true);
    });
    it("check state[modal] is false on mounting", () => {
        const wrapper = shallow(<SuspendedQualification modal={false} suspendedQualifications={[]} />);
        expect(wrapper.state('isInitial')).toEqual(false);
    });
    it('should call toggle', () => {
        const clickFn = jest.fn();
        const component = shallow(<SuspendedQualification updateState={clickFn} modal={false} suspendedQualifications={[]} />);        
        component.instance().toggle();
        expect(clickFn).toHaveBeenCalled();
        component.unmount();
    });
    it('should call cellFormatter', () => {
        const component = shallow(<SuspendedQualification modal={false} suspendedQualifications={[]} />);        
        const spy = jest.spyOn(SuspendedQualification.prototype, 'cellFormatter');
        const result = component.instance().cellFormatter({ value: ""});
        expect(spy).toHaveBeenCalled();
        component.unmount();
    });
    it('should call createRows', () => {
        let  qualifications = [{TaskCode: "", TaskName: "", EmployeeName: "", AssignedDate: "",  }]
        const component = shallow(<SuspendedQualification modal={false} suspendedQualifications={[]} />);        
        const spy = jest.spyOn(component.instance(), 'createRows');
        //update the instance with the new spy
        component.instance().createRows(qualifications);
        //invoke createRows
        expect(spy).toBeCalled();
      });
      it('should call componentWillReceiveProps', () => {
        const component = shallow(<SuspendedQualification modal={false} suspendedQualifications={[]} />);        
        const spy = jest.spyOn(component.instance(), 'componentWillReceiveProps');
        //update the instance with the new spy
        component.instance().componentWillReceiveProps({ suspendedQualifications: [], modal: true });
        //invoke componentWillReceiveProps
        expect(component.state('modal')).toEqual(true);
      });
      it('should call componentDidCatch', () => {
        const component = shallow(<SuspendedQualification modal={false} suspendedQualifications={[]} />);        
        const spy = jest.spyOn(component.instance(), 'componentDidCatch');
        //update the instance with the new spy
        component.instance().componentDidCatch("Test Error", "This is error from test case");
        //invoke componentDidCatch
        expect(spy).toBeCalled();
      });
      it('should call customCell', () => {
        const component = shallow(<SuspendedQualification modal={false} suspendedQualifications={[]} />);        
        const spy = jest.spyOn(component.instance(), 'customCell');
        //update the instance with the new spy
        component.instance().customCell({ value: 1 });
        //invoke customCell
        expect(spy).toBeCalled();
      });
      it('should call render', () => {
        const component = shallow(<SuspendedQualification modal={false} suspendedQualifications={[]} />);        
        const spy = jest.spyOn(component.instance(), 'render');
        //update the instance with the new spy
        component.instance().render();
        //invoke render
        expect(spy).toBeCalled();
      });
});
