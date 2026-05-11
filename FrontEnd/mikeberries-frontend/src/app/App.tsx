import styles from './App.module.css';
import HomePage from "../pages/home/ui/HomePage.tsx";
import { Routes, Route } from "react-router-dom";
import ProductPage from "../pages/products/ui/ProductPage.tsx";


function App() {
  return (
    <div className={styles.app}>
        <Routes>
            <Route path="/products" element={<HomePage />} />
            <Route path="/products/:id" element={<ProductPage />} />
        </Routes>
    </div>
  )
}

export default App

//title="aaa" description="bbb" price={50} images={["https://img.freepik.com/premium-photo/small-duck-background-green-grass_390194-3302.jpg", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQI-SaYWlXmVicHWYEEpRgrmFir507tWQk3pA&s"]} />
