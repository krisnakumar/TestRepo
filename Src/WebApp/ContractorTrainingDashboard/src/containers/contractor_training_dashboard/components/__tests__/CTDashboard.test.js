import React from "react";
import CTDashboard from '../CTDashboard.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("CTDashboard component", () => {
  test("check renders", () => {
    const wrapper = shallow(<CTDashboard modal={false} userDetails={[]} title={""} />);
    expect(wrapper.exists()).toBe(true);
  });
  
  it('should call cellFormatter', () => {
    const wrapper = shallow(<CTDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'cellFormatter');
    //update the instance with the new spy
    wrapper.instance().cellFormatter({ value: "" });
    //invoke cellFormatter
    expect(spy).toBeCalled();
  });
  
  it('should call roleDetailsFormatter with valid props', () => {
    let props = {
      dependentValues: {
        "test": 0,
        "contractors": "Total"
      },
      value: ""
    }

    const wrapper = shallow(<CTDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'roleDetailsFormatter');
    //update the instance with the new spy
    wrapper.instance().roleDetailsFormatter("test", props);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call componentDidCatch', () => {
    const wrapper = shallow(<CTDashboard modal={false} userDetails={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
    //update the instance with the new spy
    wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });

  it('should call componentDidMount', () => {
    sessionStorage.setItem("dashboardAPIToken", '{"dashboardAPIToken":{"AccessToken":"eyJraWQiOiJ1MUpCRFZ5eGJ0T1MzcFBSTm9JdWJjYUs5ZjJYQ2dlOXJyTXJWQjEzc0J3PSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiJkZWE2YTEwZi04OTk0LTRhNDYtOGE2MC0wMjk4M2Q2M2I1MGEiLCJldmVudF9pZCI6IjRjZjEzYTMxLTc1N2QtMTFlOS04YjhkLTkxYmIwMmE4M2RmYSIsInRva2VuX3VzZSI6ImFjY2VzcyIsInNjb3BlIjoiYXdzLmNvZ25pdG8uc2lnbmluLnVzZXIuYWRtaW4iLCJhdXRoX3RpbWUiOjE1NTc3NTE2NjMsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy13ZXN0LTIuYW1hem9uYXdzLmNvbVwvdXMtd2VzdC0yX0Y1NGo2bTNUMiIsImV4cCI6MTU1Nzc1NTI2MywiaWF0IjoxNTU3NzUxNjYzLCJqdGkiOiJkMDJiNjg2Ny05OTdjLTRhMzAtYjFkNC0wYTBhYjAzMzdiYzgiLCJjbGllbnRfaWQiOiI1aHU0OHJkM21iZnZyM3Q2ZDZyMnJjaGFwbSIsInVzZXJuYW1lIjoibHVjZW50ZWpfMzMxNTM1In0.Ac9Yq0J1R43eYH8V49K4kFr6U468KuYtdqqdfbqwekm2PncpOk4V5EYAn_AbTsMEf1HllTBzQbOmRJpT6hdK7b2UIhHmDj-SVP3B3q5-OHnxRNnAf0cjwlPByx0FaMbK4JXVcvyATcNEW7cX9FDU52q9QsJ6e8RsYiIsbQ0YJHo8QKTmwxcbARzwzmYuw3DwY8bWftT-l2omvaPPK4ZfnBzsDf8Xa3a7KzW7BfIUQKhhlYlCX5LxAYebJBS-oUdqpdObJ8cZ6dRSwdep-E6LtlQqv95cKvrjev6HjGLRyuTO7gzrDVO4I7AP0m6g4dVhrQtf6385Ihq2bN9CJ1j8Sg","ExpiresIn":3600,"IdToken":"eyJraWQiOiJ5REQ0ZjA5bm9ScndSc2hKMXM3OEFhUW0wUHVUZjVBUFwvQ3A2SkFjS0V4RT0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJkZWE2YTEwZi04OTk0LTRhNDYtOGE2MC0wMjk4M2Q2M2I1MGEiLCJhdWQiOiI1aHU0OHJkM21iZnZyM3Q2ZDZyMnJjaGFwbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJldmVudF9pZCI6IjRjZjEzYTMxLTc1N2QtMTFlOS04YjhkLTkxYmIwMmE4M2RmYSIsInRva2VuX3VzZSI6ImlkIiwiYXV0aF90aW1lIjoxNTU3NzUxNjYzLCJpc3MiOiJodHRwczpcL1wvY29nbml0by1pZHAudXMtd2VzdC0yLmFtYXpvbmF3cy5jb21cL3VzLXdlc3QtMl9GNTRqNm0zVDIiLCJjdXN0b206dXNlcl9pZCI6IjMzMTUzNSIsImNvZ25pdG86dXNlcm5hbWUiOiJsdWNlbnRlal8zMzE1MzUiLCJleHAiOjE1NTc3NTUyNjMsImlhdCI6MTU1Nzc1MTY2MywiZW1haWwiOiJuby1yZXBseUBpdHMtdHJhaW5pbmcuY29tIn0.D7pyWRXx9AHN1LYk8I0mQeyn3_AbPpK6r53ujcA_Bwy-lF3bue6CLrGSWteWf1nE1ddaPju212skbypn0SeMzPrMwoF6rBua5cFZ2pgcKyo7HNeckjHeyfjdtmKZnmJb_epSLD2m0s7wo_W0dYMbzvPIXKxna9fa49kgYn2BZbNEkExF_uOcfa7hLiY_UHVSL2se-QrAK5RToOOXFWx9XoicNIbJcwdwch9aGv-967gky--8X2sGD66sBkIojSv9f7Vo30adj-xE82lLJWvG1PRQqi0x-2e4Vo_tpM1QjhEwpxtJ6Bgywx3GohyORVmr7WzttfKs30NLAXt9rzO6-A","NewDeviceMetadata":null,"RefreshToken":"eyJjdHkiOiJKV1QiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiUlNBLU9BRVAifQ.gPHF0r9iuKU-6mignsS2sBgXSfxsSVtGIaOlQ_ZOASfk0Ih5tOOLG2Jm7o7MPH0dQaGrT5Nuo-M9teNU3zYN8FJ1jtv1yugtvRu9d-cTobpG-fFq9Mbh_cyz2iLkblUE6pnFhP8j6SdLG-LKhWFr-uITOYSwH6SsgYCqZ2tZQy5NOwL7et7_wiEEu3_aOyHNrI8zKQz7R92ihs5pA2FxvXQXhwZkOBA5p8HQyDBsWuRlguvdk7bt4FhuYe3v1giZMKE0s3IIH2nA-Mcv74LxfhF61r1xMQHgM9r-e9ws-OLD0AwRO4jTLwXun0cd84CBtzzVpvbD9QxWOUgaIathQg.GjxB2tVihDPYfGbK.ygoiT7iNEk2IRcEY1xdryaJdYyluqG4rr7GhYzbLsD37B1EYoXY6iNfdG6fW8c-lriDcZRW18Zrv5T4fbyYUZoz8bceatLmpaNW7YjsLkSAelUD2oknJ4Qi4TJazAWl939McQyDxf3gb__lHnDAX_nCcFyf95jGJduROJBrSzjCVE8XTcGwAdpKbht4w3fTYXxzlw6rAuk2G8it6KxN-anm9BstAYFtovevg18f4Ag9D1zjhXzAcUCFvNkHKDzq48EZU_Ir44_GSDHpoa5_7n1TAHmiT3zHtFdo6mPZ1Eb60TSCyc3mZ-4XuSK6XioTM-5Ko2WvLnuFSX3aq6MpLEx7RXZZT4OatdNOxoMrD9CgBtvilHLr2ZU2fK25QzF-ieV9NgrMnEUh6ltDf5hEmH__bSHhMz9u4fhggaxnDRvZ0WneGsCvUIHRqxnQeWjPICG-HKeIoNIEvoKgqkbPpmstuOHCcFCcqFl-4uvt3QfyygYjN5Yd-slsKPv209BvSdmPv-y6P72SlS6mkr9MyMQQsDm1v61_QXg8BZdV_o5hlHT_d39jPxwP5yQFLcJS10AnPQFq7ptxv2oxUNaOMk587_Kz4su1y7vGZIlBfIWW3iiVRtnH61ziIenMi_3nemthUJl7xK_YQhi1azFnFCVpxYxWwbL-mkxW_To3myF0NkH2p_WExnR0-j-5QSWFnjKGi8xVjCxXNOcBkxSKzNBe8ez72Mq8ogr60BHcDecjWD5yM2741QPkIjOBIY4v8f7QNS7sFNH-IPqW6MGcuUmfKQETMedzFLCGlco6UV6PK-X8mc8hAdMMdKM6uBe65Gub4JDEGnie8VijZpDa7J4sXj51kfCveGWW0o291utmDsJlXm4XjXdWkf0_HQ_eCeH1MUAmbqKc5j-sSPZM3IHMWuHVGWt9KVK8_HjB9dWNYKfR3dInBl9wcCpl6HjTZZymPmEKQHTJB29_AxyTT0XfT4f0nrk9Wvu4Zlk0m49ors_-uHLGoTDdGHF7M_wiagBt6qZPEBadJZDMWw0xl6Y0yByetfdDvvV81U2ZXfQkSDFs9kPoQ0A1sAANGn6GyLcBXowTtrgy3Zt8gIEtRZh_9gYLWqIvtFYHguWXqmfTV7yS6TIKudJrHjeipzHJVmN55Scc2KRlemizxBDiQb8BgRHaZn7VCqGgJtOf8MYifYc7gKqp_Ur2wRg_-2E1UqpqJFHfMFHnKdDYSWovendjC0IKhNzTp3thiMYbQ3apiBZfNSl1WMaCZJLejtgj4hN1TY-lcN4wW4XpEHwtldg.klc3-r17vYVjCQgmhYr08A","TokenType":"Bearer"}}');
    const wrapper = shallow(<CTDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidMount');
    //update the instance with the new spy
    wrapper.instance().componentDidMount();
    //invoke componentDidMount
    expect(spy).toBeCalled();
  });

  it('should call updateModalState', () => {    
    const wrapper = shallow(<CTDashboard modal={false} userDetails={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'updateModalState');
    //update the instance with the new spy
    wrapper.instance().updateModalState("testModal");
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call async getRoles', () => {
    const wrapper = shallow(<CTDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'getRoles');
    //update the instance with the new spy
    wrapper.instance().getRoles([], []);
    //invoke getRoles
    expect(spy).toBeCalled();
  });

  it('should call async getFilterOptions', () => {
    sessionStorage.setItem("contractorManagementDetails", '{"Company":{"Name":"Consolidated Edison Company of New York Inc.","Logo":"https://onboard-lms-private.s3.amazonaws.com/files/logos/1c0bd30e-b47b-4fe6-8478-863bf52a7b33.png?AWSAccessKeyId=AKIAIS7GG7JYOOR2HEDA&Expires=1557305616&Signature=4O9uaXU5W5e4UfPzMD5I4tJ2ivk%3D","Id":2288},"Menu":{"Learn":{"Title":"Learn","Url":"/learnLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_learn.png?v=2","Items":[{"Title":"My Assignments","Url":"/MyAssignments.aspx"},{"Title":"My Transcript","Url":"/transcript.aspx"},{"Title":"My Downloads","Url":"/MyDownloads.aspx"},{"Title":"Purchase","Url":"/CourseLibrary.aspx"},{"Title":"Find Proctors","Url":"/ProctorSearch.aspx"}]},"Manage":{"Title":"Manage","Url":"/manageLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_manage.png?v=2","Items":[{"Title":"Course Assignments","Url":"/CourseAssignmentLanding.aspx"},{"Title":"Users","Url":"/users.aspx#u=my"},{"Title":"Company","Url":"/Company.aspx"},{"Title":"Tasks","Url":"/Tasks.aspx"},{"Title":"Work Locations","Url":"/CompanySites.aspx"},{"Title":"Job Assignment Check","Url":"/JobsLanding.aspx"},{"Title":"Workbook","Url":"/WorkbooksLanding.aspx"}]},"Reports":{"Title":"Reports","Url":"/reportsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_reports.png?v=2","Items":[{"Title":"Qualifications","Url":"/TranscriptLanding.aspx"},{"Title":"Course Usage Reports","Url":"/CourseUsageLanding.aspx"},{"Title":"Exception Reports","Url":"/ExceptionReportsLanding.aspx"},{"Title":"Custom Reports","Url":"/MyReports.aspx"},{"Title":"Dashboard","Url":"/DashBoard.aspx"},{"Title":"Coaching Reports","Url":"/CoachingReportsLanding.aspx"}]},"Settings":{"Title":"Settings","Url":"/settingsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_settings.png?v=2","Items":[{"Title":"My Profile","Url":"/UserProfile.aspx#u=m"},{"Title":"Change Password","Url":"/UserProfile.aspx#change=pass"},{"Title":"Change Username","Url":"/UserProfile.aspx#change=username"}]}},"User":{"FullName":"James Lucente","Id":331535,"Url":"/userProfile.aspx?u=331535"}}');
    sessionStorage.setItem("userPhoto", 'userPhoto');
    const wrapper = shallow(<CTDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'getFilterOptions');
    //update the instance with the new spy
    wrapper.instance().getFilterOptions();
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call async getCompanyFilterOptions', () => {
    sessionStorage.setItem("contractorManagementDetails", '{"Company":{"Name":"Consolidated Edison Company of New York Inc.","Logo":"https://onboard-lms-private.s3.amazonaws.com/files/logos/1c0bd30e-b47b-4fe6-8478-863bf52a7b33.png?AWSAccessKeyId=AKIAIS7GG7JYOOR2HEDA&Expires=1557305616&Signature=4O9uaXU5W5e4UfPzMD5I4tJ2ivk%3D","Id":2288},"Menu":{"Learn":{"Title":"Learn","Url":"/learnLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_learn.png?v=2","Items":[{"Title":"My Assignments","Url":"/MyAssignments.aspx"},{"Title":"My Transcript","Url":"/transcript.aspx"},{"Title":"My Downloads","Url":"/MyDownloads.aspx"},{"Title":"Purchase","Url":"/CourseLibrary.aspx"},{"Title":"Find Proctors","Url":"/ProctorSearch.aspx"}]},"Manage":{"Title":"Manage","Url":"/manageLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_manage.png?v=2","Items":[{"Title":"Course Assignments","Url":"/CourseAssignmentLanding.aspx"},{"Title":"Users","Url":"/users.aspx#u=my"},{"Title":"Company","Url":"/Company.aspx"},{"Title":"Tasks","Url":"/Tasks.aspx"},{"Title":"Work Locations","Url":"/CompanySites.aspx"},{"Title":"Job Assignment Check","Url":"/JobsLanding.aspx"},{"Title":"Workbook","Url":"/WorkbooksLanding.aspx"}]},"Reports":{"Title":"Reports","Url":"/reportsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_reports.png?v=2","Items":[{"Title":"Qualifications","Url":"/TranscriptLanding.aspx"},{"Title":"Course Usage Reports","Url":"/CourseUsageLanding.aspx"},{"Title":"Exception Reports","Url":"/ExceptionReportsLanding.aspx"},{"Title":"Custom Reports","Url":"/MyReports.aspx"},{"Title":"Dashboard","Url":"/DashBoard.aspx"},{"Title":"Coaching Reports","Url":"/CoachingReportsLanding.aspx"}]},"Settings":{"Title":"Settings","Url":"/settingsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_settings.png?v=2","Items":[{"Title":"My Profile","Url":"/UserProfile.aspx#u=m"},{"Title":"Change Password","Url":"/UserProfile.aspx#change=pass"},{"Title":"Change Username","Url":"/UserProfile.aspx#change=username"}]}},"User":{"FullName":"James Lucente","Id":331535,"Url":"/userProfile.aspx?u=331535"}}');
    sessionStorage.setItem("userPhoto", 'userPhoto');
    const wrapper = shallow(<CTDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'getCompanyFilterOptions');
    //update the instance with the new spy
    wrapper.instance().getCompanyFilterOptions();
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call async getCompanyDetails', () => {
    const wrapper = shallow(<CTDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'getCompanyDetails');
    //update the instance with the new spy
    wrapper.instance().getCompanyDetails("", 1, true);
    //invoke getCompanyDetails
    expect(spy).toBeCalled();
  });

  it('should call createRows', () => {
    let companyTasks = [{ Role: "", RoleId: 0, CompletedRoleQualification: "", InCompletedRoleQualification: ""}]
    const wrapper = shallow(<CTDashboard modal={false} userDetails={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'createRows');
    //update the instance with the new spy
    wrapper.instance().createRows(companyTasks);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call handleCellClick', () => {
    let args = {
      roleId: 1,
      role: ""
    };
    const wrapper = shallow(<CTDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'handleCellClick');
    //update the instance with the new spy
    wrapper.instance().handleCellClick("completedCompanies", args);
    wrapper.instance().handleCellClick("incompleteCompanies", args);
    wrapper.instance().handleCellClick("default", args);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call toggle', () => {
    const wrapper = shallow(<CTDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'toggle');
    //update the instance with the new spy
    wrapper.instance().toggle();
    //invoke toggleFilter
    expect(spy).toBeCalled();
  });

  it('should call toggleFilter', () => {
    const wrapper = shallow(<CTDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'toggleFilter');
    //update the instance with the new spy
    wrapper.instance().toggleFilter();
    //invoke toggleFilter
    expect(spy).toBeCalled();
  });

  it('should call toggleCompanyFilter', () => {
    const wrapper = shallow(<CTDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'toggleCompanyFilter');
    //update the instance with the new spy
    wrapper.instance().toggleCompanyFilter();
    //invoke toggleCompanyFilter
    expect(spy).toBeCalled();
  });
  
  it('should call updateSelectedData', () => {
    const wrapper = shallow(<CTDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'updateSelectedData');
    //update the instance with the new spy
    wrapper.instance().updateSelectedData([]);
    //invoke updateSelectedData
    expect(spy).toBeCalled();
  });

  it('should call updateCompanySelectedData', () => {
    const wrapper = shallow(<CTDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'updateCompanySelectedData');
    //update the instance with the new spy
    wrapper.instance().updateCompanySelectedData([]);
    //invoke updateCompanySelectedData
    expect(spy).toBeCalled();
  });

  it('should call handleRoleDelete', () => {
    const wrapper = shallow(<CTDashboard modal={false} filteredRoles={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'handleRoleDelete');
    //update the instance with the new spy
    wrapper.instance().handleRoleDelete(1);
    //invoke render
    expect(spy).toBeCalled();
  });

  it('should call handleCompanyDelete', () => {
    const wrapper = shallow(<CTDashboard modal={false} filteredRoles={[]} title={""} />);
    const spy = jest.spyOn(wrapper.instance(), 'handleCompanyDelete');
    //update the instance with the new spy
    wrapper.instance().handleCompanyDelete(1);
    //invoke render
    expect(spy).toBeCalled();
  });


  it('should call filterGoAction', () => {
    sessionStorage.setItem("contractorManagementDetails", '{"Company":{"Name":"Consolidated Edison Company of New York Inc.","Logo":"https://onboard-lms-private.s3.amazonaws.com/files/logos/1c0bd30e-b47b-4fe6-8478-863bf52a7b33.png?AWSAccessKeyId=AKIAIS7GG7JYOOR2HEDA&Expires=1557305616&Signature=4O9uaXU5W5e4UfPzMD5I4tJ2ivk%3D","Id":2288},"Menu":{"Learn":{"Title":"Learn","Url":"/learnLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_learn.png?v=2","Items":[{"Title":"My Assignments","Url":"/MyAssignments.aspx"},{"Title":"My Transcript","Url":"/transcript.aspx"},{"Title":"My Downloads","Url":"/MyDownloads.aspx"},{"Title":"Purchase","Url":"/CourseLibrary.aspx"},{"Title":"Find Proctors","Url":"/ProctorSearch.aspx"}]},"Manage":{"Title":"Manage","Url":"/manageLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_manage.png?v=2","Items":[{"Title":"Course Assignments","Url":"/CourseAssignmentLanding.aspx"},{"Title":"Users","Url":"/users.aspx#u=my"},{"Title":"Company","Url":"/Company.aspx"},{"Title":"Tasks","Url":"/Tasks.aspx"},{"Title":"Work Locations","Url":"/CompanySites.aspx"},{"Title":"Job Assignment Check","Url":"/JobsLanding.aspx"},{"Title":"Workbook","Url":"/WorkbooksLanding.aspx"}]},"Reports":{"Title":"Reports","Url":"/reportsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_reports.png?v=2","Items":[{"Title":"Qualifications","Url":"/TranscriptLanding.aspx"},{"Title":"Course Usage Reports","Url":"/CourseUsageLanding.aspx"},{"Title":"Exception Reports","Url":"/ExceptionReportsLanding.aspx"},{"Title":"Custom Reports","Url":"/MyReports.aspx"},{"Title":"Dashboard","Url":"/DashBoard.aspx"},{"Title":"Coaching Reports","Url":"/CoachingReportsLanding.aspx"}]},"Settings":{"Title":"Settings","Url":"/settingsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_settings.png?v=2","Items":[{"Title":"My Profile","Url":"/UserProfile.aspx#u=m"},{"Title":"Change Password","Url":"/UserProfile.aspx#change=pass"},{"Title":"Change Username","Url":"/UserProfile.aspx#change=username"}]}},"User":{"FullName":"James Lucente","Id":331535,"Url":"/userProfile.aspx?u=331535"}}');
    const wrapper = shallow(<CTDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'filterGoAction');
    //update the instance with the new spy
    wrapper.instance().filterGoAction();
    //invoke filterGoAction
    expect(spy).toBeCalled();
  });

  it('should call customCell', () => {
    const wrapper = shallow(<CTDashboard modal={false} employee={[]} />);
    const spy = jest.spyOn(wrapper.instance(), 'customCell');
    //update the instance with the new spy
    wrapper.instance().customCell({ value: "" });
    //invoke customCell
    expect(spy).toBeCalled();
  });

  it('should call autoLogout', () => {
    const wrapper = shallow(<CTDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'autoLogout');
    //update the instance with the new spy
    wrapper.instance().autoLogout();
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });

  it('should call render', () => {
    const wrapper = shallow(<CTDashboard modal={false} userDetails={[]} title={""} />);

    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });

});
