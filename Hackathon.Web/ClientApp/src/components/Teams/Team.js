import React, { Component } from 'react';
import axios from "axios";

export class Team extends Component {

    static displayName = Team.name;

    constructor(props) {
        super(props);

        this.API = process.env.REACT_APP_API_URL;

        this.state = {
            teamId: this.props.match.params.id,
            isLoading: false
        };
    }

    componentDidMount() {
        this.getTeam(this.state.teamId);
    }

    getTeam(id){

        this.setState({ isLoading: true });
        const endpoint = `${this.API}/Team/${id}`;
        axios.get(endpoint).then(
            response => {
                console.log(endpoint);
                console.log(response);
                this.setState({
                    team: response.data
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
                this.state.team !== undefined &&
                <div>
                    <h1>Team {this.state.team.id}</h1>
                    <ul>
                        <li><b>id</b>: {this.state.team.id}</li>
                        <li><b>name</b>: {this.state.team.name}</li>
                        <li>
                            <b>eventId</b>: {this.state.team.eventId}
                            <br/>
                            <a href={"/Event/" + this.state.team.eventId}>{this.state.team.event.name}</a>
                        </li>
                        <li><b>members</b>:
                        </li>
                    </ul>
                </div>
        );
    }
}
