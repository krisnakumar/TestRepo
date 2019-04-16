/* eslint-disable */
/*
* Footer.jsx
* Written by Prashanth Ravi (pravi@its-training.com)
* This javascript file will used render Footer on reports landing page
* Template: React PureComponent
* Prerequisites: Reactstrap, React and babel
*/

import React from 'react';
import { Col, Row, Container } from 'reactstrap';

const Footer = () => (
  <footer className="landing__footer">
    <Container>
      <Row>
        <Col md={12}>
          <div className="copyright">© 2019 &nbsp;
            <a href="http://www.its-training.com" rel="noopener noreferrer" target="_blank">
              Industrial Training Services, Inc.
            </a>           
          </div>
        </Col>
      </Row>
    </Container>
  </footer>
);

export default Footer;
