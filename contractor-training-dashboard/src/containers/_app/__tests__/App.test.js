import React from "react";
import Enzyme, { shallow, mount } from "enzyme";
import App from "../App";
import Adapter from "enzyme-adapter-react-16";
import { create } from "react-test-renderer";

Enzyme.configure({ adapter: new Adapter() });

describe("App", () => {
  test("renders", () => {
    const wrapper = shallow(<App />);

    expect(wrapper.exists()).toBe(true);
  });

  // it('invokes `componentDidMount` when mounted', () => {
  //   jest.spyOn(App.prototype, 'componentDidMount');
  //   shallow(<App />);
  //   expect(App.prototype.componentDidMount).toHaveBeenCalled();
  //   App.prototype.componentDidMount.mockRestore();
  // });

  // it('invokes `componentWillMount` when mounted', () => {
  //   jest.spyOn(App.prototype, 'componentWillMount');
  //   shallow(<App />);
  //   expect(App.prototype.componentWillMount).toHaveBeenCalled();
  //   App.prototype.componentWillMount.mockRestore();
  // });

  it('invokes `cancelAutoLogout` when mounted', () => {
    const wrapper = mount(<App />);
    const instance = wrapper.instance();
    // spy on the instance instead of the component
    spyOn(instance, 'cancelAutoLogout').and.callThrough();
    expect(instance.cancelAutoLogout()).toHaveBeenCalled();
  });

  it('invokes `updateSessionTokens` when mounted', () => {
    const wrapper = mount(<App />);
    const instance = wrapper.instance();
    // spy on the instance instead of the component
    spyOn(instance, 'updateSessionTokens').and.callThrough();
    expect(instance.updateSessionTokens()).toHaveBeenCalled();
  });

});

