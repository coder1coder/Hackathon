import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';

import './custom.css'
import {Events} from "./components/Events/Events";
import {Event} from "./components/Events/Event";
import {Teams} from "./components/Teams/Teams";
import {Team} from "./components/Teams/Team";

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
          <Route exact path='/' component={Home} />
          <Route path="/event/:id" component={Event} />
          <Route path="/events" component={Events}/>
          <Route path="/team/:id" component={Team} />
          <Route path="/teams" component={Teams}/>
      </Layout>
    );
  }
}
