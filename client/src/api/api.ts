import axios, { type AxiosError, type AxiosInstance, type AxiosRequestConfig } from 'axios';

const api: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  },
});

api.interceptors.request.use((config) => {
  if (config.params) {
    const searchParams = new URLSearchParams();
    Object.entries(config.params).forEach(([key, value]) => {
      if (value !== undefined && value !== null) {
        const capitalizedKey = key.charAt(0).toUpperCase() + key.slice(1);
        searchParams.append(capitalizedKey, String(value));
      }
    });
    config.params = searchParams;
  }
  return config;
});


// Response interceptor for handling errors globally
api.interceptors.response.use(
  (response) => response,
  (error: AxiosError) => {
    if (error.code === 'ECONNABORTED') {
      return Promise.reject(new Error('Request timeout. Please try again.'));
    }
    return Promise.reject(error);
  }
);

/**
 * Handles API errors and throws formatted error messages
 * @param error The Axios error object
 * @throws Error with formatted message
 */
export const handleApiError = (error: AxiosError): never => {
  if (error.response) {
    const status = error.response.status;
    let message = 'Unknown error occurred';

    if (error.response.data && typeof error.response.data === 'object') {
      const data = error.response.data as any;
      message = data.message || data.title || JSON.stringify(data);
    } else if (typeof error.response.data === 'string') {
      message = error.response.data;
    }
    switch (status) {
      case 400:
        throw new Error(`Bad Request: ${message}`);
      case 401:
        throw new Error('Unauthorized: Please login again');
      case 403:
        throw new Error('Forbidden: You don\'t have permission to access this resource');
      case 404:
        throw new Error('Not Found: The requested resource was not found');
      case 500:
        throw new Error(`Server Error: ${message}`);
      default:
        throw new Error(`HTTP Error ${status}: ${message}`);
    }
  } else if (error.request) {
    throw new Error('Network Error: Could not connect to the server. Please check your connection.');
  } else {
    throw new Error(`Request Error: ${error.message}`);
  }
};

/**
 * Makes a GET request to the API
 * @param url The endpoint URL
 * @param params Optional query parameters
 * @param config Optional Axios config
 * @returns Promise with the response data
 */
export const get = async <T>(url: string, params?: object, config?: AxiosRequestConfig): Promise<T> => {
  try {
    const response = await api.get<T>(url, { params, ...config });
    return response.data;
  } catch (error) {
    return handleApiError(error as AxiosError);
  }
};

/**
 * Makes a POST request to the API
 * @param url The endpoint URL
 * @param data The request body data
 * @param config Optional Axios config
 * @returns Promise with the response data
 */
export const post = async <T>(url: string, data?: object, config?: AxiosRequestConfig): Promise<T> => {
  try {
    const response = await api.post<T>(url, data, config);
    return response.data;
  } catch (error) {
    return handleApiError(error as AxiosError);
  }
};

/**
 * Makes a PUT request to the API
 * @param url The endpoint URL
 * @param data The request body data
 * @param config Optional Axios config
 * @returns Promise with the response data
 */
export const put = async <T>(url: string, data?: object, config?: AxiosRequestConfig): Promise<T> => {
  try {
    const response = await api.put<T>(url, data, config);
    return response.data;
  } catch (error) {
    return handleApiError(error as AxiosError);
  }
};

/**
 * Makes a DELETE request to the API
 * @param url The endpoint URL
 * @param config Optional Axios config
 * @returns Promise with the response data
 */
export const del = async <T>(url: string, config?: AxiosRequestConfig): Promise<T> => {
  try {
    const response = await api.delete<T>(url, config);
    return response.data;
  } catch (error) {
    return handleApiError(error as AxiosError);
  }
};

export default {
  get,
  post,
  put,
  delete: del,
  handleApiError
};