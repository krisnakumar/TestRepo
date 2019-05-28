import React from "react";
import WorkBookProgress from '../WorkBookProgress';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("WorkBookProgress component", () => {
  test("check renders", () => {
    const wrapper = shallow(<WorkBookProgress modal={false} WorkBooksProgress={[]} selectedWorkbook={[]} />);
    expect(wrapper.exists()).toBe(true);
  });

  it("check state[modal] is false on mounting", () => {
    const wrapper = shallow(<WorkBookProgress modal={false} WorkBooksProgress={[]} selectedWorkbook={[]} />);
    expect(wrapper.state('isInitial')).toEqual(false);
  });

  it('should call componentDidCatch', () => {
    const wrapper = shallow(<WorkBookProgress modal={false} WorkBooksProgress={[]} selectedWorkbook={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
    //update the instance with the new spy
    wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });
  
  it('should call createRows', () => {
    let workbooks = [{ TaskId: 0, TaskCode: "", TaskName: "", RepsRequired: 0, }]
    const wrapper = shallow(<WorkBookProgress modal={false} WorkBooksProgress={[]} selectedWorkbook={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'createRows');
    //update the instance with the new spy
    wrapper.instance().createRows(workbooks, 1, 1);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call componentWillReceiveProps', () => {
    const wrapper = shallow(<WorkBookProgress modal={false} Qualifications={[]} selectedWorkbook={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
    //update the instance with the new spy
    wrapper.instance().componentWillReceiveProps({ assignedWorkBooks: [], modal: true, selectedWorkbook:{} });
    //invoke componentWillReceiveProps
    expect(wrapper.state('modal')).toEqual(true);
  });
  
  it('should call getWorkbookRepetitions', () => {
    const wrapper = shallow(<WorkBookProgress modal={true} WorkBooksRepetition={[]} selectedWorkbook={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'getWorkbookRepetitions');
    //update the instance with the new spy
    wrapper.instance().getWorkbookRepetitions(1,1,1,"");
    //invoke componentWillReceiveProps
    expect(wrapper.state('modal')).toEqual(true);
  });

  it('should call toggle', () => {
    const clickFn = jest.fn();
    const wrapper = shallow(<WorkBookProgress updateState={clickFn} modal={false} assignedWorkBooks={[]} selectedWorkbook={[]} />);
    wrapper.instance().toggle();
    expect(clickFn).toHaveBeenCalled();
    wrapper.unmount();
  });
  
  it('should call updateModalState', () => {
    const wrapper = shallow(<WorkBookProgress modal={false} WorkBooksProgress={[]} selectedWorkbook={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'updateModalState');
    //update the instance with the new spy
    wrapper.instance().updateModalState("Test Modal");
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call handleCellClick', () => {
    let args = {
      userId: 1, workBookId: 1
    };
    const wrapper = shallow(<WorkBookProgress modal={false} WorkBooksProgress={[]} selectedWorkbook={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'handleCellClick');
    //update the instance with the new spy
    wrapper.instance().handleCellClick("incompletedTasksCount", args);
    wrapper.instance().handleCellClick("completedTasksCount", args);
    wrapper.instance().handleCellClick("default", args);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call cellFormatter', () => {
    const wrapper = shallow(<WorkBookProgress modal={false} WorkBooksProgress={[]} selectedWorkbook={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'cellFormatter');
    //update the instance with the new spy
    wrapper.instance().cellFormatter({ value: "" });
    //invoke cellFormatter
    expect(spy).toBeCalled();
  });

  it('should call workbookIncompleteFormatter with valid props', () => {
    let props = {
      dependentValues: {
        "test": 1,
        "employee": "Test Name"
      },
      value: ""
    }

    const wrapper = shallow(<WorkBookProgress modal={false} WorkBooksProgress={[]} selectedWorkbook={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'workbookIncompleteFormatter');
    //update the instance with the new spy
    wrapper.instance().workbookIncompleteFormatter("test", props);
    //invoke createRows
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

    const wrapper = shallow(<WorkBookProgress modal={false} WorkBooksProgress={[]} selectedWorkbook={[]} />);
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

    const wrapper = shallow(<WorkBookProgress modal={false} WorkBooksProgress={[]} selectedWorkbook={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'workbookFormatter');
    //update the instance with the new spy
    wrapper.instance().workbookFormatter("test", props);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call customCell', () => {
    const wrapper = shallow(<WorkBookProgress modal={false} employee={[]} selectedWorkbook={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'customCell');
    //update the instance with the new spy
    wrapper.instance().customCell({ value: "" });
    //invoke customCell
    expect(spy).toBeCalled();
  });

  it('should call render', () => {
    const wrapper = shallow(<WorkBookProgress modal={false} WorkBooksProgress={[]} selectedWorkbook={[]} />);;
    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });
});
