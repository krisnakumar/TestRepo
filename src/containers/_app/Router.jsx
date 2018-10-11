
import React from 'react';
import { Route, Switch } from 'react-router-dom';
import MainWrapper from './MainWrapper';
import Landing from '../landing/index';

const Router = () => (
  <MainWrapper>
    <main>
      <Switch>
        <Route exact path="/" component={Landing} />
      </Switch>
    </main>
  </MainWrapper>
);

export default Router;
