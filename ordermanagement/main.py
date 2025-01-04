from fastapi import FastAPI, Depends, HTTPException
from sqlalchemy.orm import Session
from database import SessionLocal, engine
from models import Base
from crud import create_customer, create_product, create_order, get_customer_orders
from schemas import CustomerCreate, ProductCreate, OrderCreate, Customer, Product, Order

# สร้างฐานข้อมูล
Base.metadata.create_all(bind=engine)

# สร้างแอป FastAPI
app = FastAPI()

# Dependency สำหรับ Session
def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

# API สำหรับ Customer
@app.post("/customers/", response_model=Customer)
def create_new_customer(customer: CustomerCreate, db: Session = Depends(get_db)):
    return create_customer(db=db, customer=customer)

# API สำหรับ Product
@app.post("/products/", response_model=Product)
def create_new_product(product: ProductCreate, db: Session = Depends(get_db)):
    return create_product(db=db, product=product)

# API สำหรับ Order
@app.post("/orders/", response_model=Order)
def create_new_order(order: OrderCreate, db: Session = Depends(get_db)):
    return create_order(db=db, order=order)

# API สำหรับดึงข้อมูล Order ของลูกค้า
@app.get("/customers/{customer_id}/orders/", response_model=list[Order])
def get_orders_by_customer(customer_id: int, db: Session = Depends(get_db)):
    orders = get_customer_orders(db=db, customer_id=customer_id)
    if not orders:
        raise HTTPException(status_code=404, detail="Orders not found")
    return orders

# ตัวอย่างการใช้งาน
#python -m uvicorn main:app --reload
