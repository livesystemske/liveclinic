import {Patient} from "../../models/patient";
import React, {FC} from "react";
import {DataTable} from "primereact/datatable";
import {Column} from "primereact/column";
import {Button} from "primereact/button";
import {useNavigate} from "react-router-dom";

interface Props {
    patients?: Patient[];
}

const PatientList: FC<Props> = ({patients}) => {
    const navigate = useNavigate();
    const openTemplate = (rowData: any, column: any) => {
        return (
            <div>
                <Button
                    icon="pi pi-external-link"
                    onClick={(event) =>navigate(`/registry/${rowData.id}`)}
                />
            </div>
        );
    };

    return (
        <div>
            <DataTable value={patients} responsiveLayout="scroll">
                <Column field="memberNo" header="No"></Column>
                <Column field="patientName.lastName" header="Last Name"></Column>
                <Column field="patientName.firstName" header="First Name"></Column>
                <Column field="gender" header="Gender"></Column>
                <Column field="birthDate" header="DOB"></Column>
                <Column
                    body={openTemplate}
                    style={{textAlign: "center", width: "5em"}}
                />
            </DataTable>
        </div>
    );
}
export default PatientList;
