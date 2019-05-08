import React from "react";
import Enzyme, { shallow, mount } from "enzyme";
import CTDashboard from "./CTDashboard";
import Adapter from "enzyme-adapter-react-16";

Enzyme.configure({ adapter: new Adapter() });

describe("CT Dashboard", () => {
  test("renders", () => {
    const wrapper = shallow(<CTDashboard />);

    expect(wrapper.exists()).toBe(true);
  });
});
