import React, {FC} from 'react';
import './App.css';
import {QueryClient, QueryClientProvider} from "react-query";
import Patients from "./features/registry/components/patients/Patients";

const App:FC=()=> {
    return (
        <Patients/>
    );
}

export default App;
