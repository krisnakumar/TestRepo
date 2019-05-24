import React from "react";
import ContractorCompanyDetails from '../ContractorCompanyDetail';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("ContractorCompanyDetails component", () => {
  test("check renders", () => {
    const wrapper = shallow(<ContractorCompanyDetails modal={false} userDetails={[]} title={""} />);
    expect(wrapper.exists()).toBe(true);
  });

  it("check state[title] is empty on mounting", () => {
    const wrapper = shallow(<ContractorCompanyDetails modal={false} userDetails={[]} title={""} />);
    expect(wrapper.state('title')).toEqual('');
  });

  it('should call componentDidCatch', () => {
    const wrapper = shallow(<ContractorCompanyDetails modal={false} userDetails={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
    //update the instance with the new spy
    wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });

  it('should call createRows', () => {
    let companyTasks = [{ UserId: 0, RoleId: 0, CompletedCompanyQualification: "", CompanyId: "", InCompletedCompanyQualification: ""}]
    const wrapper = shallow(<ContractorCompanyDetails modal={false} userDetails={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'createRows');
    //update the instance with the new spy
    wrapper.instance().createRows(companyTasks);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call componentWillReceiveProps', () => {
    const wrapper = shallow(<ContractorCompanyDetails modal={false} userDetails={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
    //update the instance with the new spy
    wrapper.instance().componentWillReceiveProps({ companyDetails: [], modal: true });
    //invoke componentWillReceiveProps
    expect(wrapper.state('modal')).toEqual(true);
  });
  
  it('should call async getUserDetails', () => {
    const wrapper = shallow(<ContractorCompanyDetails />);
    const spy = jest.spyOn(wrapper.instance(), 'getUserDetails');
    //update the instance with the new spy
    wrapper.instance().getUserDetails("", 1, false, 1);
    //invoke getUserDetails
    expect(spy).toBeCalled();
  });

  it('should call updateModalState', () => {    
    const wrapper = shallow(<ContractorCompanyDetails modal={false} userDetails={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'updateModalState');
    //update the instance with the new spy
    wrapper.instance().updateModalState("testModal");
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call toggle', () => {
    const clickFn = jest.fn();
    const wrapper = shallow(<ContractorCompanyDetails updateState={clickFn} modal={false} userDetails={[]} title={""} />);
    wrapper.instance().toggle();
    expect(clickFn).toHaveBeenCalled();
    wrapper.unmount();
  });
  
  it('should call handleCellClick', () => {
    let args = {
      roleId: 1,
      companyId: 1,
      company: ""
    };
    const wrapper = shallow(<ContractorCompanyDetails />);
    const spy = jest.spyOn(wrapper.instance(), 'handleCellClick');
    //update the instance with the new spy
    wrapper.instance().handleCellClick("completedUsers", args);
    wrapper.instance().handleCellClick("incompleteUsers", args);
    wrapper.instance().handleCellClick("total", args);
    wrapper.instance().handleCellClick("percentageCompleted", args);
    wrapper.instance().handleCellClick("default", args);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call cellFormatter', () => {
    const wrapper = shallow(<ContractorCompanyDetails />);
    const spy = jest.spyOn(wrapper.instance(), 'cellFormatter');
    //update the instance with the new spy
    wrapper.instance().cellFormatter({ value: "" });
    //invoke cellFormatter
    expect(spy).toBeCalled();
  });

  it('should call cellClickFormatter', () => {
    const wrapper = shallow(<ContractorCompanyDetails />);
    const spy = jest.spyOn(wrapper.instance(), 'cellClickFormatter');
    const props={
      dependentValues: {
        "test": 0
      }
    }
    //update the instance with the new spy
    wrapper.instance().cellClickFormatter("test", props);
    //invoke cellClickFormatter
    expect(spy).toBeCalled();
  });

  it('should call customCell', () => {
    const wrapper = shallow(<ContractorCompanyDetails />);
    const spy = jest.spyOn(wrapper.instance(), 'customCell');
    //update the instance with the new spy
    wrapper.instance().customCell({ value: "" });
    //invoke customCell
    expect(spy).toBeCalled();
  });

  it('should call render', () => {
    const wrapper = shallow(<ContractorCompanyDetails modal={false} userDetails={[]} title={""} />);

    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });

});
