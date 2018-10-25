/* eslint-disable */
import {
    SHOW_LOADER,
    HIDE_LOADER,
  } from '../actions/loaderActions';

  const initialState = {
    isLoaderShow: 'theme-light',
  };
  
  export default function (state = initialState, action) {
    switch (action.type) {
      case SHOW_LOADER:
        return { isLoaderShow: true };
      case HIDE_LOADER:
        return { isLoaderShow: false };
      default:
        return state;
    }
  }
  