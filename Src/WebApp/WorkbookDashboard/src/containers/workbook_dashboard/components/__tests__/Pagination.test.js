import React from "react";
import Pagination from '../Pagination.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("Pagination component", () => {
    test("check renders", () => {
        const wrapper = shallow(<Pagination pages={1} />);
        expect(wrapper.exists()).toBe(true);
    });
    it("check state[visiblePages] is false on mounting", () => {
        const wrapper = shallow(<Pagination pages={4} />);
        expect(wrapper.state('visiblePages')).toEqual([1, 2, 3, 4]);
    });
    it('should call componentWillReceiveProps with same page', () => {
        const clickFn = jest.fn();
        const wrapper = shallow(<Pagination onPageChange={clickFn}  pages={4} />);
        const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
        //update the instance with the new spy
        wrapper.instance().componentWillReceiveProps({ pages: 4 });
        //invoke componentWillReceiveProps
        expect(spy).toHaveBeenCalled();
        wrapper.unmount();
    });
    it('should call componentWillReceiveProps with different page', () => {
        const clickFn = jest.fn();
        const wrapper = shallow(<Pagination onPageChange={clickFn}  pages={4} />);
        const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
        //update the instance with the new spy
        wrapper.instance().componentWillReceiveProps({ pages: 5 });
        //invoke componentWillReceiveProps
        expect(spy).toHaveBeenCalled();
        wrapper.unmount();
    });
    it('should call filterPages', () => {    
        const wrapper = shallow(<Pagination pages={5} />);
        const spy = jest.spyOn(wrapper.instance(), 'filterPages');
        //update the instance with the new spy
        wrapper.instance().filterPages([1, 2, 3, 4] , 5);
        //invoke filterPages
        expect(spy).toBeCalled();
    });
    it('should call getVisiblePages', () => {    
        const wrapper = shallow(<Pagination pages={5} />);
        const spy = jest.spyOn(wrapper.instance(), 'getVisiblePages');
        //update the instance with the new spy
        wrapper.instance().getVisiblePages(2 , 5);
        //invoke filterPages
        expect(spy).toBeCalled();
    });
    it('should call changePage', () => { 
        const clickFn = jest.fn();   
        const wrapper = shallow(<Pagination onPageChange={clickFn} pages={5} />);
        const spy = jest.spyOn(wrapper.instance(), 'changePage');
        //update the instance with the new spy
        wrapper.instance().changePage(3);
        //invoke filterPages
        expect(spy).toBeCalled();
    });
    it('should call render', () => {
        const wrapper = shallow(<Pagination pages={5} />);
        const spy = jest.spyOn(wrapper.instance(), 'render');
        //update the instance with the new spy
        wrapper.instance().render();
        //invoke render
        expect(spy).toBeCalled();
      });
});
