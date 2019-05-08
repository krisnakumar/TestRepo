import React from "react";
import Enzyme, { shallow, mount } from "enzyme";
import Router from "../Router";
import Adapter from "enzyme-adapter-react-16";

Enzyme.configure({ adapter: new Adapter() });

describe("Router", () => {
  test("renders", () => {
    const wrapper = shallow(<Router />);

    expect(wrapper.exists()).toBe(true);
  });
});
