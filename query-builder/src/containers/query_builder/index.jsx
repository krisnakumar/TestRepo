/* eslint-disable */
import React, { PureComponent } from 'react';
import { Card, Col, Row, Container } from 'reactstrap';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import { changeThemeToDark, changeThemeToLight } from '../../redux/actions/themeActions';
import Layout from '../_layout/index';
import Footer from './components/Footer';
import QuerySection from './components/QuerySection';

class QueryBuilder extends PureComponent {
  static propTypes = {
    dispatch: PropTypes.func.isRequired,
  };

  changeToDark = () => {
    this.props.dispatch(changeThemeToDark());
  };

  changeToLight = () => {
    this.props.dispatch(changeThemeToLight());
  };

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
                    <QuerySection />                               
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

export default connect(state => ({ theme: state.theme }))(QueryBuilder);
