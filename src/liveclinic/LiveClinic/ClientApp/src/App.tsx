import React, {FC} from 'react';
import {BrowserRouter, Routes, Route} from "react-router-dom";
import './App.css';
import Navbar from "./features/layout/components/Navbar";
import Footer from "./features/layout/components/Footer";
import Patients from "./features/registry/components/patients/Patients";
import Home from "./features/dashboard/components/Home";
import PatientProfile from "./features/registry/components/patients/PatientProfile";

const App: FC = () => {
    return (
        <div>
            <BrowserRouter>
                <Navbar/>
                <Routes>
                    <Route path="/" element={<Home/>}/>
                    <Route path="/registry" element={<Patients/>}/>
                    <Route path="/registry/patient/:patientId" element={<PatientProfile/>}/>
                </Routes>
                <Footer/>
            </BrowserRouter>
        </div>
    );
}
export default App;
