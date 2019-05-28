import React from "react";
import Enzyme, { shallow, mount } from "enzyme";
import MainWrapper from "../MainWrapper";
import Adapter from "enzyme-adapter-react-16";

Enzyme.configure({ adapter: new Adapter() });

describe("Main Wrapper", () => {
  test("renders", () => {
    // const wrapper = shallow(<p> Test </p>);
    const wrapper = shallow(<MainWrapper />);
    expect(wrapper.exists()).toBe(true);
  });
});
