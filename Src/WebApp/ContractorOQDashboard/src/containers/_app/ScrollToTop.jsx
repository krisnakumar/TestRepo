/* eslint-disable */
import { PureComponent } from 'react';
import { withRouter } from 'react-router-dom';

class ScrollToTop extends PureComponent {
  render() {
    return this.props.children;
  }
}

export default withRouter(ScrollToTop);
