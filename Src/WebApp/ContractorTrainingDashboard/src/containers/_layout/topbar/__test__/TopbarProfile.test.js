
import React from "react";
import TopbarProfile from '../TopbarProfile.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("TopbarProfile component", () => {
    test("renders", () => {
        sessionStorage.setItem("contractorManagementDetails", '{"Company":{"Name":"Consolidated Edison Company of New York Inc.","Logo":"https://onboard-lms-private.s3.amazonaws.com/files/logos/1c0bd30e-b47b-4fe6-8478-863bf52a7b33.png?AWSAccessKeyId=AKIAIS7GG7JYOOR2HEDA&Expires=1557305616&Signature=4O9uaXU5W5e4UfPzMD5I4tJ2ivk%3D","Id":2288},"Menu":{"Learn":{"Title":"Learn","Url":"/learnLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_learn.png?v=2","Items":[{"Title":"My Assignments","Url":"/MyAssignments.aspx"},{"Title":"My Transcript","Url":"/transcript.aspx"},{"Title":"My Downloads","Url":"/MyDownloads.aspx"},{"Title":"Purchase","Url":"/CourseLibrary.aspx"},{"Title":"Find Proctors","Url":"/ProctorSearch.aspx"}]},"Manage":{"Title":"Manage","Url":"/manageLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_manage.png?v=2","Items":[{"Title":"Course Assignments","Url":"/CourseAssignmentLanding.aspx"},{"Title":"Users","Url":"/users.aspx#u=my"},{"Title":"Company","Url":"/Company.aspx"},{"Title":"Tasks","Url":"/Tasks.aspx"},{"Title":"Work Locations","Url":"/CompanySites.aspx"},{"Title":"Job Assignment Check","Url":"/JobsLanding.aspx"},{"Title":"Workbook","Url":"/WorkbooksLanding.aspx"}]},"Reports":{"Title":"Reports","Url":"/reportsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_reports.png?v=2","Items":[{"Title":"Qualifications","Url":"/TranscriptLanding.aspx"},{"Title":"Course Usage Reports","Url":"/CourseUsageLanding.aspx"},{"Title":"Exception Reports","Url":"/ExceptionReportsLanding.aspx"},{"Title":"Custom Reports","Url":"/MyReports.aspx"},{"Title":"Dashboard","Url":"/DashBoard.aspx"},{"Title":"Coaching Reports","Url":"/CoachingReportsLanding.aspx"}]},"Settings":{"Title":"Settings","Url":"/settingsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_settings.png?v=2","Items":[{"Title":"My Profile","Url":"/UserProfile.aspx#u=m"},{"Title":"Change Password","Url":"/UserProfile.aspx#change=pass"},{"Title":"Change Username","Url":"/UserProfile.aspx#change=username"}]}},"User":{"FullName":"James Lucente","Id":331535,"Url":"/userProfile.aspx?u=331535"}}');
        sessionStorage.setItem("userPhoto", 'userPhoto');
        const wrapper = shallow(<TopbarProfile />);
        expect(wrapper.exists()).toBe(true);
    });
    it("check state[modal] is false on mounting", () => {
        const wrapper = shallow(<TopbarProfile />);
        expect(wrapper.state('collapse')).toEqual(false);
    });
    it('should call toggle', () => {
        const wrapper = shallow(<TopbarProfile />);
        const spy = jest.spyOn(wrapper.instance(), 'toggle');
        //update the instance with the new spy
        wrapper.instance().toggle();
        //invoke toggle
        expect(spy).toBeCalled();
    });
    it('should call render', () => {
        sessionStorage.setItem("contractorManagementDetails", '{"Company":{"Name":"Consolidated Edison Company of New York Inc.","Logo":"https://onboard-lms-private.s3.amazonaws.com/files/logos/1c0bd30e-b47b-4fe6-8478-863bf52a7b33.png?AWSAccessKeyId=AKIAIS7GG7JYOOR2HEDA&Expires=1557305616&Signature=4O9uaXU5W5e4UfPzMD5I4tJ2ivk%3D","Id":2288},"Menu":{"Learn":{"Title":"Learn","Url":"/learnLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_learn.png?v=2","Items":[{"Title":"My Assignments","Url":"/MyAssignments.aspx"},{"Title":"My Transcript","Url":"/transcript.aspx"},{"Title":"My Downloads","Url":"/MyDownloads.aspx"},{"Title":"Purchase","Url":"/CourseLibrary.aspx"},{"Title":"Find Proctors","Url":"/ProctorSearch.aspx"}]},"Manage":{"Title":"Manage","Url":"/manageLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_manage.png?v=2","Items":[{"Title":"Course Assignments","Url":"/CourseAssignmentLanding.aspx"},{"Title":"Users","Url":"/users.aspx#u=my"},{"Title":"Company","Url":"/Company.aspx"},{"Title":"Tasks","Url":"/Tasks.aspx"},{"Title":"Work Locations","Url":"/CompanySites.aspx"},{"Title":"Job Assignment Check","Url":"/JobsLanding.aspx"},{"Title":"Workbook","Url":"/WorkbooksLanding.aspx"}]},"Reports":{"Title":"Reports","Url":"/reportsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_reports.png?v=2","Items":[{"Title":"Qualifications","Url":"/TranscriptLanding.aspx"},{"Title":"Course Usage Reports","Url":"/CourseUsageLanding.aspx"},{"Title":"Exception Reports","Url":"/ExceptionReportsLanding.aspx"},{"Title":"Custom Reports","Url":"/MyReports.aspx"},{"Title":"Dashboard","Url":"/DashBoard.aspx"},{"Title":"Coaching Reports","Url":"/CoachingReportsLanding.aspx"}]},"Settings":{"Title":"Settings","Url":"/settingsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_settings.png?v=2","Items":[{"Title":"My Profile","Url":"/UserProfile.aspx#u=m"},{"Title":"Change Password","Url":"/UserProfile.aspx#change=pass"},{"Title":"Change Username","Url":"/UserProfile.aspx#change=username"}]}},"User":{"FullName":"James Lucente","Id":331535,"Url":"/userProfile.aspx?u=331535"}}');
        sessionStorage.setItem("userPhoto", 'userPhoto');
        const wrapper = shallow(<TopbarProfile />);
        const spy = jest.spyOn(wrapper.instance(), 'render');
        //update the instance with the new spy
        wrapper.instance().render();
        //invoke render
        expect(spy).toBeCalled();
    });
});