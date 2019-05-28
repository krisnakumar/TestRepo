import React from "react";
import LockedOutQualification from '../LockedOutQualification.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("LockedOutQualification component", () => {
  test("check renders", () => {
    const wrapper = shallow(<LockedOutQualification modal={false} lockoutQualifications={[]} />);
    expect(wrapper.exists()).toBe(true);
  });
  it("check state[modal] is false on mounting", () => {
    const wrapper = shallow(<LockedOutQualification modal={false} lockoutQualifications={[]} />);
    expect(wrapper.state('isInitial')).toEqual(false);
  });
  it('should call cellFormatter', () => {
    const wrapper = shallow(<LockedOutQualification modal={false} lockoutQualifications={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'cellFormatter');
    //update the instance with the new spy
    wrapper.instance().cellFormatter({ value: "" });
    //invoke cellFormatter
    expect(spy).toBeCalled();
  });
  it('should call componentDidCatch', () => {
    const wrapper = shallow(<LockedOutQualification modal={false} lockoutQualifications={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
    //update the instance with the new spy
    wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });
  it('should call createRows', () => {
    let  qualifications = [{TaskCode: "", TaskName: "", EmployeeName: "", UnlockDate: "",  }]
     const wrapper = shallow(<LockedOutQualification modal={false} lockoutQualifications={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'createRows');
    //update the instance with the new spy
    wrapper.instance().createRows(qualifications);
    //invoke createRows
    expect(spy).toBeCalled();
  });
  it('should call componentWillReceiveProps', () => {
     const wrapper = shallow(<LockedOutQualification modal={false} lockoutQualifications={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
    //update the instance with the new spy
    wrapper.instance().componentWillReceiveProps({ lockoutQualifications: [], modal: true });
    //invoke componentWillReceiveProps
    expect(wrapper.state('modal')).toEqual(true);
  });
  it('should call toggle', () => {
    const clickFn = jest.fn();
    const wrapper = shallow(<LockedOutQualification updateState={clickFn} modal={false} pastDueQualifications={[]} />);
    wrapper.instance().toggle();
    expect(clickFn).toHaveBeenCalled();
    wrapper.unmount();
  });
  it('should call render', () => {
     const wrapper = shallow(<LockedOutQualification modal={false} lockoutQualifications={[]} />);;        
    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });
});
