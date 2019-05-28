import React from "react";
import Enzyme, { shallow, mount } from "enzyme";
import CTDashboardLayout from "../";
import Adapter from "enzyme-adapter-react-16";

Enzyme.configure({ adapter: new Adapter() });

describe("CT Dashboard Layout", () => {
  test("renders", () => {
    const wrapper = shallow(<CTDashboardLayout />);
    expect(wrapper.exists()).toBe(true);
  });
});
