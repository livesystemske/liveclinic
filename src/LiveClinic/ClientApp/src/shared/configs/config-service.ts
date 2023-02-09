import axios, {AxiosInstance} from "axios";
import {QueryClient} from "react-query";

class ConfigService {
    getQueryClient() {

        const queryClient = new QueryClient(
            /*{
                defaultOptions: {
                    queries: {
                        refetchOnWindowFocus: false,
                        refetchOnMount: false,
                        refetchOnReconnect: false,
                        retry: false,
                        staleTime: 5 * 60 * 1000,
                    },
                },
            }*/
        )
        return queryClient;
    }

    getHttpClient() {

        const axiosInstance: AxiosInstance = axios.create({
            baseURL: process.env.REACT_APP_LCS_API_URL,
            headers: {
                "Content-type": "application/json",
            },
        });
        return axiosInstance;
    }
}

const configService = new ConfigService();
export {configService};


