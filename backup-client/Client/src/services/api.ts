import axios, { AxiosError, AxiosInstance, AxiosResponse } from 'axios';

const api: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
  timeout: 10000,
});

export const handleApiError = (error: AxiosError): never => {
  if (error.response) {
    throw new Error(
      `API Error: ${error.response.status} - ${error.response.data?.message || 'Unknown error'}`
    );
  } else if (error.request) {
    throw new Error('Network Error: Could not connect to the server');
  } else {
    throw new Error(`Request Error: ${error.message}`);
  }
};

export default api;