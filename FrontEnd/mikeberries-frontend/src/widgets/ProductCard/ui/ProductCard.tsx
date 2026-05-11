import Card from '@mui/material/Card';
import styles from "./ProductCard.module.css"
import {Button, CardActionArea, CardActions, CardContent, CardMedia } from "@mui/material";
import type {ProductCardProps} from '../model/ProductCardProps';
import { useNavigate } from "react-router-dom";

export default function ProductCard(props: ProductCardProps) {
    const navigate = useNavigate();

    const handleCardClick = () => {
        navigate(`/products/${props.id}`);
    };

    return(
        <Card className={styles.productCard}>
            <CardActionArea onClick={handleCardClick} sx = {{ width: '100%', height: '100%' }}>
                <CardMedia
                    className={styles.productCardMedia}
                    component="img"
                    image={props.imageUrl}
                />
                <CardContent className={styles.price}>
                    {props.price}
                </CardContent>
                <CardContent className={styles.title}>
                    {props.title}
                </CardContent>
            </CardActionArea>
            <CardActions className={styles.actions}>
                <Button variant="contained" className={styles.button}>
                    Добавить в корзину
                </Button>
            </CardActions>
        </Card>
    );
}
