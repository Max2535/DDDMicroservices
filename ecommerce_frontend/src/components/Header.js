// import React from "react";
// import { Link } from "react-router-dom";
// import { useCart } from "../CartContext";

// function Header() {
//   const { cart } = useCart();

//   return (
//     <header className="bg-blue-500 text-white p-4">
//       <div className="container mx-auto flex justify-between">
//         <Link to="/" className="text-xl font-bold">My Shop</Link>
//         <Link to="/cart" className="text-lg">
//           🛒 Cart ({cart.length})
//         </Link>
//       </div>
//     </header>
//   );
// }

// export default Header;

import React from 'react';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';

function Header() {
  const cart = useSelector((state) => state.cart);

  return (
    <header className="bg-blue-500 text-white p-4">
      <div className="container mx-auto flex justify-between">
        <Link to="/" className="text-xl font-bold">My Shop</Link>
        <Link to="/cart" className="text-lg">
          🛒 Cart ({cart.length})
        </Link>
      </div>
    </header>
  );
}

export default Header;
// ใช้ useSelector จาก react-redux แทน useCart จาก CartContext