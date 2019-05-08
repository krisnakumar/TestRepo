import React from "react";
import Enzyme, { shallow, mount } from "enzyme";
import Topbar from "../Topbar";
import Adapter from "enzyme-adapter-react-16";

Enzyme.configure({ adapter: new Adapter() });

describe("Topbar", () => {
  test("renders", () => {
    const wrapper = mount(<Topbar />);

    expect(wrapper.exists()).toBe(true);
  });
});
