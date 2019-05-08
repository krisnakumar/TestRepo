import React from "react";
import Enzyme, { shallow, mount } from "enzyme";
import TopbarProfile from "../TopbarProfile";
import Adapter from "enzyme-adapter-react-16";

Enzyme.configure({ adapter: new Adapter() });

describe("TopbarProfile", () => {
  test("renders", () => {
    const wrapper = mount(<TopbarProfile />);

    expect(wrapper.exists()).toBe(true);
  });
});
