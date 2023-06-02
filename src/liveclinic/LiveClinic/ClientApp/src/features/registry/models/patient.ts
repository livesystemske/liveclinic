import {Gender, PersonName} from "../../../shared/models";
import {Encounter} from "./encounter";

export interface Patient {
    memberNo?: string;
    patientName?: PersonName;
    gender?: Gender;
    birthDate?: Date;
    created?: Date;
    encounters?: Encounter[];
    id?: number;
}

