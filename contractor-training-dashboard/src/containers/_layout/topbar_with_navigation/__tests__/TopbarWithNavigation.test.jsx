import React from "react";
import Enzyme, { shallow, mount } from "enzyme";
import TopbarWithNavigation from "../TopbarWithNavigation";
import Adapter from "enzyme-adapter-react-16";

Enzyme.configure({ adapter: new Adapter() });

describe("TopbarWithNavigation", () => {
  test("renders", () => {
    const wrapper = mount(<TopbarWithNavigation />);

    expect(wrapper.exists()).toBe(true);
  });
});
