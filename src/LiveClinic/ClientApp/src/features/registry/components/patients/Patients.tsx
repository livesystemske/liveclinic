import React, {FC} from 'react';
import {Patient} from "../../models/patient";
import PatientList from "./PatientList";
import {useQuery} from "react-query";
import {patientService} from "../../services";


const Patients: FC = () => {

    const {isLoading, error, data} = useQuery<Patient[], Error>('patients', async () => patientService.getAll());

    if (isLoading) return (<h1>Loading...</h1>)

    if (error) return <h1>`An error has occurred: ${error.message}`</h1>

    return (
        <div>
            <h3>Registry</h3>
            <PatientList patients={data}/>
        </div>
    );
}
export default Patients;
