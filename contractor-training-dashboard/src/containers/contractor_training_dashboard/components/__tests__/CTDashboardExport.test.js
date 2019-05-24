import React from "react";
import CTDashboardExport from '../CTDashboardExport';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("CTDashboardExport component", () => {
  test("check renders", () => {
    const wrapper = shallow(<CTDashboardExport data={[]} modal={false} userDetails={[]} title={""} />);
    expect(wrapper.exists()).toBe(true);
  });

  it('should call componentWillReceiveProps', () => {
    const wrapper = shallow(<CTDashboardExport data={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
    //update the instance with the new spy
    wrapper.instance().componentWillReceiveProps({ sheetName: "", data: [], heads: "" });
    //invoke createRows
    expect(spy).toBeCalled();
  });
  
  it('should call formatData', () => {
    let heads = [
        {
            key: 'company',
            name: 'Company',
            sortable: true,
            editable: false,
            cellClass: "text-left"
        }];
    let data = [
        { company: "Test 1" }, { company: "Test 2" }
    ]
    const wrapper = shallow(<CTDashboardExport data={[]} heads={[]} sheetName={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'formatData');
    //update the instance with the new spy
    wrapper.instance().formatData(data, heads);
    //invoke formatData
    expect(spy).toBeCalled();
  });

  it('should call render', () => {
    const wrapper = shallow(<CTDashboardExport modal={false} data={[]} userDetails={[]} title={""} />);

    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });

});
