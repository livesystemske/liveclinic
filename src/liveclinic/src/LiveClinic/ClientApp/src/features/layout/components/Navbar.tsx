import React, {FC} from 'react';
import {Link} from "react-router-dom";
import "./Navbar.css";
import {useAuthUser} from "../../auth/services/useAuthUser";
const Navbar: FC = () => {

    const {authUser} = useAuthUser();

    return (
        <div>
            <nav>
                <ul>
                    <li><Link to="/">Home</Link></li>
                    <li><Link to="/registry">Registry</Link></li>
                </ul>
                {authUser.isLoggedIn
                    ?<Link to={`${authUser?.logoutUrl}`}>LogOut [{authUser.username}]</Link>
                    :<a href="/bff/login?returnUrl=/">Login</a>
                }
            </nav>
            <hr/>
        </div>
    );
}
export default Navbar;
