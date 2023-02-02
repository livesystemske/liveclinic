import React, {FC} from 'react';
import {Patient} from "../../models/patient";

interface Props {
    patient ?:Patient;
}

const PatientListItem:FC<Props>=({patient})=> {
    return (
        <div>
           <li>{`${patient?.memberNo} | ${patient?.patientName?.FirstName} | ${patient?.patientName?.LastName} | ${patient?.gender}`}</li>
        </div>
    );
}
export default PatientListItem;
