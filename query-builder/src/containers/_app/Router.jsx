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
          <div className="load__icon-wrap">
            <svg className="load__icon">
              <path fill="#4ce1b6" d="M12,4V2A10,10 0 0,0 2,12H4A8,8 0 0,1 12,4Z">.</path>
            </svg>
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
