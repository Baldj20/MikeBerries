import {Box, Button, MobileStepper, Typography} from "@mui/material";
import {KeyboardArrowLeft, KeyboardArrowRight} from "@mui/icons-material";
import {useEffect, useState} from "react";
import { useParams } from "react-router-dom";
import {apiInstance} from "../../../shared/api/AxiosClient.ts";
import type {GetProductDto} from "../../../entities/Product/GetProductDto.ts";
import type {ApiResponse} from "../../../shared/api/ApiResponse.ts";

function ProductPage() {
    const { id } = useParams();
    const [currentIndex, setCurrentIndex] = useState(0);
    const [product, setProduct] = useState<GetProductDto>({
        id: "",
        title: "",
        price: 0,
        description: "",
        images: [],
        provider:{
            name: "",
            email: ""
        }
    } as GetProductDto);

    const handleNext = () => setCurrentIndex(currentIndex + 1);
    const handlePrev = () => setCurrentIndex(currentIndex - 1);

    useEffect(() => {
        const fetchProducts = async() => {
            console.log(id);
            const response = await apiInstance.get<ApiResponse<GetProductDto>>(`products/${id}`);
            console.log(response);
            setProduct(response.data.value);

        }
        fetchProducts();
    }, [id]);

    if (!product || !product.images || product.images.length === 0) {
        return <p>Loading product details...</p>;
    }

    return (
        <>
            <Box style = {{ height: "20%" }} />
            <div style={{display: "flex"}}>
                <Box style = {{ width: "10%" }} />
                <div style={{ display: "flex", flexDirection: "column", width: "30%", justifySelf: "center" }}>
                    <Box component="img" sx = {{ height: "20vw" }} src = {product.images[currentIndex].url} />
                    <MobileStepper
                        variant="dots"
                        steps={product.images.length}
                        position="static"
                        activeStep={currentIndex}
                        nextButton={
                            <Button
                                size="small"
                                onClick={handleNext}
                                disabled={currentIndex == product.images.length - 1}
                            >
                                <KeyboardArrowRight />
                            </Button>
                        }
                        backButton={
                            <Button size="small"
                                    onClick={handlePrev}
                                    disabled={currentIndex == 0}>
                                <KeyboardArrowLeft />
                            </Button>
                        }
                    />
                </div>
                <Box style = {{ width: "10%" }} />
                <div style = {{ display: "flex", flexDirection: "column"}}>
                    <Typography variant="h1" noWrap>
                        {product.title}
                    </Typography>
                    <Typography variant="h1" noWrap>
                        {product.price}
                    </Typography>
                    <Typography variant="h6" noWrap>
                        {product.description}
                    </Typography>
                </div>
            </div>
        </>
    )
}

export default ProductPage;
