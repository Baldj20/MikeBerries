import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.tsx'
import {ThemeProvider} from "@mui/material";
import theme from "../shared/ui/Theme.ts";
import { BrowserRouter } from "react-router-dom";
import {KcPage} from "../keycloak-theme/kc.gen.tsx";


const root = createRoot(document.getElementById('root')!);

const kcContext = window.kcContext;

if (kcContext !== undefined) {

    root.render(
        <ThemeProvider theme={theme}>
            <StrictMode>
                <KcPage kcContext={kcContext} />
            </StrictMode>
        </ThemeProvider>
    );
} else {

    root.render(
        <ThemeProvider theme={theme}>
            <StrictMode>
                <BrowserRouter>
                    <App />
                </BrowserRouter>
            </StrictMode>
        </ThemeProvider>
    );
}
