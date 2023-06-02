import {Service} from "../../../shared/models";

export interface Encounter {
    patientId?: number;
    service?: Service;
    created?: Date;
    id?: number;
}