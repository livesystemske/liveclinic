import {Encounter} from "./encounter";
import {Gender} from "../../shared/models/gender";
import {PersonName} from "../../shared/models/person-name";

export interface Patient {
    memberNo?: string;
    patientName?: PersonName;
    gender?: Gender;
    birthDate?: Date;
    created?: Date;
    encounters?: Encounter[];
    id?: number;
}

