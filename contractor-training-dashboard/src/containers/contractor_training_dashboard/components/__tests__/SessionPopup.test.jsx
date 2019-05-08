import React from "react";
import Enzyme, { shallow, mount } from "enzyme";
import SessionPopup from "../SessionPopup";
import Adapter from "enzyme-adapter-react-16";

Enzyme.configure({ adapter: new Adapter() });

describe("Session Popup", () => {
  test("renders", () => {
    const wrapper = shallow(<SessionPopup />);

    expect(wrapper.exists()).toBe(true);
  });
});
