import {apiInstance} from "../../../shared/api/AxiosClient.ts";
import type {GetProductsListDto} from "./responses/GetProductsListDto.ts";
import type {GetProductDto} from "./responses/GetProductDto.ts";
import type {Product} from "../../../entities/Product/Product.ts";

export const mapDtoToProduct = (dto: GetProductDto): Product => {
    return {
        id: "1",
        title: dto.title,
        description: dto.description,
        price: dto.price,
        images: dto.images.map((img) => img.url)
    };
};

export const productsService = {
    async getAll(page = 1, pageSize = 10) {
        const response = await apiInstance.get<GetProductsListDto>('/Products', {
            params: {
                page: page,
                pageSize: pageSize
            }
        });
        return response.data;
    },
};

