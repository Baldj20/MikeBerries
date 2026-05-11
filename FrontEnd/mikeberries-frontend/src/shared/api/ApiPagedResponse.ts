export interface ApiPagedResponse<T>{
    value:{
        items: T[];
        currentPage: number;
        totalPages: number;
    }
    isSuccess: boolean;
}
