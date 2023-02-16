import {AxiosInstance} from "axios";
import {configService} from "../../../shared/configs/config-service";

class AuthService {
    private http: AxiosInstance;

    constructor() {
        this.http = configService.getHttpClient("/bff");
    }

    async getClaims() {
        const result = await this.http.get(`/user`);
        return result.data || [];
    }
}

const authService = new AuthService();
export { authService };
