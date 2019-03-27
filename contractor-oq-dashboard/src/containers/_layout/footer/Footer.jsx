import React from 'react';
import { Col, Row, Container } from 'reactstrap';

const Footer = () => (
  <footer className="landing__footer">
    <Container>
      <Row>
        <Col md={12}>
          <div className="copyright">© 2019
            <a href="http://www.its-training.com" rel="noopener noreferrer" target="_blank">
              Industrial Training Services, Inc.
            </a>
            <br />
            <a href="/bz_business">Business</a>
          </div>
        </Col>
      </Row>
    </Container>
  </footer>
);

export default Footer;
