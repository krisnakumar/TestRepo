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
import { config as i18nextConfig } from './translations/index';

i18next.init(i18nextConfig);

render(
  <Provider store={store}>
    <BrowserRouter basename="/">
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
