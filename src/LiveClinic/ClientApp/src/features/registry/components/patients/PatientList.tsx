import {Patient} from "../../models/patient";
import React, {FC} from "react";
import PatientListItem from "./PatientListItem";

interface Props {
    patients ?:Patient[];
}

const PatientList:FC<Props>=({patients})=> {

    const patientList = patients?.map((p,i) =>
        <PatientListItem key={i} patient={p}/>
    );

    return (
        <div>
            <ul>{patientList}</ul>
        </div>
    );
}
export default PatientList;
