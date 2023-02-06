import React, {FC, useEffect, useState} from 'react';
import {Patient} from "../../models/patient";
import {useQuery} from "react-query";
import {patientService} from "../../services/patient-service";
import {useParams} from "react-router-dom";
import {Card} from "primereact/card";
import {ProgressBar} from "primereact/progressbar";
import {Message} from "primereact/message";
import {useForm, SubmitHandler} from "react-hook-form";
import {InputText} from "primereact/inputtext";
import {Dropdown} from "primereact/dropdown";

interface FormAction {
    title?: string
    showEdit?: boolean
}

type Inputs = {
    firstName: string,
    lastName: string,
    gender: number,
    birthDate: Date,
};

const gender = [
    { name: 'Male', value:1 },
    { name: 'Female', value: 0 }
];

const PatientProfile: FC = () => {
    const [selectedGender, setSelectedGender] = useState(0);
    const {patientId} = useParams()
    const {register, handleSubmit, watch, formState: {errors}} = useForm<Inputs>();
    const [action, setAction] = useState<FormAction>({
        title: 'Register New',
        showEdit: false
    });
    const {
        status,
        isLoading,
        error,
        data
    } = useQuery<Patient, Error>('patient', async () => patientService.getById(patientId));
    const onSubmit: SubmitHandler<Inputs> = fdata => {
        // fdata.gender = selectedGender
        console.log(fdata)
    }
    useEffect(() => {
        if (status === 'success') {
            console.log('success >>>  ', patientId, data?.id)
            setAction({
                title: data?.id ? 'View/Edit Patient' : 'Register New',
                showEdit: data?.id ? true : false
            });
        }
    }, [status, data]);

    if (isLoading) return <ProgressBar mode="indeterminate" style={{height: '6px'}}></ProgressBar>

    if (error) return <Message severity="error" text={`An error has occurred: ${error.message}`}/>

    return (
        <Card title={action.title}>
            <form onSubmit={handleSubmit(onSubmit)}>
                <label htmlFor="firstName">firstName</label>
                <InputText id="firstName" value={data?.patientName?.firstName}  {...register("firstName", {required: true})} />
                {errors.firstName && <span>*</span>}
                <label htmlFor="lastName">lastName</label>
                <InputText id="lastName"  value={data?.patientName?.lastName} {...register("lastName", {required: true})} />
                {errors.lastName && <span>*</span>}
                <label htmlFor="gender">gender</label>
                <Dropdown ref= register("firstName", {required: true}) onChange={(e) => setSelectedGender(e.value)} id="gender"  optionLabel="name" value={data?.gender} options={gender} placeholder="Select Gender"/>

                {/*<label htmlFor="birthDate">birthDate</label>*/}
                {/*<input type="date" id="birthDate"   {...register("birthDate", {pattern:/yyy/i, value:data?.birthDate, valueAsDate:true, required: true})} />*/}
                {/*    {errors.birthDate && <span>*</span>}*/}
                <input type="submit"/>
            </form>
        </Card>
    );
}
export default PatientProfile;
