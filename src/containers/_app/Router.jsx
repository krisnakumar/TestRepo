
import React from 'react';
import { Route, Switch } from 'react-router-dom';
import MainWrapper from './MainWrapper';
import Landing from '../landing/index';
import Login from '../account/log_in/index';

const Router = () => (
  <MainWrapper>
    <main>
      <Switch>
        <Route exact path="/" component={Login} />
        <Route exact path="/reports" component={Landing} />
      </Switch>
    </main>
  </MainWrapper>
);

export default Router;
