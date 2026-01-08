import {Grid} from "@mui/material";
import ProductCard from "../../../widgets/ProductCard/ui/ProductCard.tsx";
import {useEffect, useState} from "react";
import {mapDtoToProduct, productsService} from "../api/ProductsService.ts";
import type {GetProductDto} from "../api/responses/GetProductDto.ts";

function ProductsPage() {
    const [products, setProducts] = useState<GetProductDto[]>([]);

    useEffect(() => {
        const loadProducts = async () => {
            try {
                const data = await productsService.getAll(1, 10);
                setProducts(data.value.items);
            } catch (err) {
                console.error(err);
            }
        };

        loadProducts();
    }, []);

    return (
        <Grid container spacing={2} sx={{ padding: 2 }}>
            {products.map((dto, index) => (
                <Grid
                    key={index}
                    size={{ xs: 12, sm: 6, md: 4, lg: 3 }}
                >
                    <ProductCard {...mapDtoToProduct(dto)}  />
                </Grid>
            ))}
        </Grid>
    )
}

export default ProductsPage
