import React, { Component } from 'react';
import axios from "axios";

export class Events extends Component {

    static displayName = Events.name;

    constructor(props) {
        super(props);

        this.API = process.env.REACT_APP_API_URL;

        this.state = {
            isLoading: false
        };
    }

    componentDidMount() {
        this.getEvents();
    }

    getEvents(){
        this.setState({ isLoading: true });

        axios.get(`${this.API}/Event`).then(
            response => {
                console.log(response);
                this.setState({
                    events: response.data.items
                });
            },
            error =>{
                console.log(error);
            })
            .finally(()=>{
                this.setState({ isLoading: false });
            });
    }

    render () {
        return (
            <div>
                <h1>Events</h1>

                <ul>
                    {this.state.events && this.state.events.length > 0 && this.state.events.map(( item, index ) => (
                        <li key={ index }>
                            <a href={"/event/" + item.id}>{ item.name }</a>
                        </li>
                    ))}
                </ul>
            </div>
        );
    }
}
