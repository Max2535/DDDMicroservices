import React from "react";
import { useSelector, useDispatch } from "react-redux";
import { removeFromCart, updateQuantity } from "../cartSlice";

function Cart() {
  const cart = useSelector((state) => state.cart);
  const dispatch = useDispatch();

  // Group By Product (จัดกลุ่มสินค้า)
  const groupedCart = cart.reduce((acc, product) => {
    const existingProduct = acc.find((item) => item.id === product.id);
    if (existingProduct) {
      existingProduct.quantity += 1;
    } else {
      acc.push({ ...product, quantity: product.quantity || 1 }); // กำหนดค่าเริ่มต้นให้ quantity
    }
    return acc;
  }, []);

  const handleQuantityChange = (id, quantity) => {
    if (quantity < 1) return; // ป้องกันจำนวนที่น้อยกว่า 1
    dispatch(updateQuantity({ id, quantity })); // เรียก Redux Action
  };

  const handleRemove = (id) => {
    dispatch(removeFromCart({ id }));
  };

  // คำนวณราคารวม
  const totalPrice = groupedCart.reduce(
    (total, item) => total + item.price * item.quantity,
    0
  );

  return (
    <div className="container mx-auto py-8">
      <h1 className="text-3xl font-bold mb-6">Your Cart</h1>
      {groupedCart.length === 0 ? (
        <p>No items in cart</p>
      ) : (
        <>
          <ul className="space-y-4">
            {groupedCart.map((item) => (
              <li
                key={item.id}
                className="flex items-center justify-between border p-4 rounded"
              >
                <div className="flex items-center">
                  <img
                    src={item.image}
                    alt={item.name}
                    className="w-16 h-16 object-cover mr-4"
                  />
                  <div>
                    <h2 className="text-lg font-bold">{item.name}</h2>
                    <p className="text-gray-600">
                      Price: ${item.price} x {item.quantity} = $
                      {item.price * item.quantity}
                    </p>
                  </div>
                </div>
                <div className="flex items-center">
                  <input
                    type="number"
                    value={item.quantity}
                    min="1"
                    onChange={(e) =>
                      handleQuantityChange(item.id, parseInt(e.target.value, 10))
                    }
                    className="w-16 text-center border rounded"
                  />
                  <button
                    onClick={() => handleRemove(item.id)}
                    className="bg-red-500 text-white px-4 py-2 ml-4 rounded"
                  >
                    Remove
                  </button>
                </div>
              </li>
            ))}
          </ul>
          <div className="mt-6 text-right">
            <h2 className="text-2xl font-bold">Total: ${totalPrice.toFixed(2)}</h2>
          </div>
        </>
      )}
    </div>
  );
}

export default Cart;
