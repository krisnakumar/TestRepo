import React from 'react';
import { Col, Row, Container } from 'reactstrap';
// import { Link } from 'react-router-dom';

// const background = `${process.env.PUBLIC_URL}/img/landing/header_bg.png`;
// const img = `${process.env.PUBLIC_URL}/img/landing/macbook.png`;

const Header = () => (
  <div className="landing__header">
    <Container>
      <Row>
        <Col md={12}>
          <h3> Hello World!!!</h3>
        </Col>
      </Row>
    </Container>
  </div>
);

export default Header;
