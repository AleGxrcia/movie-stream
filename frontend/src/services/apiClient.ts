import axios from 'axios';
import { API_BASE_URL } from '../constants/api';

const apiClient = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
    withCredentials: true,
});

export default apiClient;