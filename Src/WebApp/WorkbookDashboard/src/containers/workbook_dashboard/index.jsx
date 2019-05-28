/* eslint-disable */
import React, { PureComponent } from 'react';
import { Card, Col, Row, Container } from 'reactstrap';
import Footer from './components/Footer';
import Layout from '../_layout/index';
import Table from './components/WorkBookDashboard';

class Landing extends PureComponent {
  componentDidCatch(error, info) {
    // Display fallback UI
    // this.setState({ hasError: true });
    // You can also log the error to an error reporting service
    console.log(error, info);
  }

  render() {
    return (
      <div className="landing">
        <div className="landing__menu">
          <Layout />
          <Container>
            <Row>
              <Col md={12}>
                <Col md={12} lg={12}>
                  <Card>
                    <Table />
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

// export default connect(state => ({ theme: state.theme }))(Landing);
export default Landing;
