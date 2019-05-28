import React from "react";
import OQDashboard from '../OQDashboard.jsx';
import { shallow, mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("OQDashboard component", () => {

  beforeEach(() => {
    sessionStorage.setItem("dashboardAPIToken", '{"dashboardAPIToken":{"AccessToken":"eyJraWQiOiJ1MUpCRFZ5eGJ0T1MzcFBSTm9JdWJjYUs5ZjJYQ2dlOXJyTXJWQjEzc0J3PSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiJkZWE2YTEwZi04OTk0LTRhNDYtOGE2MC0wMjk4M2Q2M2I1MGEiLCJldmVudF9pZCI6IjRjZjEzYTMxLTc1N2QtMTFlOS04YjhkLTkxYmIwMmE4M2RmYSIsInRva2VuX3VzZSI6ImFjY2VzcyIsInNjb3BlIjoiYXdzLmNvZ25pdG8uc2lnbmluLnVzZXIuYWRtaW4iLCJhdXRoX3RpbWUiOjE1NTc3NTE2NjMsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy13ZXN0LTIuYW1hem9uYXdzLmNvbVwvdXMtd2VzdC0yX0Y1NGo2bTNUMiIsImV4cCI6MTU1Nzc1NTI2MywiaWF0IjoxNTU3NzUxNjYzLCJqdGkiOiJkMDJiNjg2Ny05OTdjLTRhMzAtYjFkNC0wYTBhYjAzMzdiYzgiLCJjbGllbnRfaWQiOiI1aHU0OHJkM21iZnZyM3Q2ZDZyMnJjaGFwbSIsInVzZXJuYW1lIjoibHVjZW50ZWpfMzMxNTM1In0.Ac9Yq0J1R43eYH8V49K4kFr6U468KuYtdqqdfbqwekm2PncpOk4V5EYAn_AbTsMEf1HllTBzQbOmRJpT6hdK7b2UIhHmDj-SVP3B3q5-OHnxRNnAf0cjwlPByx0FaMbK4JXVcvyATcNEW7cX9FDU52q9QsJ6e8RsYiIsbQ0YJHo8QKTmwxcbARzwzmYuw3DwY8bWftT-l2omvaPPK4ZfnBzsDf8Xa3a7KzW7BfIUQKhhlYlCX5LxAYebJBS-oUdqpdObJ8cZ6dRSwdep-E6LtlQqv95cKvrjev6HjGLRyuTO7gzrDVO4I7AP0m6g4dVhrQtf6385Ihq2bN9CJ1j8Sg","ExpiresIn":3600,"IdToken":"eyJraWQiOiJ5REQ0ZjA5bm9ScndSc2hKMXM3OEFhUW0wUHVUZjVBUFwvQ3A2SkFjS0V4RT0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJkZWE2YTEwZi04OTk0LTRhNDYtOGE2MC0wMjk4M2Q2M2I1MGEiLCJhdWQiOiI1aHU0OHJkM21iZnZyM3Q2ZDZyMnJjaGFwbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJldmVudF9pZCI6IjRjZjEzYTMxLTc1N2QtMTFlOS04YjhkLTkxYmIwMmE4M2RmYSIsInRva2VuX3VzZSI6ImlkIiwiYXV0aF90aW1lIjoxNTU3NzUxNjYzLCJpc3MiOiJodHRwczpcL1wvY29nbml0by1pZHAudXMtd2VzdC0yLmFtYXpvbmF3cy5jb21cL3VzLXdlc3QtMl9GNTRqNm0zVDIiLCJjdXN0b206dXNlcl9pZCI6IjMzMTUzNSIsImNvZ25pdG86dXNlcm5hbWUiOiJsdWNlbnRlal8zMzE1MzUiLCJleHAiOjE1NTc3NTUyNjMsImlhdCI6MTU1Nzc1MTY2MywiZW1haWwiOiJuby1yZXBseUBpdHMtdHJhaW5pbmcuY29tIn0.D7pyWRXx9AHN1LYk8I0mQeyn3_AbPpK6r53ujcA_Bwy-lF3bue6CLrGSWteWf1nE1ddaPju212skbypn0SeMzPrMwoF6rBua5cFZ2pgcKyo7HNeckjHeyfjdtmKZnmJb_epSLD2m0s7wo_W0dYMbzvPIXKxna9fa49kgYn2BZbNEkExF_uOcfa7hLiY_UHVSL2se-QrAK5RToOOXFWx9XoicNIbJcwdwch9aGv-967gky--8X2sGD66sBkIojSv9f7Vo30adj-xE82lLJWvG1PRQqi0x-2e4Vo_tpM1QjhEwpxtJ6Bgywx3GohyORVmr7WzttfKs30NLAXt9rzO6-A","NewDeviceMetadata":null,"RefreshToken":"eyJjdHkiOiJKV1QiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiUlNBLU9BRVAifQ.gPHF0r9iuKU-6mignsS2sBgXSfxsSVtGIaOlQ_ZOASfk0Ih5tOOLG2Jm7o7MPH0dQaGrT5Nuo-M9teNU3zYN8FJ1jtv1yugtvRu9d-cTobpG-fFq9Mbh_cyz2iLkblUE6pnFhP8j6SdLG-LKhWFr-uITOYSwH6SsgYCqZ2tZQy5NOwL7et7_wiEEu3_aOyHNrI8zKQz7R92ihs5pA2FxvXQXhwZkOBA5p8HQyDBsWuRlguvdk7bt4FhuYe3v1giZMKE0s3IIH2nA-Mcv74LxfhF61r1xMQHgM9r-e9ws-OLD0AwRO4jTLwXun0cd84CBtzzVpvbD9QxWOUgaIathQg.GjxB2tVihDPYfGbK.ygoiT7iNEk2IRcEY1xdryaJdYyluqG4rr7GhYzbLsD37B1EYoXY6iNfdG6fW8c-lriDcZRW18Zrv5T4fbyYUZoz8bceatLmpaNW7YjsLkSAelUD2oknJ4Qi4TJazAWl939McQyDxf3gb__lHnDAX_nCcFyf95jGJduROJBrSzjCVE8XTcGwAdpKbht4w3fTYXxzlw6rAuk2G8it6KxN-anm9BstAYFtovevg18f4Ag9D1zjhXzAcUCFvNkHKDzq48EZU_Ir44_GSDHpoa5_7n1TAHmiT3zHtFdo6mPZ1Eb60TSCyc3mZ-4XuSK6XioTM-5Ko2WvLnuFSX3aq6MpLEx7RXZZT4OatdNOxoMrD9CgBtvilHLr2ZU2fK25QzF-ieV9NgrMnEUh6ltDf5hEmH__bSHhMz9u4fhggaxnDRvZ0WneGsCvUIHRqxnQeWjPICG-HKeIoNIEvoKgqkbPpmstuOHCcFCcqFl-4uvt3QfyygYjN5Yd-slsKPv209BvSdmPv-y6P72SlS6mkr9MyMQQsDm1v61_QXg8BZdV_o5hlHT_d39jPxwP5yQFLcJS10AnPQFq7ptxv2oxUNaOMk587_Kz4su1y7vGZIlBfIWW3iiVRtnH61ziIenMi_3nemthUJl7xK_YQhi1azFnFCVpxYxWwbL-mkxW_To3myF0NkH2p_WExnR0-j-5QSWFnjKGi8xVjCxXNOcBkxSKzNBe8ez72Mq8ogr60BHcDecjWD5yM2741QPkIjOBIY4v8f7QNS7sFNH-IPqW6MGcuUmfKQETMedzFLCGlco6UV6PK-X8mc8hAdMMdKM6uBe65Gub4JDEGnie8VijZpDa7J4sXj51kfCveGWW0o291utmDsJlXm4XjXdWkf0_HQ_eCeH1MUAmbqKc5j-sSPZM3IHMWuHVGWt9KVK8_HjB9dWNYKfR3dInBl9wcCpl6HjTZZymPmEKQHTJB29_AxyTT0XfT4f0nrk9Wvu4Zlk0m49ors_-uHLGoTDdGHF7M_wiagBt6qZPEBadJZDMWw0xl6Y0yByetfdDvvV81U2ZXfQkSDFs9kPoQ0A1sAANGn6GyLcBXowTtrgy3Zt8gIEtRZh_9gYLWqIvtFYHguWXqmfTV7yS6TIKudJrHjeipzHJVmN55Scc2KRlemizxBDiQb8BgRHaZn7VCqGgJtOf8MYifYc7gKqp_Ur2wRg_-2E1UqpqJFHfMFHnKdDYSWovendjC0IKhNzTp3thiMYbQ3apiBZfNSl1WMaCZJLejtgj4hN1TY-lcN4wW4XpEHwtldg.klc3-r17vYVjCQgmhYr08A","TokenType":"Bearer"}}');
    sessionStorage.setItem("contractorManagementDetails", '{"Company":{"Name":"Consolidated Edison Company of New York Inc.","Logo":"https://onboard-lms-private.s3.amazonaws.com/files/logos/1c0bd30e-b47b-4fe6-8478-863bf52a7b33.png?AWSAccessKeyId=AKIAIS7GG7JYOOR2HEDA&Expires=1557305616&Signature=4O9uaXU5W5e4UfPzMD5I4tJ2ivk%3D","Id":2288},"Menu":{"Learn":{"Title":"Learn","Url":"/learnLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_learn.png?v=2","Items":[{"Title":"My Assignments","Url":"/MyAssignments.aspx"},{"Title":"My Transcript","Url":"/transcript.aspx"},{"Title":"My Downloads","Url":"/MyDownloads.aspx"},{"Title":"Purchase","Url":"/CourseLibrary.aspx"},{"Title":"Find Proctors","Url":"/ProctorSearch.aspx"}]},"Manage":{"Title":"Manage","Url":"/manageLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_manage.png?v=2","Items":[{"Title":"Course Assignments","Url":"/CourseAssignmentLanding.aspx"},{"Title":"Users","Url":"/users.aspx#u=my"},{"Title":"Company","Url":"/Company.aspx"},{"Title":"Tasks","Url":"/Tasks.aspx"},{"Title":"Work Locations","Url":"/CompanySites.aspx"},{"Title":"Job Assignment Check","Url":"/JobsLanding.aspx"},{"Title":"Workbook","Url":"/WorkbooksLanding.aspx"}]},"Reports":{"Title":"Reports","Url":"/reportsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_reports.png?v=2","Items":[{"Title":"Qualifications","Url":"/TranscriptLanding.aspx"},{"Title":"Course Usage Reports","Url":"/CourseUsageLanding.aspx"},{"Title":"Exception Reports","Url":"/ExceptionReportsLanding.aspx"},{"Title":"Custom Reports","Url":"/MyReports.aspx"},{"Title":"Dashboard","Url":"/DashBoard.aspx"},{"Title":"Coaching Reports","Url":"/CoachingReportsLanding.aspx"}]},"Settings":{"Title":"Settings","Url":"/settingsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_settings.png?v=2","Items":[{"Title":"My Profile","Url":"/UserProfile.aspx#u=m"},{"Title":"Change Password","Url":"/UserProfile.aspx#change=pass"},{"Title":"Change Username","Url":"/UserProfile.aspx#change=username"}]}},"User":{"FullName":"James Lucente","Id":331535,"Url":"/userProfile.aspx?u=331535"}}');
    sessionStorage.setItem("userPhoto", 'userPhoto');
  });
  
  test("check renders", () => {
    const wrapper = shallow(<OQDashboard />);
    expect(wrapper.exists()).toBe(true);
  });

  it("check state[level] is 0 on mounting", () => {
    const wrapper = shallow(<OQDashboard />);
    expect(wrapper.state('level')).toEqual(0);
  });

  it('should call cellFormatter', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'cellFormatter');
    //update the instance with the new spy
    wrapper.instance().cellFormatter({ value: "" });
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call componentDidCatch', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidCatch');
    //update the instance with the new spy
    wrapper.instance().componentDidCatch("Test Error", "This is error from test case");
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });

  it('should call componentDidMount without session', () => {
    sessionStorage.setItem("dashboardAPIToken", '');
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidMount');
    //update the instance with the new spy
    wrapper.instance().componentDidMount();
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });

  it('should call componentDidMount with session', ()  => {
    sessionStorage.setItem("dashboardAPIToken", '{"dashboardAPIToken":{"AccessToken":"eyJraWQiOiJ1MUpCRFZ5eGJ0T1MzcFBSTm9JdWJjYUs5ZjJYQ2dlOXJyTXJWQjEzc0J3PSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiJkZWE2YTEwZi04OTk0LTRhNDYtOGE2MC0wMjk4M2Q2M2I1MGEiLCJldmVudF9pZCI6IjMxNGZkNmVhLTcxN2MtMTFlOS1hNDJmLTU1OWE5ODk5NTA4YyIsInRva2VuX3VzZSI6ImFjY2VzcyIsInNjb3BlIjoiYXdzLmNvZ25pdG8uc2lnbmluLnVzZXIuYWRtaW4iLCJhdXRoX3RpbWUiOjE1NTczMTEzODMsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy13ZXN0LTIuYW1hem9uYXdzLmNvbVwvdXMtd2VzdC0yX0Y1NGo2bTNUMiIsImV4cCI6MTU1NzMxNDk4MywiaWF0IjoxNTU3MzExMzgzLCJqdGkiOiIyODI3NDM3My1hY2RhLTQ5ODUtOGEwZS1mYTI2NDIyMTQ2ZTIiLCJjbGllbnRfaWQiOiI1aHU0OHJkM21iZnZyM3Q2ZDZyMnJjaGFwbSIsInVzZXJuYW1lIjoibHVjZW50ZWpfMzMxNTM1In0.ZSpi8aZP4Z20G1-700PyywapvpCcFts3l-AhomIX66_w-TR6qzHo6MNdKrKuv9jOxe8D9le_8XBVYMflPq__jaTP4I1iOWmJhkLUWRCW4Zdjc5qZ0jjoGSSNgcUJXzJQ_nDxt7vlL7gbgEiIVmuMLASTr5CikiKJfi_R0hA_-5V78Ayegcz-qhh0MCF-PZBzxx3w1t3dKNX6lnAvISD-DWS485f3rbit5i0X_8QlcUFBjikgMPGb9aV3DIFodGXZjcTk4XnWyu857iaRlEKkoNrVhl8QunhSASuk7kNQZ8LuwvXy9ZBnxC7Px3ZqWfVHBzjMScRDqpiMKlAA3z0uxw","ExpiresIn":3600,"IdToken":"eyJraWQiOiJ5REQ0ZjA5bm9ScndSc2hKMXM3OEFhUW0wUHVUZjVBUFwvQ3A2SkFjS0V4RT0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJkZWE2YTEwZi04OTk0LTRhNDYtOGE2MC0wMjk4M2Q2M2I1MGEiLCJhdWQiOiI1aHU0OHJkM21iZnZyM3Q2ZDZyMnJjaGFwbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJldmVudF9pZCI6IjMxNGZkNmVhLTcxN2MtMTFlOS1hNDJmLTU1OWE5ODk5NTA4YyIsInRva2VuX3VzZSI6ImlkIiwiYXV0aF90aW1lIjoxNTU3MzExMzgzLCJpc3MiOiJodHRwczpcL1wvY29nbml0by1pZHAudXMtd2VzdC0yLmFtYXpvbmF3cy5jb21cL3VzLXdlc3QtMl9GNTRqNm0zVDIiLCJjdXN0b206dXNlcl9pZCI6IjMzMTUzNSIsImNvZ25pdG86dXNlcm5hbWUiOiJsdWNlbnRlal8zMzE1MzUiLCJleHAiOjE1NTczMTQ5ODMsImlhdCI6MTU1NzMxMTM4MywiZW1haWwiOiJuby1yZXBseUBpdHMtdHJhaW5pbmcuY29tIn0.POwQTxkBaet3dJj6pAJzHpS2MYKn3k1ZpMd5hAD0fjTVVF_rrF-RESpkDj3_zNxkekexut42_rzBe2lztaMDjLR_TVr6xsFze0xhgBJuBB5r5Q88347ojtTis4rF2Eup0fSoCilnQACsh6H2ffRckRgqV86-U-GuMZ0JS04xRoGVrXSNhwIlKZ8pHTEqsvsOuR3uEBGpVN2iBhAlP55ERCyUtUOHNC276TfqeKo2Y4b4nH5WQJcqsgpvjMr_swftZjZEMQlx3MnDZkCPM1hg8Gi0TrXvb4-wm7dlVPykzROehxEj6Jz7UGSAj2iI7k2tKCUPyPdbD4ozJCtve_A4gA","NewDeviceMetadata":null,"RefreshToken":"eyJjdHkiOiJKV1QiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiUlNBLU9BRVAifQ.Pbq9opfojBFizJUYJF1GhO2ZZ1tEohY4HmyAozDvS19f34dN5J_7UY5Jqi98l6Rbk30ud9f5eaMbveVqC8gFuylRRcyY6xzgz3xsMO9oxxpXYc5EuoJZPiI3N_7EKm7xBpJnACw6yU8I_hBEXo-P07ZOIxU8CPc87v3dx0fnLXoOlYz7b3Gt_nV9_upAl46CCgNFkRMat3mBd5dCe15B9X3pwhUNxJ-Jcs5k2OZesH2livj3iUxY3Rc00FJzMUHoy05swhOIty2xSfb_8i6U9DgCq4tE7m-uBW1mq6n5DXwHNWoliGRCKxWkSPrWr-whqvhru4ETSwc23-rVrA6u6w.bxg8zNknepSxNDp1.8RdJ-f2I74Kw0gZxTVvLRQpqN6fP_WDm3e8Zw-TmqdTIBEkb9MbP3Bz32KotgSWoaXGBD-4to5QLQotIvuFiifnCXqbrimDlWUzpbEjm1Zjk4bmqW3_pq8Zzvb7_4jQmFPoJdcBzYzwVYV8hr06nJx2PV8WHyyUVMI46Srg0bc_MVgSE71TeLJKDRztACG9QtvyqXC335PtmoHK6Dy8GoDzObupqq8TuoPqi1746GOszwhCV40K6574zhXdcXRiksJ_9xcnLUTYMd1s-25MbUAoyM-Q5vtoiEasOJea_kladN1QN7-lLVPCd9iEJkZuXHrwwFFIj471-g638vU9ZrutJHCsWQDxE4XOHTzbCXiFvOTmUgC5ZFALs2H5gTRy_F3zRzEE3VZtYlAVdSP2j9jvEjZk2NbOIhBb82KL-4Znidjpx-nH6FSfQeAPd_sigZ4GHyaoVyjYPLX6bQ3KjB5iB_E3ZX3feLIRhnmNjGmoLTa1OUFkLLwpEJOYXx3PQdPTRr2NBSCON056x378dbS8xJhxEJYAvu7NmPi5OP0ga58pFNTOBZmaWA9fMfnKrepz4dUJWFypnUriv1s7dCEYFwKm2-1UyWW7yG-69L8xi2Sk58ejenTaJaF2tmYWqPkOw8VG5D0gUJeT5kClVp4ATb6-WpsyirTddX72M6oy5EYc31NyhsRvPz05zDh6kO9PAP0MmOq-kpsQOvy5zeWXT7ZEkuBPtunR7hBA3sY8aNmI5AGy4inWnx1zjInAEnvwF-wAgr3_R4x_NU0GsHgBWtbMM0c0OOZ2h_0or7Me-lga5MQw87q8qC0DHZIMVG9ciKEyMMZAPARyv7FpARAvttNF-KBMKYamd--ACnhfWS7G1a8-BeCyafzz20gLZELodrc8kP32-Ku1nG67CdZY6q507uwbSQtYxFYCYxPDjlPAO34a_baw-uAGifyxFwV9ywBO9_W-c_WbA4JvicA9kMUsGSzJVl3Hcznjm2NwvNyb3wSCHPnMolTcUVSrjGU54lpMFbj0EPiC6JlEl7KxaDl7o1IFucTkjXINH_ZzTu8yIjjS5zg2GsZVrTeuoRoP8jMYeM-YknNSz4g_qOE8Vy9nzceJfLbZ5RJpYb6xIkVQ9sR2evbGj2D2Wo1TNwrba_FZ6GYQAnYxrWlLJhniJjUFddjcs2OH7iHPjM-9ZhYlOWwX6m9c_XbsnuZvUgVkgl597IWjQ7gSPvJdh6C6gC7nAAcWqE81_03DVtAaOYfEqXNJ4TJoylHdpF2KXN9aw3UZxPU_le7de4mBWcg.kn-dkouXpqhKRxf106hDnQ","TokenType":"Bearer"}}');
    const wrapper = mount(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidMount');
    //update the instance with the new spy
    wrapper.instance().componentDidMount();
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });

  it('should call createRows', () => {
    let qualifications = [
      {
        TaskCode: "",
        TaskName: "",
        EmployeeName: "",
        AssignedDate: "",
      }
    ]
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'createRows');
    //update the instance with the new spy
    wrapper.instance().createRows(qualifications);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call qualificationsFormatter with valid props', () => {
    let props = {
      original: {
        "test": 1,
        "company": "Test Name"
      }
    }

    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'qualificationsFormatter');
    //update the instance with the new spy
    wrapper.instance().qualificationsFormatter("test", props);
    //invoke createRows
    expect(spy).toBeCalled();
  });


  it('should call qualificationsFormatter with invalid props', () => {
    let props = {
      original: {
        "test": 1,
        "company": "Total"
      }
    }

    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'qualificationsFormatter');
    //update the instance with the new spy
    wrapper.instance().qualificationsFormatter("test", props);
    //invoke createRows
    expect(spy).toBeCalled();
  });


  it('should call updateModalState', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'updateModalState');
    //update the instance with the new spy
    wrapper.instance().updateModalState("Test Modal");
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call handleCellClick', () => {
    let args = {
      userId: 1,
      companyId: 2,
      company: "Test Company"
    };
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'handleCellClick');
    //update the instance with the new spy
    wrapper.instance().handleCellClick("company", args);
    wrapper.instance().handleCellClick("total", args);
    wrapper.instance().handleCellClick("assignedQualification", args);
    wrapper.instance().handleCellClick("completedQualification", args);
    wrapper.instance().handleCellClick("inCompletedQualification", args);
    wrapper.instance().handleCellClick("pastDue", args);
    wrapper.instance().handleCellClick("comingDue", args);
    wrapper.instance().handleCellClick("lockoutCount", args);
    wrapper.instance().handleCellClick("default", args);
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call async getFilterOptions', ()  => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'getFilterOptions');
    //update the instance with the new spy
    wrapper.instance().getFilterOptions();
    //invoke createRows
    expect(spy).toBeCalled();
  });

  it('should call async getQualifications', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'getQualifications');
    //update the instance with the new spy
    wrapper.instance().getQualifications(1, [1, 2 ,3]);
    //invoke getQualifications
    expect(spy).toBeCalled();
  });

  it('should call async getAssignedQualifications', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'getAssignedQualifications');
    //update the instance with the new spy
    wrapper.instance().getAssignedQualifications(1, 2);
    //invoke getAssignedQualifications
    expect(spy).toBeCalled();
  });

  it('should call async getCompletedQualifications', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'getCompletedQualifications');
    //update the instance with the new spy
    wrapper.instance().getCompletedQualifications(1, 2);
    //invoke getCompletedQualifications
    expect(spy).toBeCalled();
  });

  it('should call async getLockedOutQualifications', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'getLockedOutQualifications');
    //update the instance with the new spy
    wrapper.instance().getLockedOutQualifications(1, 2);
    //invoke getLockedOutQualifications
    expect(spy).toBeCalled();
  });


  it('should call async getInCompletedQualifications', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'getInCompletedQualifications');
    //update the instance with the new spy
    wrapper.instance().getInCompletedQualifications(1, 2);
    //invoke getInCompletedQualifications
    expect(spy).toBeCalled();
  });

  it('should call async getSuspendedQualifications', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'getSuspendedQualifications');
    //update the instance with the new spy
    wrapper.instance().getSuspendedQualifications(1, 2);
    //invoke getSuspendedQualifications
    expect(spy).toBeCalled();
  });

  it('should call async getPastDueQualifications', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'getPastDueQualifications');
    //update the instance with the new spy
    wrapper.instance().getPastDueQualifications(1, 2);
    //invoke getPastDueQualifications
    expect(spy).toBeCalled();
  });

  it('should call async getComingDueQualifications', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'getComingDueQualifications');
    //update the instance with the new spy
    wrapper.instance().getComingDueQualifications(1, 2);
    //invoke getComingDueQualifications
    expect(spy).toBeCalled();
  });

  it('should call async getEmployeeQualifications', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'getEmployeeQualifications');
    //update the instance with the new spy
    wrapper.instance().getEmployeeQualifications(1, 2);
    //invoke getEmployeeQualifications
    expect(spy).toBeCalled();
  });

  it('should call updateEmployeesQualificationsArray', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'updateEmployeesQualificationsArray');
    //update the instance with the new spy
    wrapper.instance().updateEmployeesQualificationsArray([], []);
    //invoke updateEmployeesQualificationsArray
    expect(spy).toBeCalled();
  });

  it('should call popEmployeesQualificationsArray', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'popEmployeesQualificationsArray');
    //update the instance with the new spy
    wrapper.instance().popEmployeesQualificationsArray();
    //invoke popEmployeesQualificationsArray
    expect(spy).toBeCalled();
  });

  it('should call toggle', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'toggle');
    //update the instance with the new spy
    wrapper.instance().toggle();
    //invoke toggle
    expect(spy).toBeCalled();
  });

  it('should call toggleFilter', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'toggleFilter');
    //update the instance with the new spy
    wrapper.instance().toggleFilter();
    //invoke toggleFilter
    expect(spy).toBeCalled();
  });

  it('should call updateSelectedData', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'updateSelectedData');
    //update the instance with the new spy
    wrapper.instance().updateSelectedData([]);
    //invoke updateSelectedData
    expect(spy).toBeCalled();
  });

  it('should call handleRoleDelete', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'handleRoleDelete');
    //update the instance with the new spy
    wrapper.instance().handleRoleDelete(1);
    //invoke handleRoleDelete
    expect(spy).toBeCalled();
  });


  it('should call filterGoAction', () => {
    sessionStorage.setItem("contractorManagementDetails", '{"Company":{"Name":"Consolidated Edison Company of New York Inc.","Logo":"https://onboard-lms-private.s3.amazonaws.com/files/logos/1c0bd30e-b47b-4fe6-8478-863bf52a7b33.png?AWSAccessKeyId=AKIAIS7GG7JYOOR2HEDA&Expires=1557305616&Signature=4O9uaXU5W5e4UfPzMD5I4tJ2ivk%3D","Id":2288},"Menu":{"Learn":{"Title":"Learn","Url":"/learnLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_learn.png?v=2","Items":[{"Title":"My Assignments","Url":"/MyAssignments.aspx"},{"Title":"My Transcript","Url":"/transcript.aspx"},{"Title":"My Downloads","Url":"/MyDownloads.aspx"},{"Title":"Purchase","Url":"/CourseLibrary.aspx"},{"Title":"Find Proctors","Url":"/ProctorSearch.aspx"}]},"Manage":{"Title":"Manage","Url":"/manageLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_manage.png?v=2","Items":[{"Title":"Course Assignments","Url":"/CourseAssignmentLanding.aspx"},{"Title":"Users","Url":"/users.aspx#u=my"},{"Title":"Company","Url":"/Company.aspx"},{"Title":"Tasks","Url":"/Tasks.aspx"},{"Title":"Work Locations","Url":"/CompanySites.aspx"},{"Title":"Job Assignment Check","Url":"/JobsLanding.aspx"},{"Title":"Workbook","Url":"/WorkbooksLanding.aspx"}]},"Reports":{"Title":"Reports","Url":"/reportsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_reports.png?v=2","Items":[{"Title":"Qualifications","Url":"/TranscriptLanding.aspx"},{"Title":"Course Usage Reports","Url":"/CourseUsageLanding.aspx"},{"Title":"Exception Reports","Url":"/ExceptionReportsLanding.aspx"},{"Title":"Custom Reports","Url":"/MyReports.aspx"},{"Title":"Dashboard","Url":"/DashBoard.aspx"},{"Title":"Coaching Reports","Url":"/CoachingReportsLanding.aspx"}]},"Settings":{"Title":"Settings","Url":"/settingsLanding.aspx","Icon":"https://d2vkqsz7y0fh3j.cloudfront.net/img/body_settings.png?v=2","Items":[{"Title":"My Profile","Url":"/UserProfile.aspx#u=m"},{"Title":"Change Password","Url":"/UserProfile.aspx#change=pass"},{"Title":"Change Username","Url":"/UserProfile.aspx#change=username"}]}},"User":{"FullName":"James Lucente","Id":331535,"Url":"/userProfile.aspx?u=331535"}}');
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'filterGoAction');
    //update the instance with the new spy
    wrapper.instance().filterGoAction();
    //invoke filterGoAction
    expect(spy).toBeCalled();
  });


  it('should call customCell', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'customCell');
    //update the instance with the new spy
    wrapper.instance().customCell({ value: "" });
    //invoke customCell
    expect(spy).toBeCalled();
  });

  it('should call autoLogout', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'autoLogout');
    //update the instance with the new spy
    wrapper.instance().autoLogout();
    //invoke autoLogout
    expect(spy).toBeCalled();
  });

  it('should call render', () => {
    const wrapper = shallow(<OQDashboard />);
    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });

});
