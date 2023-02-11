import React, {FC} from 'react';
import {Patient} from "../../models/patient";
import PatientList from "./PatientList";
import {useQuery} from "react-query";
import {patientService} from "../../services/patient-service";
import {Button} from "primereact/button";
import {Toolbar} from "primereact/toolbar";
import {useNavigate} from "react-router-dom";
import {ProgressBar} from "primereact/progressbar";
import {Message} from "primereact/message";
import {useAuthUser} from "../../../auth/services/useAuthUser";


const Patients: FC = () => {
    const navigate = useNavigate();
    const {isLoading, error, data} = useQuery<Patient[], Error>('patients', async () => patientService.getAll());

    if (isLoading) return <ProgressBar mode="indeterminate" style={{height: '6px'}}></ProgressBar>

    if (error) return <Message severity="error" text={`An error has occurred: ${error.message}`}/>

    const onLoadPatient = (patientId: number) => {
        navigate(`/registry/patient/${patientId}`)
    }

    const startContent = (
        <React.Fragment>
            <Button label="Register" icon="pi pi-plus" className="mr-2" onClick={(event) => onLoadPatient(0)}/>
        </React.Fragment>
    );

    return (
        <div>
            <h3>Registry</h3>
            <div className="card">
                <Toolbar start={startContent}/>
            </div>
            <PatientList patients={data} loadPatient={onLoadPatient}/>
        </div>
    );
}
export default Patients;
