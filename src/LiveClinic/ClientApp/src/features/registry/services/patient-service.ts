import {AxiosInstance} from "axios";
import {Patient} from "../models";
import {configService} from "../../../shared/configs/config-service";

class PatientService {
    private url:string;
    private http: AxiosInstance;

    constructor() {
        this.url='/registry/patients';
        this.http = configService.getHttpClient();
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
