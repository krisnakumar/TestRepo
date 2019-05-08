import React from "react";
import Enzyme, { shallow, mount } from "enzyme";
import LoginForm from "../LogInForm";
import Adapter from "enzyme-adapter-react-16";

Enzyme.configure({ adapter: new Adapter() });

describe("Login Form", () => {
  test("renders", () => {
    const wrapper = shallow(<LoginForm />);

    expect(wrapper.exists()).toBe(true);
  });
});
