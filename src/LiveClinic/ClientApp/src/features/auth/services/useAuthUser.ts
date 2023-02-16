import {useQuery} from 'react-query';
import {authService} from "./auth-service";
import {AuthUser} from "../model/auth-user";
import {Claim} from "../model/claim";
import {User} from "oidc-client";
import {useState} from "react";
import {Gender} from "../../../shared/models";


function buildUser(claims?: Claim[]): AuthUser {
    let user: AuthUser = {};
    if (claims && claims.length > 0) {

        let username = claims?.find((claim: any) => claim.type === 'name')
        let logoutUrl = claims?.find((claim: any) => claim.type === 'bff:logout_url')
        let sub = claims?.find((claim: any) => claim.type === 'sub')
        let session = claims?.find((claim: any) => claim.type === 'bff:session_state')

        user.username = username ? username.value : '';
        user.logoutUrl = logoutUrl ? logoutUrl.value : '';
        user.sub = sub ? sub.value : '';
        user.session = session ? session.value : '';
        user.isLoggedIn = username ? true : false;
        user.claims = claims;
    }
    return user;
}

function useAuthUser() {

    const [authUser, setAuthUser] = useState<AuthUser>({});

    const {data, isLoading: isUserLoading,error} = useQuery<Claim[],Error>(
        'user',
        async () => authService.getClaims(), {
            onSuccess: (data) => {
                console.log('NANI...',data);
                setAuthUser(buildUser(data));
            }
        });

    return {
        authUser,
        isUserLoading,
        error
    };
}

export {useAuthUser};
