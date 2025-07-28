import axios from 'axios';
import { AUTH_API_BASE_URL } from '../constants/api';

const apiClientAuth = axios.create({
    baseURL: AUTH_API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
    withCredentials: true,
});

export default apiClientAuth;
