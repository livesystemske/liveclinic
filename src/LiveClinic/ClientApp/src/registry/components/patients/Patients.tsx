import React, {FC, useEffect, useState} from 'react';
import {Patient} from "../../models/patient";
import PatientList from "./PatientList";
import axios from "axios";

const Patients:FC=()=> {
    const [patients,setPatients]=useState<Patient[]>([]);

    const loadPatients=async () => {
        const {data} = await axios.get('/api/registry/patients')
        setPatients(data);
    }

    useEffect(()=>{
        loadPatients()
    },[]);
    return (
        <div>
            <h3>Registry</h3>
            <PatientList patients={patients}/>
        </div>
    );
}
export default Patients;
