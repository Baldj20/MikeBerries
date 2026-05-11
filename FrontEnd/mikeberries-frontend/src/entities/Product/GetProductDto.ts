import type {GetProductImageDto} from "./GetProductImageDto.ts";
import type {ProviderDto} from "../Provider/ProviderDto.ts";

export interface GetProductDto{
    id: string;
    title: string;
    description?: string;
    price: number;
    images: GetProductImageDto[];
    provider: ProviderDto
}
