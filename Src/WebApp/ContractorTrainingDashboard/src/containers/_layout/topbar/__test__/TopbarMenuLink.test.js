import React from "react";
import Enzyme, { shallow, mount } from "enzyme";
import TopbarMenuLink from "../TopbarMenuLink";
import Adapter from "enzyme-adapter-react-16";

Enzyme.configure({ adapter: new Adapter() });

describe("TopbarMenuLink", () => {
  test("renders", () => {
    const wrapper = shallow(<TopbarMenuLink title={""} icon={""} path={""}/>);
    expect(wrapper.exists()).toBe(true);
  });
});
