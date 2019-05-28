
import React from "react";
import AssignedWorkBook from '../AssignedWorkBook.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

/**
 * This Class defines the jest to test
 * the AssignedWorkBook components
 * extending ReactDOM module.
 */

Enzyme.configure({ adapter: new Adapter() });

describe("AssignedWorkBook component", () => {
  test("check renders", () => {
    const wrapper = shallow(<AssignedWorkBook modal={false} assignedWorkBooks={[]} />);
    expect(wrapper.exists()).toBe(true);
  });
  it("check state[modal] is false on mounting", () => {
    const wrapper = shallow(<AssignedWorkBook modal={false} assignedWorkBooks={[]} />);
    expect(wrapper.state('isInitial')).toEqual(false);
  });
  it('should call componentDidCatch', () => {
    const wrapper = shallow(<AssignedWorkBook modal={false} assignedWorkBooks={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
    //update the instance with the new spy
    wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });
  it('should call getWorkBookProgress', () => {
    const wrapper = shallow(<AssignedWorkBook modal={true} assignedWorkBooks={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'getWorkBookProgress');
    //update the instance with the new spy
    wrapper.instance().getWorkBookProgress(0, 0);
    //invoke componentWillReceiveProps
    expect(wrapper.state('modal')).toEqual(true);
  });
  it('should call createRows', () => {
    let qualifications = [{ UserId: 0, WorkBookId: 0, WorkBookName: "", Role: "", EmployeeName: "", UserName: "", RepsCompleted: "", RepsRequired: "", dueDate: ""}]
    const wrapper = shallow(<AssignedWorkBook modal={false} assignedWorkBooks={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'createRows');
    //update the instance with the new spy
    wrapper.instance().createRows(qualifications);
    //invoke createRows
    expect(spy).toBeCalled();
  });
  it('should call componentWillReceiveProps', () => {
    const wrapper = shallow(<AssignedWorkBook modal={false} assignedWorkBooks={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
    //update the instance with the new spy
    wrapper.instance().componentWillReceiveProps({ assignedWorkBooks: [], modal: true });
    //invoke componentWillReceiveProps
    expect(wrapper.state('modal')).toEqual(true);
  });
  it('should call updateModalState', () => {
    const wrapper = shallow(<AssignedWorkBook modal={true} assignedWorkBooks={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'updateModalState');
    //update the instance with the new spy
    wrapper.instance().updateModalState("");
    //invoke updateModalState
    expect(wrapper.state('modal')).toEqual(true);
  });
  it('should call toggle', () => {
    const clickFn = jest.fn();
    const wrapper = shallow(<AssignedWorkBook updateState={clickFn} modal={false} pastDueWorkbooks={[]} />);
    wrapper.instance().toggle();
    expect(clickFn).toHaveBeenCalled();
    wrapper.unmount();
  });
  it('should call handleCellClick', () => {
    const wrapper = shallow(<AssignedWorkBook modal={false} assignedWorkBooks={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'handleCellClick');
    //update the instance with the new spy
    wrapper.instance().handleCellClick("", {userId:0, workBookId:0});
    //invoke handleCellClick
    expect(spy).toBeCalled();
  }); 
  it('should call cellFormatter', () => {
    const wrapper = shallow(<AssignedWorkBook modal={false} assignedWorkBooks={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'cellFormatter');
    //update the instance with the new spy
    wrapper.instance().cellFormatter({ value: "" });
    //invoke cellFormatter
    expect(spy).toBeCalled();
  });
  it('should call workbookFormatter', () => {
    let props = {
      dependentValues:{
        employee: ""
      },
      value: ""
    };
    const wrapper = shallow(<AssignedWorkBook modal={false} assignedWorkBooks={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'workbookFormatter');
    //update the instance with the new spy
    wrapper.instance().workbookFormatter("", props);
    //invoke workbookFormatter
    expect(spy).toBeCalled();
  });
  it('should call customCell', () => {
    const wrapper = shallow(<AssignedWorkBook modal={false} assignedWorkBooks={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'customCell');
    //update the instance with the new spy
    wrapper.instance().customCell({ value: "" });
    //invoke customCell
    expect(spy).toBeCalled();
  });
  it('should call render', () => {
    const wrapper = shallow(<AssignedWorkBook modal={false} assignedWorkBooks={[]} />);;
    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });
});

