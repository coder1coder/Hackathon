import React, { Component } from 'react';
import { Route } from 'react-router';

import { Layout } from './components/Layout';
import { Home } from './components/Home';

import './custom.css'
import {Events} from "./components/Events/Events";
import {Event} from "./components/Events/Event";
import {Teams} from "./components/Teams/Teams";
import {Team} from "./components/Teams/Team";
import {SignIn} from "./components/Users/SignIn/SignIn";
import {SignUp} from "./components/Users/SignUp/SignUp";
import PrivateRoute from "./components/PrivateRoute";
import {EventForm} from "./components/Events/EventForm";
import {TeamForm} from "./components/Teams/TeamForm";

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
          <Route exact path='/' component={Home} />
          <Route path="/signIn" component={SignIn}/>
          <Route path="/signUp" component={SignUp}/>

          <PrivateRoute exact path="/event" component={EventForm}/>
          <PrivateRoute exact path="/event/:id" component={Event} />
          <PrivateRoute exact path="/events" component={Events}/>
          <PrivateRoute exact path="/team" component={TeamForm}/>
          <PrivateRoute exact path="/team/:id" component={Team} />
          <PrivateRoute exact path="/teams" component={Teams}/>

      </Layout>
    );
  }
}

