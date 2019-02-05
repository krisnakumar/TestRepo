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
<author>Shoba Eswar</author>
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

                UserName = "devtester@its-training.com",

                RefreshToken = "eyJjdHkiOiJKV1QiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiUlNBLU9BRVAifQ.a40fcGx7A6aqc9_uXPLG_oqXDTstqDdxMCe7gipIEP3isUxQuM-e_aGgrMGxo9HZ_PBS1I2t9GV-mRf5AakZTMlZJqfYiPIhbPU9AeSUfM32k3Vx191bHPKkNpkOzj20-o7xzUI1rfF0r8x1It-V1sfyp54ybAtOh2ZbodujtVCWKekF6tCtxt4SdLCSjIPTRGNs2QDWe8AV_5JhDybIWuqDe3-Wj3J3T3GXyHYJNKMOdH1UvF8m4BH7r5MU0SWbR8cy80Ll0Cco2CwbzMCKDs-BoWH9AwfZZrqjHQZe6L8bB1EhwiSO7EmBNaKivw_AcfjvlLaN3RAO14dYiSOj9Q.RllyQxB-i2DGAPco.58FX3agH0re38YxenRZoJUy7FV4b1iYXzb6AWBqG1_T8LyCcXlc_W7PzDLXOrceQirewmi8xP_uxYl4bnrFvgGC9O-s16nlLvmQuOrWedfi02LNR4fCUUcNQz_U9uz3nAfIWt-uf98SBuJ7If2WbwtK2NqFjAyGXRujGDxEqxw5aQ3P9wSxVHNdJZPTao8aG4VHlOP9zbaAEmLt_ZEgZ2tbhurwHlEatPDFhoRh_As1a3owNvBDOOVLIUR1VNObKGXFpr7ZQW8bUHM2LN8WWd9bNBoy8t54g7PHBuHNf7h0VlPdZNFGDbLSsAW0U3WM8yV501WPgReBjNOB8y-wTJIBdLXok1OQtD6kbb5ixzro3BiV7WeY-c8FBI_IugFMODZaEQ3k0Fhgg_cU86wvW1L-tl7A9VM0vT9qVVjYdAC2OkLfKYFeHW1CBMDWD_GiZ_tq494bwWpFshVORA2SmkG05uFEBlwDKfwrAXuOoJp6Zc2Z86v456RkeCK5hTDwjtq0l8peXoJNQCpMinN07Aw9_sN9pP-noRI3XTSKZ0XTKEUJtkLOfQrOupV7P8dOc4SxffgnUc9Rf1Qruku-3_jaUZ71QTm_z9AriF-QCrXT33lX7eDKpYeCBAIBKJp1Pa3HT6O3tJ6e4O-4TG6j-XzRr0D2GNaZudmR4UEpy-jrlgJwjZB1UGON4iCJ2xyWEx6dIpeVD-da8TJcYdoamMO2y2Hmu2vPEtsNGTFTCoFga_3PQObFvsKIzv_PyEsyVWgqGbit2GOc6aOXE8uFhrlOnbp0hfKxzKRisUwKUrQBcERZvUa1mEL0dEwRLqwADkxIyn2pwvK-iWDaEfG4suCE0vBj6u56DO-2kDWTKf7Slq6ZeUQYW_epHcZFtiVH8iNIE4hmI6ygTe_XJDzf3HjHssrdyPDmImT3_EwUbCGDukNf-OEuk9R3poQPo_SI90qqj9xHVhA6SX8eHapPPApKCgdcimoVHNurwrsdRUefVQt-UIZy4xkAx7DQ-nufOBSgekWoWFrL9L2BzMo8mHXTeteFBaD6e-6TyUBVmezUwNnBXXT6QTpyBlIpSRfR1kjTllLqXvruVftZwa4r3OL26DXZu18KdsmfV2k_OOrTiFbrrAYot5prVjhETRpEDrYA3Y7V36zt1BuujoHf7vo664-t2Nv01yREr2Gautdei9_-T2S9J4uhvCJULLgYMPdx8NfaxuJ9DSVp_q4f5JMZaW_pjpq05KHOqBGqrbDdYtOe1GOnlhZlA1CV7NRGnvppWEuJE4xsQsGJJMWhQmf42qgtVPRRdMzmx36RJykRrwrP3NAHrO7FWj5U.URj_nE2qRt2cx147jAhypg"
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
