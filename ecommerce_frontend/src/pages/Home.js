import React, { useState } from "react";
import { Link } from "react-router-dom";
import { useDispatch } from 'react-redux';
import ProductCard from "../components/ProductCard";
import { motion } from "framer-motion";
//import { useCart } from "../CartContext";
import { addToCart } from '../cartSlice';

const products = [
  { id: 1, name: "Product 1", price: 100, image: "/images/product1.png" },
  { id: 2, name: "Product 2", price: 200, image: "/images/product2.png" },
  { id: 3, name: "Product 3", price: 300, image: "/images/product3.png" },
  { id: 4, name: "Product 4", price: 400, image: "/images/product4.png" },
  { id: 5, name: "Product 5", price: 500, image: "/images/product5.png" },
  { id: 6, name: "Product 6", price: 600, image: "/images/product6.png" },
];

function Home() {
  const [cart, setCart] = useState([]);
  const [animationProps, setAnimationProps] = useState(null);
  //const { addToCart } = useCart();
  const dispatch = useDispatch();
 

  const handleAddToCart = ({ startX, startY, endX, endY, image,product }) => {
    setAnimationProps({ startX, startY, endX, endY, image });
    //addToCart({ image });
    dispatch(addToCart(product)); // เพิ่มสินค้าเข้าตะกร้า
    // เพิ่มสินค้าในตะกร้า
    setCart((prevCart) => [...prevCart, image]);

    // รีเซ็ตแอนิเมชันหลังจากเล่นจบ
    setTimeout(() => setAnimationProps(null), 1000);
  };

  return (
    <div className="container mx-auto py-8">
      <h1 className="text-3xl font-bold mb-6">Products</h1>
      <div className="grid grid-cols-2 gap-4">
        {products.map((product) => (
          <ProductCard
            key={product.id}
            product={product}
            onAddToCart={handleAddToCart}
          />
        ))}
      </div>

      {/* ไอคอนตะกร้า */}

      <Link to="/cart" className="text-lg">
        <div
          id="cart-icon"
          className="fixed bottom-4 right-4 bg-blue-500 text-white p-4 rounded-full flex items-center justify-center"
        >
          🛒
          <span className="ml-2 bg-red-500 text-white text-sm rounded-full px-2 py-1">
            {cart.length}
          </span>
        </div>
      </Link>
      {/* แอนิเมชันสินค้าไหลไปตะกร้า */}
      {animationProps && (
        <motion.img
          src={animationProps.image} // ใช้รูปสินค้า
          alt="Product Animation"
          className="fixed w-10 h-10 object-cover rounded-full"
          style={{
            top: animationProps.startY,
            left: animationProps.startX,
          }}
          animate={{
            top: animationProps.endY,
            left: animationProps.endX,
            scale: 0.5,
            opacity: 0,
          }}
          transition={{ duration: 1 }}
        />
      )}
    </div>
  );
}

export default Home;
