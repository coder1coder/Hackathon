import React, { Component } from 'react';
import axios from "axios";
import {Loader} from "../Common/Loader/Loader";
import {NoResponse} from "../Common/NoResponse/NoResponse";
import {Button, Pagination, PaginationItem, PaginationLink, Table} from "reactstrap";
import {Language} from "../../common/Language";
import {Link} from "react-router-dom";

function EventsTable(props){
    const pageSize = props.pageSize ?? 10;
    const pageCount = ((props.events?.totalCount ?? 0) / pageSize) - 1;
    const currentPage = props.currentPage ?? 1;

    let paginationItems = [];

    for (let i = 1; i < pageCount + 1; i++) {
        const isCurrentPage = currentPage === i;
        paginationItems.push(
            <PaginationItem key={i} active={isCurrentPage}>
                <PaginationLink onClick={(e) => {
                        e.preventDefault();
                        !isCurrentPage && props.onItemClick(i)
                    }
                }>{i}</PaginationLink>
            </PaginationItem>
        )
    }

    return (
        <div>
            <Table hover>
                <thead>
                <tr>
                    <td>#</td>
                    <td>{Language.Event.name}</td>
                    <td>{Language.Event.start}</td>
                </tr>
                </thead>
                <tbody>
                    { props.events?.items.length > 0 && props.events?.items.map((item, index) => (
                        <tr key={index}>
                            <td>{item.id}</td>
                            <td><a href={"/Event/" + item.id}>{item.name}</a></td>
                            <td>{item.start}</td>
                        </tr>
                    ))}
                </tbody>
            </Table>
            <Pagination>{ paginationItems }</Pagination>
        </div>
    )
}

export class Events extends Component {

    static displayName = Events.name;

    constructor(props) {
        super(props);

        this.API = process.env.REACT_APP_API_URL;

        this.state = {
            isLoading: false,
            currentPage: 1
        };

        this.getEvents = this.getEvents.bind(this);
    }

    componentDidMount() {
        this.getEvents();
    }

    getEvents(page = 1, pageSize = 10){

        this.setState({ isLoading: true })

        axios.get(`${this.API}/Event`,
            {
                params: {
                    Page: page,
                    pageSize: pageSize,
                    SortBy: 'Id'
                }
            }).then(
            response => {
                this.setState({
                    events: response.data,
                    currentPage: page
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
                <h1>События</h1>
                <Link to="/Event">
                    <Button color="primary">Создать</Button>
                </Link>
                {
                    this.state.isLoading
                        ? <Loader/>
                        : this.state.events?.items.length > 0
                            ? <EventsTable events={this.state.events} currentPage={this.state.currentPage} onItemClick={this.getEvents}/>
                            : <NoResponse/>
                }
            </div>
        )
    }
}
