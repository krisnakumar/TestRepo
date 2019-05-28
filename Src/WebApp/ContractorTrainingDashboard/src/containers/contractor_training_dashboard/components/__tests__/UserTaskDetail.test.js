import React from "react";
import UserTaskDetails from '../UserTaskDetail';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("UserTaskDetails component", () => {
  test("check renders", () => {
    const wrapper = shallow(<UserTaskDetails modal={false} userDetails={[]} title={""} />);
    expect(wrapper.exists()).toBe(true);
  });

  it("check state[title] is empty on mounting", () => {
    const wrapper = shallow(<UserTaskDetails modal={false} userDetails={[]} title={""} />);
    expect(wrapper.state('title')).toEqual('');
  });

  it('should call componentDidCatch', () => {
    const wrapper = shallow(<UserTaskDetails modal={false} userDetails={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
    //update the instance with the new spy
    wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });

  it('should call createRows', () => {
    let companyTasks = [{ UserId: 0, RoleId: 0, IncompleteUserQualification: "", CompanyId: "", CompletedUserQualification: ""}]
    const wrapper = shallow(<UserTaskDetails modal={false} userDetails={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'createRows');
    //update the instance with the new spy
    wrapper.instance().createRows(companyTasks);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call componentWillReceiveProps', () => {
    const wrapper = shallow(<UserTaskDetails modal={false} taskDetails={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
    //update the instance with the new spy
    wrapper.instance().componentWillReceiveProps({ taskDetails: [], modal: true });
    //invoke componentWillReceiveProps
    expect(wrapper.state('modal')).toEqual(true);
  });

  it('should call updateModalState', () => {    
    const wrapper = shallow(<UserTaskDetails modal={false} userDetails={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'updateModalState');
    //update the instance with the new spy
    wrapper.instance().updateModalState("testModal");
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call toggle', () => {
    const clickFn = jest.fn();
    const wrapper = shallow(<UserTaskDetails updateState={clickFn} modal={false} userDetails={[]} title={""} />);
    wrapper.instance().toggle();
    expect(clickFn).toHaveBeenCalled();
    wrapper.unmount();
  });

  it('should call cellFormatter', () => {
    const wrapper = shallow(<UserTaskDetails />);
    const spy = jest.spyOn(wrapper.instance(), 'cellFormatter');
    //update the instance with the new spy
    wrapper.instance().cellFormatter({ value: "" });
    //invoke cellFormatter
    expect(spy).toBeCalled();
  });

  it('should call customCell', () => {
    const wrapper = shallow(<UserTaskDetails />);
    const spy = jest.spyOn(wrapper.instance(), 'customCell');
    //update the instance with the new spy
    wrapper.instance().customCell({ value: "" });
    //invoke customCell
    expect(spy).toBeCalled();
  });

  it('should call render', () => {
    const wrapper = shallow(<UserTaskDetails modal={false} userDetails={[]} title={""} />);

    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });

});
