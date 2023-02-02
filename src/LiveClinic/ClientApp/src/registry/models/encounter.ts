import {Service} from "../../shared/models/service";

export interface Encounter {
    patientId?: number;
    service?: Service;
    created?: Date;
    id?: number;
}
