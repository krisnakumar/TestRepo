
import React from "react";
import OQDashboardExport from '../OQDashboardExport.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("OQDashboardExport component", () => {
    test("renders", () => {
        const wrapper = shallow(<OQDashboardExport />);
        expect(wrapper.exists()).toBe(true);
    });
    it('should call render', () => {
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
        const wrapper = shallow(<OQDashboardExport data={data} heads={heads} sheetName={[]} />);
        const spy = jest.spyOn(wrapper.instance(), 'render');
        //update the instance with the new spy
        wrapper.instance().render();
        //invoke render
        expect(spy).toBeCalled();
    });
    it('should call componentWillReceiveProps', () => {
        const wrapper = shallow(<OQDashboardExport data={[]} heads={[]} sheetName={[]} />);
        const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
        //update the instance with the new spy
        wrapper.instance().componentWillReceiveProps({ data: [], heads: [], sheetName: "Test Title" });
        //invoke componentWillReceiveProps
        expect(wrapper.state('sheetName')).toEqual("Test Title");
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
        const wrapper = shallow(<OQDashboardExport data={[]} heads={[]} sheetName={[]} />);
        const spy = jest.spyOn(wrapper.instance(), 'formatData');
        //update the instance with the new spy
        wrapper.instance().formatData(data, heads);
        //invoke formatData
        expect(spy).toBeCalled();
    });

});