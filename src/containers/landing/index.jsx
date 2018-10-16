/* eslint-disable */
import React, { PureComponent } from 'react';
import { Col, Row, Container } from 'reactstrap';
import scrollToComponent from 'react-scroll-to-component';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import Header from './components/Header';
import Footer from './components/Footer';
import { changeThemeToDark, changeThemeToLight } from '../../redux/actions/themeActions';
import Layout from '../_layout/index';
import Table from './components/DataTable';

const logo = `${process.env.PUBLIC_URL}/img/content_logo.png`;

class Landing extends PureComponent {
  static propTypes = {
    dispatch: PropTypes.func.isRequired,
  };

  changeToDark = () => {
    this.props.dispatch(changeThemeToDark());
  };

  changeToLight = () => {
    this.props.dispatch(changeThemeToLight());
  };

  render() {
    return (
      <div className="landing">
        <div className="landing__menu">
          <Layout />
          <Container>
            <Row>
              <Col md={12}>              
                <Table />
              </Col>
            </Row>
          </Container>
        </div>
        <Header />
        <span ref={(section) => {
          this.About = section;
        }}
        />
        <Footer />
      </div>
    );
  }
}

export default connect(state => ({ theme: state.theme }))(Landing);
