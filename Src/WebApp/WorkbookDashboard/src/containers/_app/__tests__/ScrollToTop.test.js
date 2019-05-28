import React from "react";
import ScrollToTop from "../ScrollToTop";
import { shallow, mount } from 'enzyme';

import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("ScrollToTop component", () => {
    test("renders", () => {
        const wrapper = shallow(<ScrollToTop />);
        expect(wrapper.exists()).toBe(true);
    });
    it('should call componentDidUpdate', () => {
        const wrapper = shallow(<ScrollToTop />);
        const spy = jest.spyOn(wrapper.instance(), 'componentDidUpdate');
        const prevProps = {
            location:{
                pathname: ""
            }
        };
        //update the instance with the new spy
        wrapper.instance().componentDidUpdate(prevProps);
        //invoke componentDidUpdate
        expect(spy).toBeCalled();
    });
});