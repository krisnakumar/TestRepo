import React from "react";
import Enzyme, { shallow, mount } from "enzyme";
import ScrollToTop from "../ScrollToTop";
import Adapter from "enzyme-adapter-react-16";

Enzyme.configure({ adapter: new Adapter() });

describe("ScrollToTop", () => {
  test("renders", () => {
    const wrapper = shallow(<ScrollToTop />);

    expect(wrapper.exists()).toBe(true);
  });
});
