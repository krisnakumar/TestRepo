import React from "react";
import WorkBookRepetition from '../WorkBookRepetition';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("WorkBookRepetition component", () => {
  test("check renders", () => {
    const wrapper = shallow(<WorkBookRepetition modal={false} workBooksProgress={[]} selectedWorkbook={[]} />);
    expect(wrapper.exists()).toBe(true);
  });

  it("check state[modal] is false on mounting", () => {
    const wrapper = shallow(<WorkBookRepetition modal={false} workBooksProgress={[]} selectedWorkbook={[]} />);
    expect(wrapper.state('isInitial')).toEqual(false);
  });

  it('should call cellFormatter', () => {
    const wrapper = shallow(<WorkBookRepetition modal={false} workBooksProgress={[]} selectedWorkbook={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'cellFormatter');
    //update the instance with the new spy
    wrapper.instance().cellFormatter({ value: "" });
    //invoke cellFormatter
    expect(spy).toBeCalled();
  });

  it('should call componentDidCatch', () => {
    const wrapper = shallow(<WorkBookRepetition modal={false} workBooksProgress={[]} selectedWorkbook={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
    //update the instance with the new spy
    wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });
  
  it('should call createRows', () => {
    let workbooks = [{ TaskId: 0, TaskCode: "", TaskName: "", RepsRequired: 0, }]
    const wrapper = shallow(<WorkBookRepetition modal={false} workBooksProgress={[]} selectedWorkbook={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'createRows');
    //update the instance with the new spy
    wrapper.instance().createRows(workbooks, 1, 1);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call componentWillReceiveProps', () => {
    const wrapper = shallow(<WorkBookRepetition modal={false} Qualifications={[]} selectedWorkbook={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
    //update the instance with the new spy
    wrapper.instance().componentWillReceiveProps({ workBooksProgress: [], modal: true, selectedWorkbook:{} });
    //invoke componentWillReceiveProps
    expect(wrapper.state('modal')).toEqual(true);
  });
  
  it('should call toggle', () => {
    const clickFn = jest.fn();
    const wrapper = shallow(<WorkBookRepetition updateState={clickFn} modal={false} assignedWorkBooks={[]} selectedWorkbook={[]} />);
    wrapper.instance().toggle();
    expect(clickFn).toHaveBeenCalled();
    wrapper.unmount();
  });

  it('should call customCell', () => {
    const wrapper = shallow(<WorkBookRepetition modal={false} employee={[]} selectedWorkbook={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'customCell');
    //update the instance with the new spy
    wrapper.instance().customCell({ value: "" });
    //invoke customCell
    expect(spy).toBeCalled();
  });

  it('should call render', () => {
    const wrapper = shallow(<WorkBookRepetition modal={false} workBooksProgress={[]} selectedWorkbook={[]} />);;
    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });
});
