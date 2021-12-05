import React, {Component} from "react";
import axios from "axios";

export class SignIn extends Component
{
    constructor(props) {
        super(props);

        this.API = process.env.REACT_APP_API_URL;

        let token = localStorage.getItem('token');
        console.log(token);

        this.state = {
            username:'',
            password:'',
            isLoading: false,
            token: token
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

        const endpoint = `${this.API}/User/SignIn`;

        this.setState({ isLoading: true });

        axios.post(endpoint, {
            username: this.state.username,
            password: this.state.password
        }).then(response => {
            console.log(endpoint);
            console.log(response);
            console.log(response.data)
            localStorage.setItem('token', JSON.stringify(response.data))
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
                <h1>SignIn</h1>

                <form onSubmit={this.handleSubmit}>

                    <label>Имя: <input type="text" name="username" value={this.state.username} onChange={ e =>
                        this.setState({ username: e.target.value })} />
                    </label>
                    <label>Пароль: <input type="password" name="password" value={this.state.password} onChange={ e =>
                        this.setState({ password: e.target.value })} /></label>

                    <input type="submit" value="Отправить" />
                </form>
            </div>
        )
    }
}