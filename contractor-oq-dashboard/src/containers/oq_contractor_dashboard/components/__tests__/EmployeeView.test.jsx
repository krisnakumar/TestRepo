import React from "react";
import EmployeeView from '../EmployeeView.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("EmployeeView component", () => {

  test("check renders", () => {
    const wrapper = shallow(<EmployeeView />);
    expect(wrapper.exists()).toBe(true);
  });

  it("check state[isInitial] is false on mounting", () => {
    const wrapper = shallow(<EmployeeView />);
    expect(wrapper.state('isInitial')).toEqual(false);
  });

  it('should call cellFormatter', () => {
    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'cellFormatter');
    //update the instance with the new spy
    wrapper.instance().cellFormatter({ value: "" });
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call componentDidCatch', () => {
    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
    //update the instance with the new spy
    wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });

  it('should call createRows', () => {
    let qualifications = [
      {
        UserId: "",
        CompanyId: "",
        EmployeeName: "",
        Role: "",
      }
    ]
    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'createRows');
    //update the instance with the new spy
    wrapper.instance().createRows(qualifications);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call updateModalState', () => {
    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'updateModalState');
    //update the instance with the new spy
    wrapper.instance().updateModalState("Test Modal");
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call componentWillReceiveProps', () => {
    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
    //update the instance with the new spy
    wrapper.instance().componentWillReceiveProps({employeesQualificationsArray: [], modal: false});
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call toggle', () => {
    const popEmployeesQualificationsArrayFn = jest.fn();
    const updateStateFn = jest.fn();
    const wrapper = shallow(<EmployeeView updateState={updateStateFn} popEmployeesQualificationsArray={popEmployeesQualificationsArrayFn}/>);
    const spy = jest.spyOn(wrapper.instance(), 'toggle');
    //update the instance with the new spy
    wrapper.instance().toggle();
    //invoke toggle
    expect(spy).toBeCalled();
  });

  it('should call qualificationsFormatter with valid props', () => {
    let props = {
      dependentValues: {
        "test": 1,
        "contractors": "Test Name"
      }
    }

    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'qualificationsFormatter');
    //update the instance with the new spy
    wrapper.instance().qualificationsFormatter("test", props);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call qualificationsFormatter with invalid props', () => {
    let props = {
      dependentValues: {
        "test": 1,
        "contractors": "Total"
      }
    }

    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'qualificationsFormatter');
    //update the instance with the new spy
    wrapper.instance().qualificationsFormatter("test", props);
    //invoke createRows
    expect(spy).toBeCalled();
  });


 

  it('should call handleCellClick', () => {
    let args = {
      userId: 1,
      companyId: 2,
      company: "Test Company"
    };
    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'handleCellClick');
    //update the instance with the new spy
    wrapper.instance().handleCellClick("contractors", args);
    wrapper.instance().handleCellClick("total", args);
    wrapper.instance().handleCellClick("assignedQualification", args);
    wrapper.instance().handleCellClick("completedQualification", args);
    wrapper.instance().handleCellClick("inCompletedQualification", args);
    wrapper.instance().handleCellClick("pastDue", args);
    wrapper.instance().handleCellClick("comingDue", args);
    wrapper.instance().handleCellClick("lockoutCount", args);
    wrapper.instance().handleCellClick("default", args);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call async getMyEmployees', ()  => {
    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'getMyEmployees');
    //update the instance with the new spy
    wrapper.instance().getMyEmployees();
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call async getAssignedQualifications', () => {
    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'getAssignedQualifications');
    //update the instance with the new spy
    wrapper.instance().getAssignedQualifications(1, [1, 2 ,3]);
    //invoke getAssignedQualifications
    expect(spy).toBeCalled();
  });

  it('should call async getCompletedQualifications', () => {
    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'getCompletedQualifications');
    //update the instance with the new spy
    wrapper.instance().getCompletedQualifications(1, 2);
    //invoke getCompletedQualifications
    expect(spy).toBeCalled();
  });

  it('should call async getInCompletedQualifications', () => {
    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'getInCompletedQualifications');
    //update the instance with the new spy
    wrapper.instance().getInCompletedQualifications(1, 2);
    //invoke getInCompletedQualifications
    expect(spy).toBeCalled();
  });

  it('should call async getPastDueQualifications', () => {
    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'getPastDueQualifications');
    //update the instance with the new spy
    wrapper.instance().getPastDueQualifications(1, 2);
    //invoke getPastDueQualifications
    expect(spy).toBeCalled();
  });

  it('should call async getComingDueQualifications', () => {
    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'getComingDueQualifications');
    //update the instance with the new spy
    wrapper.instance().getComingDueQualifications(1, 2);
    //invoke getComingDueQualifications
    expect(spy).toBeCalled();
  });

  it('should call async getLockedOutQualifications', () => {
    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'getLockedOutQualifications');
    //update the instance with the new spy
    wrapper.instance().getLockedOutQualifications(1, 2);
    //invoke getLockedOutQualifications
    expect(spy).toBeCalled();
  });

  it('should call customCell', () => {
    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'customCell');
    //update the instance with the new spy
    wrapper.instance().customCell({ value: "" });
    //invoke customCell
    expect(spy).toBeCalled();
  });

  it('should call render', () => {
    const wrapper = shallow(<EmployeeView />);
    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });

});
