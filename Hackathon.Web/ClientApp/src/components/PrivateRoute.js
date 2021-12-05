import React from 'react';
import { Route, Redirect } from 'react-router-dom'
import axios from "axios";

const PrivateRoute = ({ component: Component, ...rest }) => {

    function checkAuth() {

        let tokenObject = localStorage.getItem('token');

        if (tokenObject === undefined)
        {
            console.log('token not found in localStorage')
            return false;
        }

        let tokenInfo = JSON.parse(tokenObject);

        console.log(tokenInfo);

        let expires = Date.parse(tokenInfo.expires);

        if (expires <= Date.now())
        {
            console.log('token expired')
            return false;
        }

        const endpoint = `${process.env.REACT_APP_API_URL}/user/validate?userId=${tokenInfo.userId}&token=${tokenInfo.token}`;
        console.log(endpoint);

        // axios
        //     .get(endpoint)
        //     .then(response=>{
        //         console.log(response);
        //         return true
        //     })
        //     .catch(() => { return false })

        return true;
    }

    return (
        <Route {...rest} render={props =>
            !checkAuth() ? ( <Redirect to='/signIn'/> ) : ( <Component {...props} /> )
        }
        />
    );
};

export default PrivateRoute;