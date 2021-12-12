import React, { Component } from 'react';
import axios from "axios";
import {NoResponse} from "../Common/NoResponse/NoResponse";
import {Language} from "../../common/Language";

export class Event extends Component {

    static displayName = Event.name;

    constructor(props) {
        super(props);

        this.API = process.env.REACT_APP_API_URL;

        this.state = {
            eventId: this.props.match.params.id,
            isLoading: false
        };
    }

    componentDidMount() {
        this.fetchEvent(this.state.eventId);
    }

    fetchEvent(id){

        this.setState({ isLoading: true });

        axios.get(`${this.API}/Event/${id}`).then(
            response => {
                console.log(response);
                this.setState({
                    event: response.data
                });
            })
            .catch(function (error) {
                console.log(error)
            })
            .finally(()=>{
                this.setState({ isLoading: false });
            });
    }

    render () {
        if (this.state.event !== undefined)
        {
            return (
                <div>
                    <h1>Event {this.state.event.id}</h1>
                    <ul>
                        <li><b>{Language.Event.changeEventStatusMessages}</b>: {this.state.event.changeEventStatusMessages}</li>
                        <li><b>{Language.Event.developmentMinutes}</b>: {this.state.event.developmentMinutes}</li>
                        <li><b>{Language.Event.id}</b>: {this.state.event.id}</li>
                        <li><b>{Language.Event.isCreateTeamsAutomatically}</b>: {this.state.event.isCreateTeamsAutomatically}</li>
                        <li><b>{Language.Event.maxEventMembers}</b>: {this.state.event.maxEventMembers}</li>
                        <li><b>{Language.Event.memberRegistrationMinutes}</b>: {this.state.event.memberRegistrationMinutes}</li>
                        <li><b>{Language.Event.minTeamMembers}</b>: {this.state.event.minTeamMembers}</li>
                        <li><b>{Language.Event.name}</b>: {this.state.event.name}</li>
                        <li><b>{Language.Event.start}</b>: {this.state.event.start}</li>
                        <li><b>{Language.Event.status}</b>: {this.state.event.status}</li>
                        <li><b>{Language.Event.teamPresentationMinutes}</b>: {this.state.event.teamPresentationMinutes}</li>
                    </ul>
                </div>
            )
        } else return (<NoResponse/>)
    }
}
