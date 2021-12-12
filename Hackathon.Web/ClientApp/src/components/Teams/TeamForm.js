import React, {Component} from "react";
import {Button, Form, FormGroup, Input, Label} from "reactstrap";
import axios from "axios";
import {Language} from "../../common/Language";

export class TeamForm extends Component
{
    constructor(props) {
        super(props);

        this.API = process.env.REACT_APP_API_URL;

        this.state = {
            isLoading: false,
            form: {
                title: 'Новая команда'
            },
            team:{}
        };

        if (props.team?.length > 0){
            this.setState(prevState => ({
                form: {
                    ...prevState.form,
                    title: props.team.name
                }
            }));
        }

        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    componentDidMount() {
        this.setState({
            team: {
                name:'',
            }
        })
    }

    handleInputChange(e) {
        const target = e.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.setState(prevState => ({
            team: {
                ...prevState.team,
                [name]: value
            }
        }))
    }

    handleSubmit(e){
        e.preventDefault();

        this.setState({ isLoading: true });

        const endpoint = `${this.API}/Team`;

        axios
            .post(endpoint, this.state.team)
            .then(response => {
                if (response.status === 200 && response.data.id !== undefined)
                    this.props.history.push(`/Team/${response.data.id}`)
            })
            .catch(function (error) {
                alert(error.response.data.detail);
            })
            .finally(()=>{
                this.setState({ isLoading: false });
            });
    }

    render() {
        return(
            <Form onSubmit={this.handleSubmit}>
                <h1>{this.state.form.title}</h1>

                <FormGroup>
                    <Label for="name">{Language.Team.name}</Label>
                    <Input id="name"
                           name="name"
                           value={this.state.team.name}
                           onChange={this.handleInputChange}
                           placeholder={Language.Team.name}/>
                </FormGroup>

                <div>
                    <Button color="primary" type="submit">Сохранить</Button>
                </div>
            </Form>
        )
    }
}