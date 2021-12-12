import React, {Component} from 'react';
import axios from "axios";
import {NoResponse} from "../Common/NoResponse/NoResponse";
import {Loader} from "../Common/Loader/Loader";
import {Link} from "react-router-dom";
import {Button, Pagination, PaginationItem, PaginationLink, Table} from "reactstrap";
import {Language} from "../../common/Language";

function TeamsTable(props){
    const pageSize = props.pageSize ?? 10;
    const pageCount = ((props.teams?.totalCount ?? 0) / pageSize) - 1;
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
                    <td>{Language.Team.name}</td>
                    <td>{Language.Event.name}</td>
                </tr>
                </thead>
                <tbody>
                { props.teams?.items.length > 0 && props.teams?.items.map((item, index) => (
                    <tr key={index}>
                        <td>{item.id}</td>
                        <td><a href={"/Team/" + item.id}>{item.name}</a></td>
                        <td><a href={"/Event/" + item.event.id}>{item.event.name}</a></td>
                    </tr>
                ))}
                </tbody>
            </Table>
            <Pagination>{ paginationItems }</Pagination>
        </div>
    )
}

export class Teams extends Component {

    static displayName = Teams.name;

    constructor(props) {
        super(props);

        this.API = process.env.REACT_APP_API_URL;

        this.state = {
            isLoading: false,
            currentPage: 1
        };

        this.getTeams = this.getTeams.bind(this);
    }

    componentDidMount() {
        this.getTeams();
    }

    getTeams(page = 1, pageSize = 10) {

        this.setState({ isLoading: true });

        axios.get(`${this.API}/Team/`,{
            params: {
                Page: page,
                pageSize: pageSize,
                SortBy: 'Id'
            }
        }).then(response => {
            this.setState({
                teams: response.data,
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
                <h1>Команды</h1>
                <Link to="/Team">
                    <Button color="primary">Создать</Button>
                </Link>
                {
                    this.state.isLoading
                        ? <Loader/>
                        : this.state.teams?.items.length > 0
                            ? <TeamsTable teams={this.state.teams} currentPage={this.state.currentPage} onItemClick={this.getTeams}/>
                            : <NoResponse/>
                }
            </div>
        )
    }
}
