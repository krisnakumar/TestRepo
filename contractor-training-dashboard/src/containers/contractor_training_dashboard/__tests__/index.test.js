import React from "react";
import Enzyme, { shallow, mount } from "enzyme";
import CTDashboardComponent from "../";
import Adapter from "enzyme-adapter-react-16";

Enzyme.configure({ adapter: new Adapter() });

describe("CT Dashboard Component", () => {
  test("renders", () => {
    const wrapper = mount(<CTDashboardComponent />);

    expect(wrapper.exists()).toBe(true);
  });
});
