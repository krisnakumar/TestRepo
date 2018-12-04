using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using ReportBuilderAPI.Handlers.FunctionHandler;
using ReportBuilder.Models.Response;
using ReportBuilder.Models.Request;
using ReportBuilder.UnitTest.Helpers;


/*******************************************************************************
<copyright file="RefreshTokens.cs">
    Copyright (c) 2018 All Rights Reserved
</copyright>
<author>Pawan Kumar</author>
<date>29-Nov-2018</date>
<summary>
    This class covers test cases to get refreshed token to continue the user session.
    It has three mandatory input fields, i.e., UserName, Password and RefreshToken
    All tests assert the response based on requested input.
    The class covers following test cases :
        - Correct inputs are given which yields a well defined success response with new tokens
        - Incorrect RefreshToken is given, which yields an error response
        - A request without mandatory input, which yields an error response
</summary>
*********************************************************************************/

namespace ReportBuilder.UnitTest.TestModules.Authentication
{
    [TestClass]
    public class RefreshTokens
    {
        /// <summary> 
        /// Test to get new tokens to continue the user session
        /// [Inputs]        Correct credentials (UserName, Password) are given
        /// [Expectations]  A well defined response with new tokens and success code
        /// [Assertions]    Success response code as 200
        ///                 Response contains IdentityToken (not null)
        /// </summary>
        [TestMethod]
        public void RefreshToken()
        {
            Function function = new Function();
            UserRequest userRequest = new UserRequest
            {
                UserName = "shoba.eswar@in.sysvine.com",
                Password = "Demo@2017",
                RefreshToken = "eyJjdHkiOiJKV1QiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiUlNBLU9BRVAifQ.ScOXyTE7BEQkOgAN8-7iQgu5TnHtDBVTxewGE4QTJ16wPIuXtPxhtAQvoPyfhgjvpC82mTRLrXeSdQrHKubT8i79P0oCh-0UUpUUXzYSYSrNM2wF1vcb2tS2Pg-U_SRRxOefqDlB-mcrx69ag9w85TATI6NVscZ5L3YyHeWCRM2O8IUvYRD0KE5qSIOWFmAsnEeEbazTkjVX648s8yQmNXJBBUPnQzO7FnGMDfkx5M787LVW7G3KEiqy_V0NS8hbqRHM8g-1AXtYcgydOR2ns1VzhQBPK3bm6gFa7kUsFWX89P9Iju_xvJXQiTEATLejhzgpUbC-Siz2yFeEQqoeRA.xrDUZq0MuhyDkqWK.I64C3Nb0jLS0q0UqrPptqvRo9a9HFUk2XsrvfwRcyTDxG4YOr6KUZrGSgV7vfmGxxBsPU_JoFDO9w7YOB0jVjuCi9h9Dg8VSWTO8lXOk5n56Ihtr_ocSoHIYMypmgj0zdSheCZcYFOMKZxqnxiFqYVocQyo0T7TmsfHceGamX9oHRNJnUPFHoWsjYeSnnhkHFUgYP4OGNag_kdDDtugbuwygozeXJKWN4E4Zx25ghxolwk0N0A7PfS-40ksITfUUfSeoWvEMoxdemYZmxO7Smm_yMDyPl4-7iPmnxT1rZ3VHzMOLRvg2xrdfCazyPUMSlxg35te4NVKg0sknKUqKgqG4I2xug5KiJ7nmBp-vAccgGb9My2axsdyFpUVVY4xHPM2Qb0M65jrI0FT2Rrr9B7J7dhzYgr4f0JjO0aRPht06I7U5vxa-rDJkUyOHkILJaeg0Knn6Qlx5ostTBgKg2LgUszojwGRGas40honJQiNe3QBc9ARQVaWoWMgE9CViS9Sm37QGJcPyyMurg8CxxlFNh2UW1Q4shExcG0FCzxtWWtVgzo0kSl2XUbULSEJfmp5dQWTIivrG23Ju9E4RYWaddl6MR4XU2PI01nzeXqDXIydXGRV7dvo36ouG8H48dWHSmxVW2yVpwywz-5rvN27b-RqyHmnWkRbXs6YPQuqoVJ6kEHfNESacb3fX2bMgIDnlkKezB-MJtL6l6CC_0OgEPLKoKGitua7wvfQkJQQ8MUzXQ6D1J5v2wAKgGDtH2C7ArqKkWMlweA7tleqFg-rmW6Qq09QqghI1ZC3C4Rsgt4VIa3xDGluG2a-17Sd8KtTwAObF-br5ydFkWEXk2moQh8HA_bTxvaBAlx_BdL8Y-nCYmdc_1dHNJl47vlb5jjJowiKxOvgHX1f1fOJbz1jq5v36rsXQXDzLdi4o7_gQrfZqDccKxN2e0nh0HHLGXaJjHYJL1IeCgJfLPfsMKh3uGg9rYqFyl7avRHWx_FFE3ARomy6tMOCMwSCRtZzVU5Nb83GFmoF_Mok-dUHold0LEueprqu8YTBp3cveryf7lgCT2Q8CuvZvjdEqtMaV88v6rAQVzjOs24D6Y0Rr_zMRfim4ZfBnU_Aj1YaBLW1G2EfO7Thr01lriV5ESm3yu3qfEyODdswit8tsoOb6FWAimUTwFUfQ1s089Y-wA5tmdWuaKJGLpxUxlwZ3DJG0bSbM7FVcWQrtufdPt642nKyK06vjAKgtstKQq110K5Wom4m8p4CbjWM3yroF_Vq4CsK_FFSItkc59DvmisBesGpDJKHDt20Lbpx6bOn2Ns5Kquq1Rwmyp8ejDcg.Gbb1tkQOL57HskEzCC5E6g"
            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(userRequest),
            };
            APIGatewayProxyResponse tokenResponse = function.SilentAuth(aPIGatewayProxyRequest, null);
            string responseToken = Convert.ToString(tokenResponse.Body);
            UserResponse userLoggedIn = JsonConvert.DeserializeObject<UserResponse>(responseToken);
            Assert.AreEqual(200, tokenResponse.StatusCode);
            Assert.IsNotNull(userLoggedIn.IdentityToken);
        }

        /// <summary> 
        /// Test to get new tokens to continue the user session
        /// [Inputs]        Incorrect credentials (UserName, Password) are given
        /// [Expectations]  An error response with bad request status code
        /// [Assertions]    Response code as 400
        ///                 IdentityToken in response is NULL
        ///                 Response body contains error message
        /// </summary>
        [TestMethod]
        public void RefreshTokenWithInvalidInput()
        {
            Function function = new Function();
            UserRequest userRequest = new UserRequest
            {
                UserName = "shoba.eswar@in.sysvine.com",
                Password = "Demo@2017",
                RefreshToken = "eyJjdHkiOiJKV1QiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiUlNBLU9BRVAifQ.UQcJez8trMKhATMy4W7SYegRvZk7v0kiwP2BXvX0LV0EFq3bvjPuRZwRBBB1WsH2OdMmJejbklLGKyfrIKhxqb4EIMCV_GIkpLvgg1j5nV6MbxCkTLbrzCV-JksvGxL6LkPmo2DEMdD2SQDQcMdlB5LIDlp3xiO97QxgzXcbx7RGKlTTYOPHvgNVSvu_rE3njCmUEb_EK9Dkf9-hpQFZY1PYh-yYn_xvrL1ZlSyimWi0NR_GisGB0LRh3mPg9LV1sFRlQSXjOwcUDT-yfsu85uAcMGK0zJhmHNZN9W4MTPxpV7BK2L_a8GgeJGMR_OtnOxPw5UXOPMaPLDXZ9mFzEg.AHwTM_ai7PbLeg9D.URNhAzxPSJHUsQwVsQK9-EyxOjUucfBY07Az61NOOQJLHaz_m1CaXcruPgMH6j4BstiWXE13y8mw1x5rPJRr1ajnUCnoBE_Xm1KMtfXa26q775sBOQQImeyNozGFlSw4TvdX_iLx8SiHxMSHhkNz_gibTw8vY8Gy2l4Gn5vdLOu96bGXGe8ns74XPbOrojZ4a0UBe3oG2pqv9IYSY-PqFVL1NLLkdv_N7umwrrkROZOFG1uV1JF3IPVYkYHZ2gZ_1tvV5MiRXCvfqKTvDuGhGdjVx_YHBFDq_ITl-tIOp1ZiAi1EqC1pfsovGNISPWfokgRm6qCK6P4R8V0WS1Fl8fkwVLtpBl_G2aRFNwce1UExL-xp0IYU9pgEjyKHZz--ORPp5J__oc9sB2MpApwrOuyIRJsVI7J0n0rTIcAOGypILL_9JTYVHxqu2_s8a26R94VYCfnruBsVOwl4cx-QcXtJyA8FhIan7aZDFpcjmzPQdl7BKYLHlg8xFWYtrIFlWJ7IsmOHWqCX9c7eZAmSs_S0SsTJ76fEcIdcEyrBZajpidY6Kmy0HXp2lyRWRRxO_KjrAYy90xfTmPFr6teHXG1EVBWnW9bRYgbFAM8BaqdbUCf9QdDK81KDmtOKnsBLYuakSFR3Y2CYBh18ljSVpRo7nzjt67MnFi-Mf4Ny9khamyefLsqfK6B1w_dvwRnduf0F199gPdP6RyzTOd5SXuHFSiuKXTyHKd--tbY81TfxseRyvcagJLO44BWzKwVtFrCfksosnPdqVTY3F9Imqe628KW1yMW4tGpXOdz2HmwHJK2NZoWBrIvgCaKFPvOdgaxli_jy66UGT1bjImI-GQ0jEZ9_DjCcEVa84-xcQrL-QcSC2GcvTMUroosdg_lVyoy9vl7aFNZsNbWsZoSedP8yxPQeVIliCwf372qpIvq3r0FYTPpo82mSRYcTl9ijhGoBLERf2rrnXxL4NKRaaPyTym9J3aWj5mOkyL4vGHK3kExLoNptDXjMUtiHOBVQc-RJZ0DtO3CESyvcUABeYjHB4KAYbkekxiPQygP3f11QSYwTCaBrklQ-aj0LmfqLUdfJothh_1gP-xzp1lGzxVXsc-fAfE9BPtxWv63PGBHJetj_ye_dCDBdsMyAycApJpzJw-IvcYw1XNv-MKcH-kli8KLUKj8Gl3eMtoECTsPr0wnmV6trmidCF"
            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(userRequest),
            };
            APIGatewayProxyResponse tokenResponse = function.SilentAuth(aPIGatewayProxyRequest, null);
            string responseToken = Convert.ToString(tokenResponse.Body);
            UserResponse userLoggedIn = JsonConvert.DeserializeObject<UserResponse>(responseToken);
            Assert.AreEqual(400, tokenResponse.StatusCode);
            Assert.IsNull(userLoggedIn.IdentityToken);
            StringAssert.Contains(userLoggedIn.Message, "Invalid input");
        }

        /// <summary> 
        /// Test to get new tokens to continue the user session
        /// [Inputs]        Mandatory input (RefreshToken) is not given
        /// [Expectations]  An error response with bad request status code
        /// [Assertions]    Response code as 400
        ///                 IdentityToken in response is NULL
        ///                 Response contains custom error code as 1
        ///                 Response contains error message
        /// </summary>
        [TestMethod]
        public void RefreshTokenWithoutMandatoryInput()
        {
            Function function = new Function();
            UserRequest userRequest = new UserRequest
            {
                UserName = "shoba.eswar@in.sysvine.com",
                Password = "Demo@2017"
            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(userRequest),
            };
            APIGatewayProxyResponse tokenResponse = function.SilentAuth(aPIGatewayProxyRequest, null);
            string responseToken = Convert.ToString(tokenResponse.Body);
            UserResponse userLoggedIn = JsonConvert.DeserializeObject<UserResponse>(responseToken);
            Assert.AreEqual(400, tokenResponse.StatusCode);
            Assert.IsNull(userLoggedIn.IdentityToken);
            Assert.AreEqual(userLoggedIn.Code, 1);
            StringAssert.Contains(userLoggedIn.Message, "Invalid input");
        }
    }
}
