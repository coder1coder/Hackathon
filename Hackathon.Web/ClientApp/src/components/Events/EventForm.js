import React, {Component} from "react";
import {Button, Form, FormGroup, Input, Label} from "reactstrap";
import axios from "axios";
import moment from "moment";
import {Language} from "../../common/Language";

export class EventForm extends Component
{
    constructor(props) {
        super(props);

        this.API = process.env.REACT_APP_API_URL;

        this.state = {
            isLoading: false,
            form: {
                title: 'Новое событие'
            },
            event:{}
        };

        if (props.event?.length > 0){
            this.setState(prevState => ({
                form: {
                    ...prevState.form,
                    title: props.event.name
                }
            }));
        }

        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    componentDidMount() {
        this.setState({
            event: {
                name:'',
                isCreateTeamsAutomatically: false,
                start: moment().format('YYYY-MM-DDTHH:mm'),
                developmentMinutes: 0,
                teamPresentationMinutes: 0,
                memberRegistrationMinutes: 0,
                minTeamMembers: 3,
                maxEventMembers: 3
            }
        })
    }

    handleInputChange(e) {
        const target = e.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.setState(prevState => ({
            event: {
                ...prevState.event,
                [name]: value
            }
        }))
    }

    handleSubmit(e){
        e.preventDefault();

        this.setState({ isLoading: true });

        const endpoint = `${this.API}/Event`;

        axios
            .post(endpoint, this.state.event)
            .then(response => {
                if (response.status === 200 && response.data.id !== undefined)
                    this.props.history.push(`/Event/${response.data.id}`)
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
                    <Label for="name">{Language.Event.name}</Label>
                    <Input id="name"
                           name="name"
                           value={this.state.event.name}
                           onChange={this.handleInputChange}
                           placeholder={Language.Event.name}/>
                </FormGroup>

                <FormGroup check>
                    <Input type="checkbox"
                           id="isCreateTeamsAutomatically"
                           value={this.state.event.isCreateTeamsAutomatically}
                           onChange={this.handleInputChange}
                    />
                    {' '}
                    <Label check>{Language.Event.isCreateTeamsAutomatically}</Label>
                </FormGroup>

                <FormGroup>
                    <Label for="start">{Language.Event.start}</Label>
                    <Input id="start"
                           name="start"
                           value={this.state.event.start}
                           min={this.state.event.start}
                           onChange={this.handleInputChange}
                           placeholder={Language.Event.start}
                           type="datetime-local"/>
                </FormGroup>

                <FormGroup>
                    <Label for="developmentMinutes">{Language.Event.developmentMinutes}</Label>
                    <Input
                        id="developmentMinutes"
                        name="developmentMinutes"
                        value={this.state.event.developmentMinutes}
                        onChange={this.handleInputChange}
                        placeholder={Language.Event.developmentMinutes}
                        type="number"
                    />
                </FormGroup>

                <FormGroup>
                    <Label for="teamPresentationMinutes">{Language.Event.teamPresentationMinutes}</Label>
                    <Input
                        id="teamPresentationMinutes"
                        name="teamPresentationMinutes"
                        value={this.state.event.teamPresentationMinutes}
                        onChange={this.handleInputChange}
                        placeholder={Language.Event.teamPresentationMinutes}
                        type="number"
                    />
                </FormGroup>

                <FormGroup>
                    <Label for="memberRegistrationMinutes">{Language.Event.memberRegistrationMinutes}</Label>
                    <Input
                        id="memberRegistrationMinutes"
                        name="memberRegistrationMinutes"
                        value={this.state.event.memberRegistrationMinutes}
                        onChange={this.handleInputChange}
                        placeholder={Language.Event.memberRegistrationMinutes}
                        type="number"
                    />
                </FormGroup>

                <FormGroup>
                    <Label for="minTeamMembers">{Language.Event.minTeamMembers}</Label>
                    <Input
                        id="minTeamMembers"
                        name="minTeamMembers"
                        value={this.state.event.minTeamMembers}
                        onChange={this.handleInputChange}
                        placeholder={Language.Event.minTeamMembers}
                        type="number"
                    />
                </FormGroup>

                <FormGroup>
                    <Label for="maxEventMembers">{Language.Event.maxEventMembers}</Label>
                    <Input
                        id="maxEventMembers"
                        name="maxEventMembers"
                        value={this.state.event.maxEventMembers}
                        onChange={this.handleInputChange}
                        placeholder={Language.Event.maxEventMembers}
                        type="number"
                    />
                </FormGroup>

                <div>
                    <Button color="primary" type="submit">Сохранить</Button>
                </div>
            </Form>
        )
    }
}