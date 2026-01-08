import axios from 'axios';

export const apiInstance = axios.create({
    baseURL: 'https://localhost:7196/api',
    timeout: 10000
});