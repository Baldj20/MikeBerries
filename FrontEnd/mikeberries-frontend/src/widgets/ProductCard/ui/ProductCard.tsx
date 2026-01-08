import Card from '@mui/material/Card';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import CardMedia from '@mui/material/CardMedia';
import CardContent from '@mui/material/CardContent';
import CardActions from '@mui/material/CardActions';
import type {Product} from "../../../entities/Product/Product.ts";
import {Typography} from "@mui/material";

export default function ProductCard({title, description, price, images}: Product) {
    return(
        <Card sx={{ maxWidth: 345 }}>
            <CardMedia
                component="img"
                height="194"
                image={images[0]}
            />
            <CardContent>
                <Typography component="h1" variant="h5">
                    {title} <br />
                    {description} <br />
                    {price}
                </Typography>
            </CardContent>
            <CardActions>
                <ShoppingCartIcon />
            </CardActions>
        </Card>
    );
}