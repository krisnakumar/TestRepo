/*
* index.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This JSX file will used render Application Layout
* Template: React PureComponent
* Prerequisites: BrowserRouter, CookiesProvider, Provider, React and babel
*/

import React from 'react';
import { render } from 'react-dom';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import { I18nextProvider } from 'react-i18next';
import i18next from 'i18next';
import { CookiesProvider } from 'react-cookie';
import App from './containers/_app/App';
import store from './containers/_app/store';
import ScrollToTop from './containers/_app/ScrollToTop';

render(
  <Provider store={store}>
    <BrowserRouter basename="/reports/query-builder">
      <I18nextProvider i18n={i18next}>
        <ScrollToTop>
          <CookiesProvider>
            <App />
          </CookiesProvider>
        </ScrollToTop>
      </I18nextProvider>
    </BrowserRouter>
  </Provider>,
  document.getElementById('root'),
);
