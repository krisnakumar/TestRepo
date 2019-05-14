/* eslint-disable */
/*
* index.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This JSX file will used render Application Layout
* Template: React PureComponent
* Prerequisites: BrowserRouter, CookiesProvider, Provider, React and babel
*/
import React from 'react';
import ReactDOM from 'react-dom';
// import { render } from 'react-dom';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import { I18nextProvider } from 'react-i18next';
import i18next from 'i18next';
import { CookiesProvider } from 'react-cookie';
import 'core-js';
import 'babel-polyfill';
import App from './containers/_app/App';
import store from './containers/_app/store';
import ScrollToTop from './containers/_app/ScrollToTop';

ReactDOM.render(
  <Provider store={store}>
    <BrowserRouter basename="/contractor-management/reports/oq-dashboard">
      <I18nextProvider i18n={i18next}>
        <ScrollToTop>
          <CookiesProvider>
            <App />
          </CookiesProvider>
        </ScrollToTop>
      </I18nextProvider>
    </BrowserRouter>
  </Provider>,
  // document.getElementById('root') || document.createElement('div'),
  document.getElementById('root'),
);
