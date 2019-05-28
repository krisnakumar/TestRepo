import React from "react";
import FilterModal from '../FilterModal.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("FilterModal component", () => {
  test("check renders", () => {
    const wrapper = shallow(<FilterModal modal={false} filterOptionsRoles={[]} title={""} />);
    expect(wrapper.exists()).toBe(true);
  });

  it("check state[title] is empty on mounting", () => {
    const wrapper = shallow(<FilterModal modal={false} filterOptionsRoles={[]} title={""} />);
    expect(wrapper.state('title')).toEqual('');
  });

  it('should call componentDidCatch', () => {
    const wrapper = shallow(<FilterModal modal={false} filterOptionsRoles={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
    //update the instance with the new spy
    wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });

  it('should call componentWillReceiveProps', () => {
    const wrapper = shallow(<FilterModal modal={false} filterOptionsRoles={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
    //update the instance with the new spy
    wrapper.instance().componentWillReceiveProps({ lockoutQualifications: [], modal: true });
    //invoke componentWillReceiveProps
    expect(wrapper.state('modal')).toEqual(true);
  });

  it('should call updateModalState', () => {    
    const wrapper = shallow(<FilterModal modal={false} filterOptionsRoles={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'updateModalState');
    //update the instance with the new spy
    wrapper.instance().updateModalState("testModal");
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call toggle', () => {
    const clickFn = jest.fn();
    const wrapper = shallow(<FilterModal updateState={clickFn} modal={false} filterOptionsRoles={[]} title={""} />);
    wrapper.instance().toggle();
    expect(clickFn).toHaveBeenCalled();
    wrapper.unmount();
  });

  it('should call selectMultipleOption', () => {
    const wrapper = shallow(<FilterModal modal={false} filterOptionsRoles={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'selectMultipleOption');
    //update the instance with the new spy
    wrapper.instance().selectMultipleOption(true, "", [1, 2]);
    wrapper.instance().selectMultipleOption(false, "", [1, 2]);
    //invoke cellFormatter
    expect(spy).toBeCalled();
  });

  it('should call refreshList', () => {
    const wrapper = shallow(<FilterModal modal={false} filterOptionsRoles={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'refreshList');
    //update the instance with the new spy
    wrapper.instance().refreshList();
    //invoke render
    expect(spy).toBeCalled();
  });

  it('should call handleInputChange', () => {
    const wrapper = shallow(<FilterModal modal={false} filterOptionsRoles={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'handleInputChange');
    const updateStateFn = jest.fn();
    const eventObj = {preventDefault:updateStateFn,
      target:{
          name:"", value:""
      }}
    //update the instance with the new spy
    wrapper.instance().handleInputChange(eventObj);
    //invoke render
    expect(spy).toBeCalled();
  });

  it('should call handleDelete', () => {
    const wrapper = shallow(<FilterModal modal={false} filterOptionsRoles={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'handleDelete');
    //update the instance with the new spy
    wrapper.instance().handleDelete(1);
    //invoke render
    expect(spy).toBeCalled();
  });

  it('should call passSelectedData', () => {
    const updateSelectedDataFn = jest.fn();
    const updateStateFn = jest.fn();
    const wrapper = shallow(<FilterModal updateState={updateStateFn} updateSelectedData={updateSelectedDataFn} modal={false} filterOptionsRoles={[]} title={""} />);

    const spy = jest.spyOn(wrapper.instance(), 'passSelectedData');
    //update the instance with the new spy
    wrapper.instance().passSelectedData();
    //invoke render
    expect(updateSelectedDataFn).toBeCalled();
  });

  it('should call resetSelectedData', () => {
    const updateSelectedDataFn = jest.fn();
    const updateStateFn = jest.fn();
    const wrapper = shallow(<FilterModal updateState={updateStateFn} updateSelectedData={updateSelectedDataFn} modal={false} filterOptionsRoles={[]} title={""} />);

    const spy = jest.spyOn(wrapper.instance(), 'resetSelectedData');
    //update the instance with the new spy
    wrapper.instance().resetSelectedData();
    //invoke render
    expect(updateSelectedDataFn).toBeCalled();
  });

  it('should call filterOptions', () => {
    const wrapper = shallow(<FilterModal modal={false} filterOptionsRoles={[]} title={""} />);

    const spy = jest.spyOn(wrapper.instance(), 'filterOptions');
    //update the instance with the new spy
    wrapper.instance().filterOptions("", []);
    //invoke render
    expect(spy).toBeCalled();
  });


  it('should call render', () => {
    const wrapper = shallow(<FilterModal modal={false} filterOptionsRoles={[]} title={""} />);

    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });

});
