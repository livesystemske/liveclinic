import React from 'react';
import {useAuthUser} from "./useAuthUser";
import {ProgressBar} from "primereact/progressbar";

function Login() {
    const {username, logoutUrl, isLoading} = useAuthUser();

    return (
        <>
            {isLoading ? (
                <ProgressBar mode="indeterminate" style={{height: '6px'}}></ProgressBar>
            ) : (
                <div>
                    {username}
                    <Mvc username={username} logoutUrl={logoutUrl}/>
                </div>
            )}
        </>
    );
}

interface MvcProps {
    username: string;
    logoutUrl: any;
}

function Mvc({username, logoutUrl}: MvcProps) {
    return (
        <div>
            {!username ? (
                <a href="/bff/login?returnUrl=/">Login</a>
            ) : (
                <div>
                    <p>{`Hi, ${username}!`}</p>
                    <a href={logoutUrl?.value}>Logout</a>
                </div>
            )}
        </div>
    );
}

export {Login};
