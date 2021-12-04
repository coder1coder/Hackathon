import React, { Component } from 'react';
import axios from "axios";

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
        this.getEvent(this.state.eventId);
    }

    getEvent(id){

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
        return (
                this.state.event !== undefined &&
                <div>
                    <h1>Event {this.state.event.id}</h1>
                    <ul>
                        <li><b>changeEventStatusMessages</b>: {this.state.event.changeEventStatusMessages}</li>
                        <li><b>developmentMinutes</b>: {this.state.event.developmentMinutes}</li>
                        <li><b>id</b>: {this.state.event.id}</li>
                        <li><b>isCreateTeamsAutomatically</b>: {this.state.event.isCreateTeamsAutomatically}</li>
                        <li><b>maxEventMembers</b>: {this.state.event.maxEventMembers}</li>
                        <li><b>memberRegistrationMinutes</b>: {this.state.event.memberRegistrationMinutes}</li>
                        <li><b>minTeamMembers</b>: {this.state.event.minTeamMembers}</li>
                        <li><b>name</b>: {this.state.event.name}</li>
                        <li><b>start</b>: {this.state.event.start}</li>
                        <li><b>status</b>: {this.state.event.status}</li>
                        <li><b>teamPresentationMinutes</b>: {this.state.event.teamPresentationMinutes}</li>
                    </ul>
                </div>
        );
    }
}
