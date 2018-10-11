import React from 'react';
import { Col, Row, Container } from 'reactstrap';

const Footer = () => (
  <footer className="landing__footer">
    <Container>
      <Row>
        <Col md={12}>
          <p className="landing__footer-text"> &copy; 2018 Industrial Training Services, Inc.
                <a href="http://www.its-training.com/" rel="noopener noreferrer" target="_blank"> Business</a>
          </p>
        </Col>
      </Row>
    </Container>
  </footer>
);

export default Footer;
