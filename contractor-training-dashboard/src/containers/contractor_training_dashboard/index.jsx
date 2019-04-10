/* eslint-disable */
import React, { PureComponent } from 'react';
import { Card, Col, Row, Container } from 'reactstrap';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import Footer from './components/Footer';
import Layout from '../_layout/index';
import Dashboard from './components/CTDashboard';

class ContractorTrainingDashboard extends PureComponent {
  static propTypes = {
    dispatch: PropTypes.func.isRequired,
  };

  componentDidCatch(error, info) {
    // Display fallback UI
    // this.setState({ hasError: true });
    // You can also log the error to an error reporting service
    console.log(error, info);
  }

  reloadWindow() {
    let readAPIErrorCount = localStorage.getItem('readAPIErrorCount');
    if (readAPIErrorCount <= 2) {
      readAPIErrorCount = parseInt(readAPIErrorCount) + 1;
      localStorage.setItem('readAPIErrorCount', readAPIErrorCount);
      location.reload();
    } else {
      localStorage.removeItem('readAPIErrorCount');
      window.location = window.location.origin;
    }
  };

  render() {
    return (
      <div className="landing">
        <div className="landing__menu">
          <div id="api-error-modal-loader-layer" className="api-error-modal loader-hide" tabIndex="-1">
            <div className="">
              <div className="modal show loader-show" role="dialog" tabIndex="-1">
                <div className="modal-dialog auto-logout-modal modal-dialog-centered" role="document">
                  <div className="modal-content">
                    <div className="modal-header">
                      <h5 className="modal-title"> Alert</h5>
                    </div>
                    <div className="modal-body">Sorry, something went wrong. Please refresh the page.</div>
                    <div className="modal-footer"><button color="primary" onClick={this.reloadWindow}>Refresh</button> </div>
                  </div>
                </div>
              </div>
              <div className="modal-backdrop show"></div>
            </div>
          </div>
          <Layout />
          <Container>
            <Row>
              <Col md={12}>
                <Col md={12} lg={12}>
                  <Card>
                    <Dashboard />
                    <Footer />
                  </Card>
                </Col>
              </Col>
            </Row>
          </Container>
        </div>
      </div>
    );
  }
}

export default connect(state => ({ theme: state.theme }))(ContractorTrainingDashboard);
