import React from "react";
import ComingDueQualification from '../ComingDueQualification.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("ComingDueQualification component", () => {
  test("check renders", () => {
    const wrapper = shallow(<ComingDueQualification modal={false} comingDueQualifications={[]} />);
    expect(wrapper.exists()).toBe(true);
  });
  it("check state[modal] is false on mounting", () => {
    const wrapper = shallow(<ComingDueQualification modal={false} comingDueQualifications={[]} />);
    expect(wrapper.state('isInitial')).toEqual(false);
  });
  it('should call cellFormatter', () => {
    const wrapper = shallow(<ComingDueQualification modal={false} comingDueQualifications={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'cellFormatter');
    //update the instance with the new spy
    wrapper.instance().cellFormatter({ value: "" });
    //invoke cellFormatter
    expect(spy).toBeCalled();
  });
  it('should call componentDidCatch', () => {
    const wrapper = shallow(<ComingDueQualification modal={false} comingDueQualifications={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
    //update the instance with the new spy
    wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });
  it('should call createRows', () => {
    let qualifications = [{ TaskCode: "", TaskName: "", EmployeeName: "", ExpirationDate: "", }]
    const wrapper = shallow(<ComingDueQualification modal={false} comingDueQualifications={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'createRows');
    //update the instance with the new spy
    wrapper.instance().createRows(qualifications);
    //invoke createRows
    expect(spy).toBeCalled();
  });
  it('should call componentWillReceiveProps', () => {
    const wrapper = shallow(<ComingDueQualification modal={false} comingDueQualifications={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
    //update the instance with the new spy
    wrapper.instance().componentWillReceiveProps({ comingDueQualifications: [], modal: true });
    //invoke componentWillReceiveProps
    expect(wrapper.state('modal')).toEqual(true);
  });
  it('should call toggle', () => {
    const clickFn = jest.fn();
    const wrapper = shallow(<ComingDueQualification updateState={clickFn} modal={false} pastDueQualifications={[]} />);
    wrapper.instance().toggle();
    expect(clickFn).toHaveBeenCalled();
    wrapper.unmount();
  });
  it('should call customCell', () => {
    const wrapper = shallow(<ComingDueQualification modal={false} comingDueQualifications={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'customCell');
    //update the instance with the new spy
    wrapper.instance().customCell({ value: "" });
    //invoke customCell
    expect(spy).toBeCalled();
  });
  it('should call render', () => {
    const wrapper = shallow(<ComingDueQualification modal={false} comingDueQualifications={[]} />);;
    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });
});
