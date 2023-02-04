import React, {FC} from 'react';
import {Link} from "react-router-dom";
import "./Navbar.css";
const Navbar: FC = () => {

    return (
        <div>
            <nav>
                <ul>
                    <li><Link to="/">Home</Link></li>
                    <li><Link to="/registry">Registry</Link></li>
                </ul>
            </nav>
            <hr/>
        </div>
    );
}
export default Navbar;
