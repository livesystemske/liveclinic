import axios, {AxiosInstance} from "axios";
import {QueryClient} from "react-query";

class ConfigService {
    getQueryClient() {

        const queryClient = new QueryClient(
            {
                defaultOptions: {
                    queries: {
                        refetchOnWindowFocus: false,
                        refetchOnMount: false,
                        refetchOnReconnect: false,
                        retry: false,
                        staleTime: 5 * 60 * 1000,
                    },
                },
            }
        )
        return queryClient;
    }

    getHttpClient(url = "") {

        const axiosInstance: AxiosInstance = axios.create({
            baseURL: url === "" ? process.env.REACT_APP_LCS_API_URL : url,
            withCredentials: true,
            headers: {
                'X-CSRF': '1',
                "Content-type": "application/json",
            },
        });
        return axiosInstance;
    }
}

const configService = new ConfigService();
export {configService};


