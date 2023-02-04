import React, {FC, useEffect, useState} from 'react';
import {Patient} from "../../models/patient";
import {useQuery} from "react-query";
import {patientService} from "../../services/patient-service";
import {useParams} from "react-router-dom";
import {useForm} from "react-hook-form";
import {Card} from "primereact/card";

type FormAction= {
    title?: string
    showEdit?:boolean
    showSave?:boolean
}
const PatientProfile: FC = () => {

    const [action,setAction]=useState<FormAction>({
        title: 'Register New',
        showEdit: false,
        showSave: true
    });
    const {patientId} = useParams()
    const {isLoading, error, data} = useQuery<Patient, Error>('patient', async () => patientService.getById(patientId));

    useEffect(() => {
       if (data?.id){
           setAction(prevState => ({
               ...prevState,title:'View/Edit Patient'
           }))
       }
    }, []);

    if (isLoading) return (<h1>Loading...</h1>)

    if (error) return <h1>`An error has occurred: ${error.message}`</h1>

    return (
        <Card title={action.title}>
            <p>Hey</p>
            {/*<form onSubmit={handleSubmit(onSubmit)}>*/}
            {/*    /!* register your input into the hook by invoking the "register" function *!/*/}
            {/*    <input defaultValue="test" {...register("example")} />*/}

            {/*    /!* include validation with required or other standard HTML validation rules *!/*/}
            {/*    <input {...register("exampleRequired", { required: true })} />*/}
            {/*    /!* errors will return when field validation fails  *!/*/}
            {/*    {errors.exampleRequired && <span>This field is required</span>}*/}

            {/*    <input type="submit" />*/}
            {/*</form>*/}
        </Card>
    );
}
export default PatientProfile;
