import React, {FC} from 'react';
import {Patient} from "../../models/patient";
import {useQuery} from "react-query";
import {patientService} from "../../services/patient-service";
import {Fieldset} from "primereact/fieldset";
import {useParams} from "react-router-dom";

const PatientProfile: FC = () => {

    const {patientId} = useParams()

    const {isLoading, error, data} = useQuery<Patient, Error>('patient', async () => patientService.getById(patientId));

    if (isLoading) return (<h1>Loading...</h1>)

    if (error) return <h1>`An error has occurred: ${error.message}`</h1>

    const legendTemplate = (
        <div className="flex align-items-center text-primary">
            <span className="pi pi-user mr-2"></span>
            <span
                className="font-bold text-lg"> {`.  ${data?.patientName?.lastName} ${data?.patientName?.firstName}  | ${data?.memberNo}`}</span>
        </div>
    );

    return (
        <div className="card">
            <Fieldset legend={legendTemplate}>
                <>
                    <p><>DOB:{data?.birthDate}</></p>
                    <p><>Gender:{data?.gender}</></p>
                </>
            </Fieldset>
        </div>
    );
}
export default PatientProfile;
