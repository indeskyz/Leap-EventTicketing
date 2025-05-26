import api, { handleApiError } from "./api";
import { EventSortField, SortDirection, PaginatedResponse, SalesSummary } from './types';
import { AxiosError } from 'axios';

export const fetchEvents = async (
    page: number = 1,
    pageSize: number = 10,
    sortField?: EventSortField,
    sortDirection?: SortDirection
): Promise<PaginatedResponse<Event>> => {
    try {
        const response = await api.get<PaginatedResponse<Event>>('/events', {
            params: {
                page,
                pageSize,
                sortField,
                sortDirection,
            },
        });
        return response.data;
    } catch (error) {
        return handleApiError(error as AxiosError);
    }
};

export const fetchTopEventsBySales = async (
    page: number = 1,
    pageSize: number = 5 // Default to 5 for top events
): Promise<PaginatedResponse<SalesSummary>> => {
    try {
        const response = await api.get<PaginatedResponse<SalesSummary>>('/events/top-sales', {
            params: { page, pageSize },
        });
        return response.data;
    } catch (error) {
        return handleApiError(error as AxiosError);
    }
};