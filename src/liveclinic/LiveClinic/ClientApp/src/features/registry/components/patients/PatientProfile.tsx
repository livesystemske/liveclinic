import React, {FC, useEffect, useState} from 'react';
import {Patient} from "../../models/patient";
import {useMutation, useQuery} from "react-query";
import {patientService} from "../../services/patient-service";
import {useParams} from "react-router-dom";
import {Card} from "primereact/card";
import {ProgressBar} from "primereact/progressbar";
import {Message} from "primereact/message";
import {Controller, useForm} from "react-hook-form";
import {classNames} from "primereact/utils";
import {InputText} from "primereact/inputtext";
import {Button} from "primereact/button";
import {Dropdown} from "primereact/dropdown";
import {Calendar} from "primereact/calendar";
import {Gender} from "../../../../shared/models";
import axios from "axios";

interface FormAction {
    title?: string
    showEdit?: boolean
}

const initialState: FormAction = {
    title: 'Register New',
    showEdit: false
}

const defaultValues = {
    id:0,
    firstName: '',
    lastName: '',
    gender: Gender.Female,
    birthDate: new Date()
};

const genders = [
    {name: 'Male', value: 0},
    {name: 'Female', value: 1}
];


const PatientProfile: FC = () => {
    const {patientId} = useParams()
    const [action, setAction] = useState<FormAction>(initialState);
    const {control, formState: {errors}, handleSubmit, getValues, setValue, reset} = useForm({defaultValues});
    const {isSuccess, isLoading, error, data} = useQuery<Patient, Error>(
        'patient', async () => await patientService.getById(patientId), {
            onSuccess: (data) => {
                setAction({
                    title: data?.id ? `View/Edit Patient` : 'Register New',
                    showEdit: data?.id ? true : false
                });
                setValue('id', data?.id ? data?.id : 0);
                setValue('firstName', data?.patientName?.firstName ? data?.patientName?.firstName : '');
                setValue('lastName', data?.patientName?.lastName ? data?.patientName?.lastName : '');
                setValue('gender', data?.gender ? data.gender : Gender.Male);
                setValue('birthDate', data?.birthDate ? new Date(data.birthDate) : new Date());

            }
        });

    const mutation = useMutation(async (formData:Patient) => {
        if (action.showEdit)
        {
            console.log(formData)
            return await patientService.update( formData)
        }else {
            return await patientService.register( formData)
            reset()
        }
    })

    const onSubmit = (fdata:any) => {
        mutation.mutate(fdata)
    }

    if (isLoading) return <ProgressBar mode="indeterminate" style={{height: '6px'}}></ProgressBar>

    if (error) return <Message severity="error" text={`An error has occurred: ${error.message}`}/>

    return (
        <Card title={action.title}>
            <form onSubmit={handleSubmit(onSubmit)} className="flex flex-column gap-2">
                {mutation.isError &&<Message severity="error" text={`An error has occurred: ${mutation.error}`}/>}
                {mutation.isSuccess && <Message severity="success" text={`successfully saved.`}/>}
                <Controller
                    name="firstName"
                    control={control}
                    rules={{required: 'First Name is required'}}
                    render={({field, fieldState}) => (
                        <>
                            <label htmlFor={field.name} className={classNames({'p-error': errors.firstName})}></label>
                            <span className="p-float-label">
                                <InputText id={field.name} value={field.value}
                                           className={classNames({'p-invalid': fieldState.error})}
                                           onChange={(e) => field.onChange(e.target.value)}/>
                                <label htmlFor={field.name}>First Name</label>
                            </span>
                            <small className="p-error">{errors?.lastName?.message}</small>
                        </>
                    )}
                />

                <Controller
                    name="lastName"
                    control={control}
                    rules={{required: 'Last Name is required.'}}
                    render={({field, fieldState}) => (
                        <>
                            <label htmlFor={field.name} className={classNames({'p-error': errors.lastName})}></label>
                            <span className="p-float-label">
                                <InputText id={field.name} value={field.value}
                                           className={classNames({'p-invalid': fieldState.error})}
                                           onChange={(e) => field.onChange(e.target.value)}/>
                                <label htmlFor={field.name}>Last Name</label>
                            </span>
                            <small className="p-error">{errors?.firstName?.message}</small>
                        </>
                    )}
                />

                <Controller
                    name="gender"
                    control={control}
                    rules={{required: 'Gender is required.'}}
                    render={({field, fieldState}) => (
                        <>
                            <Dropdown
                                value={field.value}
                                optionLabel="name"
                                placeholder="Select a Gender"
                                name="city"
                                options={genders}
                                onChange={(e) => field.onChange(e.value)}
                                className={classNames({'p-invalid': fieldState.error})}
                            />
                            <small className="p-error">{errors?.gender?.message}</small>
                        </>
                    )}
                />

                <Controller
                    name="birthDate"
                    control={control}
                    rules={{required: 'Birth Date is required.'}}
                    render={({field, fieldState}) => (
                        <>
                            <label htmlFor={field.name}>Birth Date</label>
                            <Calendar inputId={field.name} value={field.value} onChange={field.onChange}
                                      dateFormat="dd/mm/yy" className={classNames({'p-invalid': fieldState.error})}/>
                            <small className="p-error">{errors?.birthDate?.message}</small>
                        </>
                    )}
                />
                <Button label="Edit" disabled={!action.showEdit}/>
                <Button label="Save Changes" type="submit" icon="pi pi-check"/>
            </form>
        </Card>
    );
}
export default PatientProfile;
