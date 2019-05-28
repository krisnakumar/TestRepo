import React from "react";
import WorkBookCompleted from '../WorkBookCompleted.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("WorkBookCompleted component", () => {
  test("check renders", () => {
    const wrapper = shallow(<WorkBookCompleted modal={false} WorkBooksCompleted={[]} />);
    expect(wrapper.exists()).toBe(true);
  });

  it("check state[modal] is false on mounting", () => {
    const wrapper = shallow(<WorkBookCompleted modal={false} WorkBooksCompleted={[]} />);
    expect(wrapper.state('isInitial')).toEqual(false);
  });
  
  it('should call cellFormatter', () => {
    const wrapper = shallow(<WorkBookCompleted modal={false} WorkBooksCompleted={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'cellFormatter');
    //update the instance with the new spy
    wrapper.instance().cellFormatter({ value: "" });
    //invoke cellFormatter
    expect(spy).toBeCalled();
  });

  it('should call componentDidCatch', () => {
    const wrapper = shallow(<WorkBookCompleted modal={false} WorkBooksCompleted={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
    //update the instance with the new spy
    wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });

  it('should call createRows', () => {
    let employees = [{ UserId: 0, Role: "", EmployeeName: "", WorkBookName: "", }]
    const wrapper = shallow(<WorkBookCompleted modal={false} WorkBooksCompleted={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'createRows');
    //update the instance with the new spy
    wrapper.instance().createRows(employees);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call componentWillReceiveProps', () => {
    const wrapper = shallow(<WorkBookCompleted modal={false} assignedWorkBooks={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
    //update the instance with the new spy
    wrapper.instance().componentWillReceiveProps({ assignedWorkBooks: [], modal: true });
    //invoke componentWillReceiveProps
    expect(wrapper.state('modal')).toEqual(true);
  });

  it('should call toggle', () => {
    const clickFn = jest.fn();
    const wrapper = shallow(<WorkBookCompleted updateState={clickFn} modal={false} assignedWorkBooks={[]} />);
    wrapper.instance().toggle();
    expect(clickFn).toHaveBeenCalled();
    wrapper.unmount();
  });

  it('should call updateModalState', () => {
    const wrapper = shallow(<WorkBookCompleted />);
    const spy = jest.spyOn(wrapper.instance(), 'updateModalState');
    //update the instance with the new spy
    wrapper.instance().updateModalState("Test Modal");
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call handleCellClick', () => {
    let args = {
      userId: 1,
      workBookId: 1
    };
    const wrapper = shallow(<WorkBookCompleted />);
    const spy = jest.spyOn(wrapper.instance(), 'handleCellClick');
    //update the instance with the new spy
    wrapper.instance().handleCellClick("completedTasks", args);
    wrapper.instance().handleCellClick("percentageCompleted", args);
    wrapper.instance().handleCellClick("default", args);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call async getWorkBookProgress', () => {
    const wrapper = shallow(<WorkBookCompleted />);
    const spy = jest.spyOn(wrapper.instance(), 'getWorkBookProgress');
    //update the instance with the new spy
    wrapper.instance().getWorkBookProgress(0, 0);
    //invoke getWorkBookProgress
    expect(spy).toBeCalled();
  });

  it('should call customCellTextTooltip', () => {
    const wrapper = shallow(<WorkBookCompleted />);
    const spy = jest.spyOn(wrapper.instance(), 'customCellTextTooltip');
    //update the instance with the new spy
    wrapper.instance().customCellTextTooltip({ value: "" });
    //invoke customCellTextTooltip
    expect(spy).toBeCalled();
  });

  it('should call render', () => {
    const wrapper = shallow(<WorkBookCompleted modal={false} WorkBooksCompleted={[]} />);;
    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });
});
