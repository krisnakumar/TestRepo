import React from "react";
import Enzyme, { shallow, mount } from "enzyme";
import LoginFormIndex from "../";
import Adapter from "enzyme-adapter-react-16";

Enzyme.configure({ adapter: new Adapter() });

describe("CT Dashboard Layout", () => {
  test("renders", () => {
    const wrapper = shallow(<LoginFormIndex />);
    expect(wrapper.exists()).toBe(true);
  });
});
