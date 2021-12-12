import React, {Component} from 'react';
import axios from "axios";
import {NoResponse} from "../Common/NoResponse/NoResponse";
import {Loader} from "../Common/Loader/Loader";

export class Teams extends Component {

    static displayName = Teams.name;

    constructor(props) {
        super(props);

        this.API = process.env.REACT_APP_API_URL;

        this.state = {
            isLoading: false
        };
    }

    componentDidMount() {
        this.getTeams();
    }

    getTeams() {
        this.setState({ isLoading: true });

        axios.get(`${this.API}/Team/`).then(
            response => {
                console.log(response);
                this.setState({
                    teams: response.data.items
                });
            },
            error =>{
                console.log(error);
            })
            .finally(()=>{
                this.setState({ isLoading: false });
            });
    }

    render() {
        return (
            <div>
                <h1>Teams</h1>
                {
                    this.state.isLoading
                    ? <Loader/>
                    : (
                        <ul>
                            {
                                this.state.teams?.length > 0
                                ? this.state.teams.map((item, index) => (
                                    <li key={index}>
                                        <a href={"/Team/" + item.id}>{item.name}</a>
                                    </li>))
                                :<NoResponse/>
                            }
                        </ul>)
                }
            </div>
        )
    }
}
