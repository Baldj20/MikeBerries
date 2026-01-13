import type {GetProductDto} from "./GetProductDto.ts";

export interface GetProductsListDto {
    items: GetProductDto[];
    totalPages: number;
    currentPage: number;
}
