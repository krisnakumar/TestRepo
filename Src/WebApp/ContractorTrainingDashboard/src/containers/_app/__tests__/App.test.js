import React from "react";
import App from "../App.jsx";
import { shallow, mount } from 'enzyme';

import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

Enzyme.configure({ adapter: new Adapter() });

describe("App component", () => {
  test("renders", () => {
    const wrapper = shallow(<App />);
    expect(wrapper.exists()).toBe(true);
  });
  it("check state[modal] is false on mounting", () => {
    const wrapper = shallow(<App />);
    expect(wrapper.state('modal')).toEqual(false);
  });
  it('should call _onAction', () => {
    const wrapper = shallow(<App />);
    const spy = jest.spyOn(wrapper.instance(), '_onAction');
    //update the instance with the new spy
    wrapper.instance()._onAction();
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });
  it('should call _onActive', () => {
    const wrapper = shallow(<App />);
    const spy = jest.spyOn(wrapper.instance(), '_onActive');
    //update the instance with the new spy
    wrapper.instance()._onActive();
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });
  it('should call _onIdle', () => {
    const wrapper = shallow(<App />);
    const spy = jest.spyOn(wrapper.instance(), '_onIdle');
    //update the instance with the new spy
    wrapper.instance()._onIdle();
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });
  it('should call autoLogout', () => {
    const wrapper = shallow(<App />);
    const spy = jest.spyOn(wrapper.instance(), 'autoLogout');
    //update the instance with the new spy
    wrapper.instance().autoLogout();
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });
  it('should call toggle', () => {
    const wrapper = shallow(<App />);
    const spy = jest.spyOn(wrapper.instance(), 'toggle');
    //update the instance with the new spy
    wrapper.instance().toggle();
    //invoke componentDidCatch
    expect(spy).toBeCalled();
  });
  it('should call cancelAutoLogout', () => {
    const wrapper = shallow(<App />);
    const spy = jest.spyOn(wrapper.instance(), 'cancelAutoLogout');
    //update the instance with the new spy
    //// wrapper.instance().cancelAutoLogout();
    //invoke componentDidCatch
    //// expect(spy).toBeCalled();
  });
  it('should call componentDidMount', () => {
    sessionStorage.setItem("dashboardAPIToken", '{"dashboardAPIToken":{"AccessToken":"eyJraWQiOiJ1MUpCRFZ5eGJ0T1MzcFBSTm9JdWJjYUs5ZjJYQ2dlOXJyTXJWQjEzc0J3PSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiJkZWE2YTEwZi04OTk0LTRhNDYtOGE2MC0wMjk4M2Q2M2I1MGEiLCJldmVudF9pZCI6IjRjZjEzYTMxLTc1N2QtMTFlOS04YjhkLTkxYmIwMmE4M2RmYSIsInRva2VuX3VzZSI6ImFjY2VzcyIsInNjb3BlIjoiYXdzLmNvZ25pdG8uc2lnbmluLnVzZXIuYWRtaW4iLCJhdXRoX3RpbWUiOjE1NTc3NTE2NjMsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy13ZXN0LTIuYW1hem9uYXdzLmNvbVwvdXMtd2VzdC0yX0Y1NGo2bTNUMiIsImV4cCI6MTU1Nzc1NTI2MywiaWF0IjoxNTU3NzUxNjYzLCJqdGkiOiJkMDJiNjg2Ny05OTdjLTRhMzAtYjFkNC0wYTBhYjAzMzdiYzgiLCJjbGllbnRfaWQiOiI1aHU0OHJkM21iZnZyM3Q2ZDZyMnJjaGFwbSIsInVzZXJuYW1lIjoibHVjZW50ZWpfMzMxNTM1In0.Ac9Yq0J1R43eYH8V49K4kFr6U468KuYtdqqdfbqwekm2PncpOk4V5EYAn_AbTsMEf1HllTBzQbOmRJpT6hdK7b2UIhHmDj-SVP3B3q5-OHnxRNnAf0cjwlPByx0FaMbK4JXVcvyATcNEW7cX9FDU52q9QsJ6e8RsYiIsbQ0YJHo8QKTmwxcbARzwzmYuw3DwY8bWftT-l2omvaPPK4ZfnBzsDf8Xa3a7KzW7BfIUQKhhlYlCX5LxAYebJBS-oUdqpdObJ8cZ6dRSwdep-E6LtlQqv95cKvrjev6HjGLRyuTO7gzrDVO4I7AP0m6g4dVhrQtf6385Ihq2bN9CJ1j8Sg","ExpiresIn":3600,"IdToken":"eyJraWQiOiJ5REQ0ZjA5bm9ScndSc2hKMXM3OEFhUW0wUHVUZjVBUFwvQ3A2SkFjS0V4RT0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJkZWE2YTEwZi04OTk0LTRhNDYtOGE2MC0wMjk4M2Q2M2I1MGEiLCJhdWQiOiI1aHU0OHJkM21iZnZyM3Q2ZDZyMnJjaGFwbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJldmVudF9pZCI6IjRjZjEzYTMxLTc1N2QtMTFlOS04YjhkLTkxYmIwMmE4M2RmYSIsInRva2VuX3VzZSI6ImlkIiwiYXV0aF90aW1lIjoxNTU3NzUxNjYzLCJpc3MiOiJodHRwczpcL1wvY29nbml0by1pZHAudXMtd2VzdC0yLmFtYXpvbmF3cy5jb21cL3VzLXdlc3QtMl9GNTRqNm0zVDIiLCJjdXN0b206dXNlcl9pZCI6IjMzMTUzNSIsImNvZ25pdG86dXNlcm5hbWUiOiJsdWNlbnRlal8zMzE1MzUiLCJleHAiOjE1NTc3NTUyNjMsImlhdCI6MTU1Nzc1MTY2MywiZW1haWwiOiJuby1yZXBseUBpdHMtdHJhaW5pbmcuY29tIn0.D7pyWRXx9AHN1LYk8I0mQeyn3_AbPpK6r53ujcA_Bwy-lF3bue6CLrGSWteWf1nE1ddaPju212skbypn0SeMzPrMwoF6rBua5cFZ2pgcKyo7HNeckjHeyfjdtmKZnmJb_epSLD2m0s7wo_W0dYMbzvPIXKxna9fa49kgYn2BZbNEkExF_uOcfa7hLiY_UHVSL2se-QrAK5RToOOXFWx9XoicNIbJcwdwch9aGv-967gky--8X2sGD66sBkIojSv9f7Vo30adj-xE82lLJWvG1PRQqi0x-2e4Vo_tpM1QjhEwpxtJ6Bgywx3GohyORVmr7WzttfKs30NLAXt9rzO6-A","NewDeviceMetadata":null,"RefreshToken":"eyJjdHkiOiJKV1QiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiUlNBLU9BRVAifQ.gPHF0r9iuKU-6mignsS2sBgXSfxsSVtGIaOlQ_ZOASfk0Ih5tOOLG2Jm7o7MPH0dQaGrT5Nuo-M9teNU3zYN8FJ1jtv1yugtvRu9d-cTobpG-fFq9Mbh_cyz2iLkblUE6pnFhP8j6SdLG-LKhWFr-uITOYSwH6SsgYCqZ2tZQy5NOwL7et7_wiEEu3_aOyHNrI8zKQz7R92ihs5pA2FxvXQXhwZkOBA5p8HQyDBsWuRlguvdk7bt4FhuYe3v1giZMKE0s3IIH2nA-Mcv74LxfhF61r1xMQHgM9r-e9ws-OLD0AwRO4jTLwXun0cd84CBtzzVpvbD9QxWOUgaIathQg.GjxB2tVihDPYfGbK.ygoiT7iNEk2IRcEY1xdryaJdYyluqG4rr7GhYzbLsD37B1EYoXY6iNfdG6fW8c-lriDcZRW18Zrv5T4fbyYUZoz8bceatLmpaNW7YjsLkSAelUD2oknJ4Qi4TJazAWl939McQyDxf3gb__lHnDAX_nCcFyf95jGJduROJBrSzjCVE8XTcGwAdpKbht4w3fTYXxzlw6rAuk2G8it6KxN-anm9BstAYFtovevg18f4Ag9D1zjhXzAcUCFvNkHKDzq48EZU_Ir44_GSDHpoa5_7n1TAHmiT3zHtFdo6mPZ1Eb60TSCyc3mZ-4XuSK6XioTM-5Ko2WvLnuFSX3aq6MpLEx7RXZZT4OatdNOxoMrD9CgBtvilHLr2ZU2fK25QzF-ieV9NgrMnEUh6ltDf5hEmH__bSHhMz9u4fhggaxnDRvZ0WneGsCvUIHRqxnQeWjPICG-HKeIoNIEvoKgqkbPpmstuOHCcFCcqFl-4uvt3QfyygYjN5Yd-slsKPv209BvSdmPv-y6P72SlS6mkr9MyMQQsDm1v61_QXg8BZdV_o5hlHT_d39jPxwP5yQFLcJS10AnPQFq7ptxv2oxUNaOMk587_Kz4su1y7vGZIlBfIWW3iiVRtnH61ziIenMi_3nemthUJl7xK_YQhi1azFnFCVpxYxWwbL-mkxW_To3myF0NkH2p_WExnR0-j-5QSWFnjKGi8xVjCxXNOcBkxSKzNBe8ez72Mq8ogr60BHcDecjWD5yM2741QPkIjOBIY4v8f7QNS7sFNH-IPqW6MGcuUmfKQETMedzFLCGlco6UV6PK-X8mc8hAdMMdKM6uBe65Gub4JDEGnie8VijZpDa7J4sXj51kfCveGWW0o291utmDsJlXm4XjXdWkf0_HQ_eCeH1MUAmbqKc5j-sSPZM3IHMWuHVGWt9KVK8_HjB9dWNYKfR3dInBl9wcCpl6HjTZZymPmEKQHTJB29_AxyTT0XfT4f0nrk9Wvu4Zlk0m49ors_-uHLGoTDdGHF7M_wiagBt6qZPEBadJZDMWw0xl6Y0yByetfdDvvV81U2ZXfQkSDFs9kPoQ0A1sAANGn6GyLcBXowTtrgy3Zt8gIEtRZh_9gYLWqIvtFYHguWXqmfTV7yS6TIKudJrHjeipzHJVmN55Scc2KRlemizxBDiQb8BgRHaZn7VCqGgJtOf8MYifYc7gKqp_Ur2wRg_-2E1UqpqJFHfMFHnKdDYSWovendjC0IKhNzTp3thiMYbQ3apiBZfNSl1WMaCZJLejtgj4hN1TY-lcN4wW4XpEHwtldg.klc3-r17vYVjCQgmhYr08A","TokenType":"Bearer"}}');
    const wrapper = shallow(<App />);
    const spy = jest.spyOn(wrapper.instance(), 'componentDidMount');
    //update the instance with the new spy
    wrapper.instance().componentDidMount({ pastDueQualifications: [], modal: true });
    //invoke componentDidMount
    expect(spy).toBeCalled();
  });
  it('should call componentWillMount', () => {
    const wrapper = shallow(<App />);
    const spy = jest.spyOn(wrapper.instance(), 'componentWillMount');
    //update the instance with the new spy
    wrapper.instance().componentWillMount();
    //invoke componentWillMount
    expect(spy).toBeCalled();
  });
  it('should call updateSessionTokens', () => {
    const wrapper = shallow(<App />);
    const spy = jest.spyOn(wrapper.instance(), 'updateSessionTokens');
    //update the instance with the new spy
    wrapper.instance().updateSessionTokens({ refreshSession: { IdToken: "eyJraWQiOiJ5REQ0ZjA5bm9ScndSc2hKMXM3OEFhUW0wUHVUZjVBUFwvQ3A2SkFjS0V4RT0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJkZWE2YTEwZi04OTk0LTRhNDYtOGE2MC0wMjk4M2Q2M2I1MGEiLCJhdWQiOiI1aHU0OHJkM21iZnZyM3Q2ZDZyMnJjaGFwbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJldmVudF9pZCI6ImY4OWQ3Mzc2LTc1ODEtMTFlOS05ZTQ1LTMxMjRmY2YxZDdjNiIsInRva2VuX3VzZSI6ImlkIiwiYXV0aF90aW1lIjoxNTU3NzUzNjY5LCJpc3MiOiJodHRwczpcL1wvY29nbml0by1pZHAudXMtd2VzdC0yLmFtYXpvbmF3cy5jb21cL3VzLXdlc3QtMl9GNTRqNm0zVDIiLCJjdXN0b206dXNlcl9pZCI6IjMzMTUzNSIsImNvZ25pdG86dXNlcm5hbWUiOiJsdWNlbnRlal8zMzE1MzUiLCJleHAiOjE1NTc3NTcyNjksImlhdCI6MTU1Nzc1MzY2OSwiZW1haWwiOiJuby1yZXBseUBpdHMtdHJhaW5pbmcuY29tIn0.g1TkL0xyxcCgl0YTpwbCqASnLnUNqKLneR8H-9O2Kvtuwr59gkwhSodCxwlN4ssq48Da1K0jAGNZiT0FPdlkSfmQkUYyMBzhLMU7g1XEMzRV2aOtwN_2Z-VUo7Wh1AzuS5utcGmwOecPfA1W8CmQluzAlDCWbTNcWcqQVWfk2yoqhJnWCGR642z2P4p8UrwJOkL04BoJfVUG0ggfswdrxbO74AdYu6HgDYzZatdO8Ulyma1yfeiisu3YlTg3x_dX0tYgzbSKD1WrzDgV4CNsCVgfuaCwn9qRjo1CjbBhVHXgr7vG6KErRRkFWu3UfUZBMdVVgGYg-sEGReOHIOJ9fw", AccessToken: "eyJraWQiOiJ1MUpCRFZ5eGJ0T1MzcFBSTm9JdWJjYUs5ZjJYQ2dlOXJyTXJWQjEzc0J3PSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiJkZWE2YTEwZi04OTk0LTRhNDYtOGE2MC0wMjk4M2Q2M2I1MGEiLCJldmVudF9pZCI6ImY4OWQ3Mzc2LTc1ODEtMTFlOS05ZTQ1LTMxMjRmY2YxZDdjNiIsInRva2VuX3VzZSI6ImFjY2VzcyIsInNjb3BlIjoiYXdzLmNvZ25pdG8uc2lnbmluLnVzZXIuYWRtaW4iLCJhdXRoX3RpbWUiOjE1NTc3NTM2NjksImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy13ZXN0LTIuYW1hem9uYXdzLmNvbVwvdXMtd2VzdC0yX0Y1NGo2bTNUMiIsImV4cCI6MTU1Nzc1NzI2OSwiaWF0IjoxNTU3NzUzNjY5LCJqdGkiOiI3Yjk5YTgzMi1kZTAzLTQ0NDMtOTJlOS0wMTExMzdjYzhmZjYiLCJjbGllbnRfaWQiOiI1aHU0OHJkM21iZnZyM3Q2ZDZyMnJjaGFwbSIsInVzZXJuYW1lIjoibHVjZW50ZWpfMzMxNTM1In0.JOhLWZKqKbJ-SX9mFPRLd5UX9KtRY9vB0At7nqZwNLvrpR40ox1G1i5zDuuufsRNYTk254G7fnWus9H9zCswrZCsWHYEcO9TO3P9KjkS3V7qehj-Mk3btwa-30Q_wsrzUoTtQ1iFSDHvHY1w9tCGKetAjCfELNuAUPazEQ5EqW3WJiVP6VSKrPiorbt1N1kjdlLJIfP6E6pZoPXUIANANOAUMf9Uq4Yp1OVJyupF0A7fOatMgO6AU2l54_rwThLzcTkGhzCguOAySeYec5GIfo0nxRFM3Rav7VPjNEb8KZyobYHSHbL-i3rAyoICc2c_Siq7cMSWa7RvXVaJ31TsPA" } });
    //invoke updateSessionTokens
    expect(spy).toBeCalled();
  });
  it('should call render', () => {
    const wrapper = shallow(<App />);
    const spy = jest.spyOn(wrapper.instance(), 'render');
    //update the instance with the new spy
    wrapper.instance().render();
    //invoke render
    expect(spy).toBeCalled();
  });
});