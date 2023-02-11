import {Claim} from "./claim";

export interface AuthUser {
    username?: string,
    logoutUrl?: string,
    isLoggedIn?: boolean,
    sub?: string,
    session?: string,
    roles?: string[]
    claims?: Claim[];
}
