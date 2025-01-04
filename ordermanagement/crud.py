from sqlalchemy.orm import Session
from models import Customer, Product, Order, OrderItem
from schemas import CustomerCreate, ProductCreate, OrderCreate

# ฟังก์ชันสำหรับ Customer
def create_customer(db: Session, customer: CustomerCreate):
    db_customer = Customer(name=customer.name, email=customer.email)
    db.add(db_customer)
    db.commit()
    db.refresh(db_customer)
    return db_customer

# ฟังก์ชันสำหรับ Product
def create_product(db: Session, product: ProductCreate):
    db_product = Product(name=product.name, price=product.price)
    db.add(db_product)
    db.commit()
    db.refresh(db_product)
    return db_product

# ฟังก์ชันสำหรับ Order
def create_order(db: Session, order: OrderCreate):
    db_order = Order(customer_id=order.customer_id)
    db.add(db_order)
    db.commit()
    db.refresh(db_order)
    for item in order.items:
        db_order_item = OrderItem(order_id=db_order.id, product_id=item.product_id, quantity=item.quantity)
        db.add(db_order_item)
    db.commit()
    return db_order

def get_customer_orders(db: Session, customer_id: int):
    return db.query(Order).filter(Order.customer_id == customer_id).all()
