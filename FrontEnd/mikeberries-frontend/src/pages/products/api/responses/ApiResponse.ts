export interface ApiResponse<T> {
    value: T;
    isSuccess: boolean;
    error: string;
    statusCode: number;
}
