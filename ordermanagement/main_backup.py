from database import SessionLocal
from models import Customer, Product, Order, OrderItem

# สร้าง Session
db = SessionLocal()

# เพิ่มข้อมูลลูกค้า
def add_customer(name, email):
    customer = Customer(name=name, email=email)
    db.add(customer)
    db.commit()
    db.refresh(customer)
    return customer

# เพิ่มข้อมูลสินค้า
def add_product(name, price):
    product = Product(name=name, price=price)
    db.add(product)
    db.commit()
    db.refresh(product)
    return product

# เพิ่มคำสั่งซื้อ
def create_order(customer_id, items):
    order = Order(customer_id=customer_id)
    db.add(order)
    db.commit()
    db.refresh(order)
    for item in items:
        order_item = OrderItem(order_id=order.id, product_id=item['product_id'], quantity=item['quantity'])
        db.add(order_item)
    db.commit()
    return order

# ดึงคำสั่งซื้อของลูกค้า
def get_customer_orders(customer_id):
    orders = db.query(Order).filter(Order.customer_id == customer_id).all()
    for order in orders:
        print(f"Order ID: {order.id}, Date: {order.order_date}")
        for item in order.items:
            print(f"  Product ID: {item.product_id}, Quantity: {item.quantity}")


# ตัวอย่างการใช้งาน
if __name__ == "__main__":
    # เพิ่มลูกค้า
    customer = add_customer("John Doe", "john@example.com")
    print(f"Added Customer: {customer.name}")

    # เพิ่มสินค้า
    product1 = add_product("Laptop", 1000.00)
    product2 = add_product("Mouse", 25.00)
    print(f"Added Products: {product1.name}, {product2.name}")

    # สร้างคำสั่งซื้อ
    order = create_order(customer_id=customer.id, items=[
        {"product_id": product1.id, "quantity": 1},
        {"product_id": product2.id, "quantity": 2}
    ])
    print(f"Created Order ID: {order.id}")
