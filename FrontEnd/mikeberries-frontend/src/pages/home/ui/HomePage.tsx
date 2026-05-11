import ProductCard from "../../../widgets/ProductCard/ui/ProductCard.tsx";
import {Box, IconButton, TextField, Toolbar} from "@mui/material";
import SearchIcon from '@mui/icons-material/Search';
import {useEffect, useState} from "react";
import {apiInstance} from "../../../shared/api/AxiosClient.ts";
import type {GetProductDto} from "../../../entities/Product/GetProductDto.ts";
import type {ApiPagedResponse} from "../../../shared/api/ApiPagedResponse.ts";
import "./HomePage.module.css";
import Account from "../../../widgets/Account/ui/Account.tsx";


function HomePage() {
    const [products, setProducts] = useState<GetProductDto[]>([]);

    useEffect(() => {
       const fetchProducts = async() =>{
            const response = await apiInstance.get<ApiPagedResponse<GetProductDto>>("products", {
                params: {
                    page: 1,
                    pagesize: 20
                }
            });
            setProducts(response.data.value.items);
       }
       fetchProducts();
    }, []);

    return(
        <>
            <Toolbar sx={{ backgroundColor: 'lightcoral' }}>
                <IconButton color="inherit">
                    <SearchIcon />
                </IconButton>
                <Box sx={{ flexGrow: 0.01 }}></Box>
                <TextField id="standard-basic" label="Search" variant="standard" sx={{mb:1.5, '& .MuiInput-underline:before': { borderBottomColor: 'black' },
                    '& .MuiInput-underline:after': { borderBottomColor: 'white' },
                    '& .MuiInputLabel-root': { color: 'white' },
                    '& .MuiInputLabel-root.Mui-focused': { color: 'white' },
                    '& input': { color: 'black' }}}/>
                <Box sx={{ flexGrow: 1 }}></Box>
                <Account />
            </Toolbar>
            <div style={{display: "flex", flexDirection: "column"}}>
                <Box  sx={{ minHeight: "1vh"}}/>
                <Box sx={{
                    display: 'grid',
                    gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))',
                    gap: 3
                }}>
                    {products.map((item) => (
                        <ProductCard key={item.title} title={item.title} price={item.price} imageUrl={item.images[0].url} id={item.id} />
                    ))}
                </Box>
            </div>
        </>
    );
}

export default HomePage;
