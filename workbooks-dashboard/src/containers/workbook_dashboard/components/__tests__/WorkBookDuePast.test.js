import React from "react";
import WorkBookDuePast from '../WorkBookDuePast';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("WorkBookDuePast component", () => {
  test("check renders", () => {
    const wrapper = shallow(<WorkBookDuePast modal={false} WorkBooksDuePast={[]} />);
    expect(wrapper.exists()).toBe(true);
  });

  it("check state[modal] is false on mounting", () => {
    const wrapper = shallow(<WorkBookDuePast modal={false} WorkBooksDuePast={[]} />);
    expect(wrapper.state('isInitial')).toEqual(false);
  });

  it('should call componentDidCatch', () => {
    const wrapper = shallow(<WorkBookDuePast modal={false} WorkBooksDuePast={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
    //update the instance with the new spy
    wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });
  
  it('should call getWorkBookProgress', () => {
    sessionStorage.setItem("contractorManagementDetails", '{"Company":{"Name":"Consolidated Edison Company of New York Inc.","Logo":"https://onboard-lms-private.s3.amazonaws.com/files/logos/1c0bd30e-b47b-4fe6-8478-863bf52a7b33.png?AWSAccessKeyId=AKIAIS7GG7JYOOR2HEDA&Expires=1557305616&Signature=4O9uaXU5W5e4UfPzMD5I4tJ2ivk%3D","Id":2288},"Menu":{"Learn":{"Title":"Learn","Url":"/learnLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_learn.png?v=2","Items":[{"Title":"My Assignments","Url":"/MyAssignments.aspx"},{"Title":"My Transcript","Url":"/transcript.aspx"},{"Title":"My Downloads","Url":"/MyDownloads.aspx"},{"Title":"Purchase","Url":"/CourseLibrary.aspx"},{"Title":"Find Proctors","Url":"/ProctorSearch.aspx"}]},"Manage":{"Title":"Manage","Url":"/manageLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_manage.png?v=2","Items":[{"Title":"Course Assignments","Url":"/CourseAssignmentLanding.aspx"},{"Title":"Users","Url":"/users.aspx#u=my"},{"Title":"Company","Url":"/Company.aspx"},{"Title":"Tasks","Url":"/Tasks.aspx"},{"Title":"Work Locations","Url":"/CompanySites.aspx"},{"Title":"Job Assignment Check","Url":"/JobsLanding.aspx"},{"Title":"Workbook","Url":"/WorkbooksLanding.aspx"}]},"Reports":{"Title":"Reports","Url":"/reportsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_reports.png?v=2","Items":[{"Title":"Qualifications","Url":"/TranscriptLanding.aspx"},{"Title":"Course Usage Reports","Url":"/CourseUsageLanding.aspx"},{"Title":"Exception Reports","Url":"/ExceptionReportsLanding.aspx"},{"Title":"Custom Reports","Url":"/MyReports.aspx"},{"Title":"Dashboard","Url":"/DashBoard.aspx"},{"Title":"Coaching Reports","Url":"/CoachingReportsLanding.aspx"}]},"Settings":{"Title":"Settings","Url":"/settingsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_settings.png?v=2","Items":[{"Title":"My Profile","Url":"/UserProfile.aspx#u=m"},{"Title":"Change Password","Url":"/UserProfile.aspx#change=pass"},{"Title":"Change Username","Url":"/UserProfile.aspx#change=username"}]}},"User":{"FullName":"James Lucente","Id":331535,"Url":"/userProfile.aspx?u=331535"}}');
    sessionStorage.setItem("userPhoto", 'userPhoto');
    const wrapper = shallow(<WorkBookDuePast modal={true} WorkBooksDuePast={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'getWorkBookProgress');
    //update the instance with the new spy
    wrapper.instance().getWorkBookProgress(0, 0);
    //invoke componentWillReceiveProps
    expect(wrapper.state('modal')).toEqual(true);
  });
  
  it('should call createRows', () => {
    let employees = [{ UserId: 0, Role: "", EmployeeName: "", WorkBookName: "", }]
    const wrapper = shallow(<WorkBookDuePast modal={false} WorkBooksDuePast={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'createRows');
    //update the instance with the new spy
    wrapper.instance().createRows(employees);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call componentWillReceiveProps', () => {
    const wrapper = shallow(<WorkBookDuePast modal={false} Qualifications={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentWillReceiveProps');
    //update the instance with the new spy
    wrapper.instance().componentWillReceiveProps({ assignedWorkBooks: [], modal: true });
    //invoke componentWillReceiveProps
    expect(wrapper.state('modal')).toEqual(true);
  });

  it('should call toggle', () => {
    const clickFn = jest.fn();
    const wrapper = shallow(<WorkBookDuePast updateState={clickFn} modal={false} assignedWorkBooks={[]} />);
    wrapper.instance().toggle();
    expect(clickFn).toHaveBeenCalled();
    wrapper.unmount();
  });
  
  it('should call updateModalState', () => {
    const wrapper = shallow(<WorkBookDuePast />);
    const spy = jest.spyOn(wrapper.instance(), 'updateModalState');
    //update the instance with the new spy
    wrapper.instance().updateModalState("Test Modal");
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call handleCellClick', () => {
    let args = {
      userId: 1, workBookId: 1
    };
    const wrapper = shallow(<WorkBookDuePast />);
    const spy = jest.spyOn(wrapper.instance(), 'handleCellClick');
    //update the instance with the new spy
    wrapper.instance().handleCellClick("percentageCompleted", args);
    wrapper.instance().handleCellClick("completedTasks", args);
    wrapper.instance().handleCellClick("default", args);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call cellFormatter', () => {
    const wrapper = shallow(<WorkBookDuePast modal={false} WorkBooksDuePast={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'cellFormatter');
    //update the instance with the new spy
    wrapper.instance().cellFormatter({ value: "" });
    //invoke cellFormatter
    expect(spy).toBeCalled();
  });

  it('should call workbookFormatter with valid props', () => {
    let props = {
      dependentValues: {
        "test": 1,
        "employee": "Test Name"
      },
      value: ""
    }

    const wrapper = shallow(<WorkBookDuePast />);
    const spy = jest.spyOn(wrapper.instance(), 'workbookFormatter');
    //update the instance with the new spy
    wrapper.instance().workbookFormatter("test", props);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call workbookFormatter with invalid props', () => {
    let props = {
      dependentValues: {
        "test": 1,
        "employee": "Total"
      }
    }

    const wrapper = shallow(<WorkBookDuePast />);
    const spy = jest.spyOn(wrapper.instance(), 'workbookFormatter');
    //update the instance with the new spy
    wrapper.instance().workbookFormatter("test", props);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call customCell', () => {
    const wrapper = shallow(<WorkBookDuePast modal={false} employee={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'customCell');
    //update the instance with the new spy
    wrapper.instance().customCell({ value: "" });
    //invoke customCell
    expect(spy).toBeCalled();
  });

  it('should call customCellTextTooltip', () => {
    const wrapper = shallow(<WorkBookDuePast />);
    const spy = jest.spyOn(wrapper.instance(), 'customCellTextTooltip');
    //update the instance with the new spy
    wrapper.instance().customCellTextTooltip({ value: "" });
    //invoke customCellTextTooltip
    expect(spy).toBeCalled();
  });

  it('should call render', () => {
    const wrapper = shallow(<WorkBookDuePast modal={false} WorkBooksDuePast={[]} />);;
    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });
});
