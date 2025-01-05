import React, { createContext, useContext, useState } from "react";

const CartContext = createContext();

export const CartProvider = ({ children }) => {
  const [cart, setCart] = useState([]);

  const addToCart = (product) => {
    setCart((prevCart) => [...prevCart, product]);
  };

  return (
    <CartContext.Provider value={{ cart, addToCart }}>
      {children}
    </CartContext.Provider>
  );
};

export const useCart = () => useContext(CartContext);
//Context API เหมาะสำหรับ state ที่มีการแชร์แบบง่าย ๆ เช่น theme, authentication, หรือตะกร้าสินค้า แต่ถ้าต้องการจัดการ state ที่ซับซ้อนขึ้น ควรใช้ Redux หรือ MobX แทน