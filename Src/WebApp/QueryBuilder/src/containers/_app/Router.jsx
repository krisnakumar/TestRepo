/* eslint-disable */
import React from 'react';
import { Route, Switch } from 'react-router-dom';
import MainWrapper from './MainWrapper';
import Login from '../account/log_in/index';

/**
 * @method
 * @name - Router
 * This method will append router component with
 * to Main Wrapper component
 * @param none
 * @returns none
 */
const Router = () => (
  <MainWrapper>
    <main>
      <div id="loader-layer" className="loader-hide">
        <div className="load loaded">
          <div className="ui-draggable custom-loader-wrapper" id="pp994">
            <table cellPadding="0" cellSpacing="0" id="pp9966">
              <tbody>
                <tr className="custom-loader-tr">
                  <td className="custom-loader-top-left"></td>
                  <td className="custom-loader-top-top"></td>
                  <td className="custom-loader-top-right"></td>
                </tr>
                <tr>
                  <td className="custom-loader-left"></td>
                  <td id="c2pp994">
                    <table cellPadding="0" cellSpacing="0" className="pptitle ui-draggable-handle" id="tpp994">
                      <tbody>
                        <tr>
                          <td><img src="https://d2vkqsz7y0fh3j.cloudfront.net/img/popup_caption_left.png" /></td>
                          <td className="custom-loader-caption-middle">
                            <div className="pptitlecaption" id="tpp994cap"></div>
                          </td>
                          <td className="custom-loader-caption-right"><img src="https://d2vkqsz7y0fh3j.cloudfront.net/img/popup_caption_right.png" /></td>
                        </tr>
                      </tbody>
                    </table>
                    <div id="cpp994">
                      <div className="custom-loader-cvloading"><img src="https://d2vkqsz7y0fh3j.cloudfront.net/img/cvloading.gif" className="custom-loader-cvloading-img" />
                        <div className="smallheader" id="loadingmsg">Loading...</div>
                      </div>
                    </div>
                  </td>
                  <td className="custom-loader-right"></td>
                </tr>
                <tr className="custom-loader-tr">
                  <td className="custom-loader-bottom-left"></td>
                  <td className="custom-loader-bottom-bottom"></td>
                  <td className="custom-loader-bottom-right"></td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
      <Switch>
        <Route exact path={`${process.env.PUBLIC_URL}/`} component={Login} />
      </Switch>
    </main>
  </MainWrapper>
);

export default Router;
