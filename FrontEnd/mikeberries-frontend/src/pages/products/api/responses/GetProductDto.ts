import type {GetProductImageDto} from "./GetProductImageDto.ts";

export interface GetProductDto {
    title: string;
    description: string;
    price: number;
    images: GetProductImageDto[];
    provider_email: string;
    provider_name: string;
}
