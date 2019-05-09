import React from "react";
import { create } from "react-test-renderer";
import App from "../App";
import { shallow, mount } from 'enzyme';
import configureStore from 'redux-mock-store'
import { BrowserRouter as Router } from 'react-router-dom';

const middlewares = []
const mockStore = configureStore(middlewares)

import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("App component", () => {
    test("renders", () => {
        const wrapper = shallow(<App />);
        expect(wrapper.exists()).toBe(true);
    });
});