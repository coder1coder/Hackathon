import React, {Component} from "react";
import axios from "axios";

export class SignUp extends Component
{
    constructor(props) {
        super(props);

        this.API = process.env.REACT_APP_API_URL;

        this.state = {
            username: '',
            password: '',
            email: '',
            fullName: '',
            isLoading: false
        };

        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleInputChange = e => {
        const target = e.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.setState({
            [name]: value
        });
    }

    handleSubmit(event) {
        event.preventDefault();

        const endpoint = `${this.API}/User`;

        this.setState({ isLoading: true });

        axios.post(endpoint, {
            username: this.state.username,
            password: this.state.password,
            email: this.state.email,
            fullName: this.state.fullName
        }).then(response => {
            console.log(endpoint);
            console.log(response);
            alert(`username: ${this.state.username} created`);
        })
        .catch(function (error) {
            let problemDetails = error.response.data;
            console.log(problemDetails);
            if (problemDetails.status === 400)
            {
                alert(problemDetails.detail)
            }
        })
        .finally(()=>{
            this.setState({ isLoading: false });
        });
    }

    render(){
        return (
            <div>
                <h1>SignUp</h1>
                <form onSubmit={this.handleSubmit}>

                    <label>userName: <input type="text" name="username" value={this.state.username} onChange={ e =>
                        this.setState({ username: e.target.value })} /></label>
                    <label>password: <input type="password" name="password" value={this.state.password} onChange={ e =>
                        this.setState({ password: e.target.value })} /></label>

                    <label>email: <input type="text" name="email" value={this.state.email} onChange={ e =>
                        this.setState({ email: e.target.value })} /></label>
                    <label>fullName: <input type="text" name="fullName" value={this.state.fullName} onChange={ e =>
                        this.setState({ fullName: e.target.value })} /></label>


                    <input type="submit" value="Отправить" />
                </form>
            </div>
        )
    }
}