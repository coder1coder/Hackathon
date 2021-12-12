import React, {Component} from "react";
import axios from "axios";
import {Button, Form, FormGroup, Input, Label} from "reactstrap";
import {Language} from "../../../common/Language";

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

                <Form onSubmit={this.handleSubmit}>
                    <h1>Регистрация нового пользователя</h1>

                    <FormGroup className={"mb-3"}>
                        <Label for="username" className={"form-label"}>Имя пользователя</Label>
                        <Input id="username"
                               name="username"
                               value={this.state.username}
                               onChange={ e => this.setState({ username: e.target.value })}
                               placeholder={"Имя пользователя"}
                               required
                        />
                    </FormGroup>

                    <FormGroup className={"mb-3"}>
                        <Label for="password" className={"form-label"}>Пароль</Label>
                        <Input id="password"
                               name="password"
                               value={this.state.password}
                               onChange={ e => this.setState({ password: e.target.value })}
                               placeholder={"Пароль"}
                               type="password"
                               required
                        />
                    </FormGroup>

                    <FormGroup className={"mb-3"}>
                        <Label for="email" className={"form-label"}>Email</Label>
                        <Input id="email"
                               name="email"
                               value={this.state.email}
                               onChange={ e => this.setState({ email: e.target.value })}
                               placeholder={"Email"}
                               type="email"
                               required
                        />
                    </FormGroup>

                    <FormGroup className={"mb-3"}>
                        <Label for="fullName" className={"form-label"}>Реальное имя пользователя</Label>
                        <Input id="fullName"
                               name="fullName"
                               value={this.state.fullName}
                               onChange={ e => this.setState({ fullName: e.target.value })}
                               placeholder={"Реальное имя пользователя"}
                               required
                        />
                    </FormGroup>

                    <div>
                        <Button color="primary" type="submit">Отправить</Button>
                    </div>
                </Form>
            </div>
        )
    }
}