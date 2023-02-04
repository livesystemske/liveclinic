import React from 'react';
import ReactDOM from 'react-dom/client';
import "primereact/resources/themes/tailwind-light/theme.css";  //theme
import "primereact/resources/primereact.min.css";                  //core css
import "primeicons/primeicons.css";
import './index.css';
import App from './App';
import { QueryClientProvider } from "react-query";
import {configService} from "./shared/configs/config-service";

const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);

root.render(
    <React.StrictMode>
        <QueryClientProvider client={configService.getQueryClient()}>
            <App/>
        </QueryClientProvider>
    </React.StrictMode>
);
