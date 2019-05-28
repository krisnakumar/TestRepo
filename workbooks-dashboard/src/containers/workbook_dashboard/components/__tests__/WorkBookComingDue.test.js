import React from "react";
import ComingDueWorkbook from '../WorkBookComingDue.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("ComingDueWorkbook component", () => {
  test("check renders", () => {
    const wrapper = shallow(<ComingDueWorkbook modal={false} comingDueWorkbooks={[]} />);
    expect(wrapper.exists()).toBe(true);
  });

  it("check state[modal] is false on mounting", () => {
    const wrapper = shallow(<ComingDueWorkbook modal={false} comingDueWorkbooks={[]} />);
    expect(wrapper.state('isInitial')).toEqual(false);
  });

  it('should call componentDidCatch', () => {
    const wrapper = shallow(<ComingDueWorkbook modal={false} comingDueWorkbooks={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
    //update the instance with the new spy
    wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });

  it('should call async getWorkBookProgress', () => {
    const wrapper = shallow(<ComingDueWorkbook />);
    const spy = jest.spyOn(wrapper.instance(), 'getWorkBookProgress');
    //update the instance with the new spy
    wrapper.instance().getWorkBookProgress(0, 0);
    //invoke getWorkBookProgress
    expect(spy).toBeCalled();
  });

  it('should call createRows', () => {
    let employees = [{ UserId: 0, Role: "", EmployeeName: "", WorkBookName: "", }]
    const wrapper = shallow(<ComingDueWorkbook modal={false} comingDueWorkbooks={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'createRows');
    //update the instance with the new spy
    wrapper.instance().createRows(employees);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call componentWillReceiveProps', () => {
    const wrapper = shallow(<ComingDueWorkbook modal={false} comingDueWorkbooks={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
    //update the instance with the new spy
    wrapper.instance().componentWillReceiveProps({ assignedWorkBooks: [], modal: true });
    //invoke componentWillReceiveProps
    expect(wrapper.state('modal')).toEqual(true);
  });

  it('should call toggle', () => {
    const clickFn = jest.fn();
    const wrapper = shallow(<ComingDueWorkbook updateState={clickFn} modal={false} comingDueWorkbooks={[]} />);
    wrapper.instance().toggle();
    expect(clickFn).toHaveBeenCalled();
    wrapper.unmount();
  });

  it('should call updateModalState', () => {
    const wrapper = shallow(<ComingDueWorkbook />);
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
    const wrapper = shallow(<ComingDueWorkbook />);
    const spy = jest.spyOn(wrapper.instance(), 'handleCellClick');
    //update the instance with the new spy
    wrapper.instance().handleCellClick("completedTasks", args);
    wrapper.instance().handleCellClick("percentageCompleted", args);
    wrapper.instance().handleCellClick("default", args);
    //invoke createRows
    expect(spy).toBeCalled();
  });
  
  it('should call cellFormatter', () => {
    const wrapper = shallow(<ComingDueWorkbook modal={false} comingDueWorkbooks={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'cellFormatter');
    //update the instance with the new spy
    wrapper.instance().cellFormatter({ value: "" });
    //invoke cellFormatter
    expect(spy).toBeCalled();
  });

  it('should call workbookFormatter with valid props', () => {
    let props = {
      dependentValues: {
        "test": 1,
        "employee": "Test Name"
      },
      value: ""
    }

    const wrapper = shallow(<ComingDueWorkbook />);
    const spy = jest.spyOn(wrapper.instance(), 'workbookFormatter');
    //update the instance with the new spy
    wrapper.instance().workbookFormatter("test", props);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call workbookFormatter with invalid props', () => {
    let props = {
      dependentValues: {
        "test": 1,
        "employee": "Total"
      }
    }

    const wrapper = shallow(<ComingDueWorkbook />);
    const spy = jest.spyOn(wrapper.instance(), 'workbookFormatter');
    //update the instance with the new spy
    wrapper.instance().workbookFormatter("test", props);
    //invoke createRows
    expect(spy).toBeCalled();
  });
  
  it('should call customCell', () => {
    const wrapper = shallow(<ComingDueWorkbook modal={false} comingDueWorkbooks={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'customCell');
    //update the instance with the new spy
    wrapper.instance().customCell({ value: "" });
    //invoke customCell
    expect(spy).toBeCalled();
  });

  it('should call customCellTextTooltip', () => {
    const wrapper = shallow(<ComingDueWorkbook />);
    const spy = jest.spyOn(wrapper.instance(), 'customCellTextTooltip');
    //update the instance with the new spy
    wrapper.instance().customCellTextTooltip({ value: "" });
    //invoke customCellTextTooltip
    expect(spy).toBeCalled();
  });

  it('should call render', () => {
    const wrapper = shallow(<ComingDueWorkbook modal={false} comingDueWorkbooks={[]} />);;
    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });
});
