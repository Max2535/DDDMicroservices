import React from "react";
import { motion } from "framer-motion";

function ProductCard({ product, onAddToCart }) {

  const handleAddToCart = () => {
    const cartIcon = document.getElementById("cart-icon");
    if (cartIcon) {
      const cartIconPosition = cartIcon.getBoundingClientRect();
      const productImage = document.getElementById(`product-${product.id}`);
      const productPosition = productImage.getBoundingClientRect();
  
      // ส่งตำแหน่งพร้อม URL ของรูปสินค้า
      onAddToCart({
        startX: productPosition.left,
        startY: productPosition.top,
        endX: cartIconPosition.left,
        endY: cartIconPosition.top,
        image: product.image, // เพิ่ม URL รูปสินค้า
        product: product
      });
    }
  };
  

  return (
    <div className="border rounded p-4">
      <img
        id={`product-${product.id}`}
        src={product.image}
        alt={product.name}
        className="w-full h-40 object-cover mb-4"
      />
      <h2 className="text-xl font-bold">{product.name}</h2>
      <p className="text-lg text-gray-700">${product.price}</p>
      <button
        onClick={handleAddToCart}
        className="bg-blue-500 text-white py-1 px-4 rounded mt-2"
      >
        Add to Cart
      </button>
    </div>
  );
}

export default ProductCard;
