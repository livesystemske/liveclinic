import axios, {AxiosInstance} from "axios";
import {Patient} from "../models";

const baseUrl='/api/'

class PatientService {
    url='registry/patients';
    private http: AxiosInstance;
    constructor() {
        this.http = axios.create({
            baseURL: baseUrl,
            headers: {
                "Content-type": "application/json",
            },

        });
    }

    async getAll() {
        const result = await this.http.get<Patient[]>(`${this.url}`);
        return result.data || [];
    }

    async getById(id: number) {
        const result = await this.http.get<Patient>(`${this.url}/${id}`);
        return result.data || [];
    }
}

const patientService = new PatientService();
export { patientService };
