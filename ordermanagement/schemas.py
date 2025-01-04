from pydantic import BaseModel
from typing import List, Optional
from datetime import datetime

# Schemas สำหรับ Customer
class CustomerBase(BaseModel):
    name: str
    email: str

class CustomerCreate(CustomerBase):
    pass

class Customer(CustomerBase):
    id: int
    class Config:
        orm_mode = True

# Schemas สำหรับ Product
class ProductBase(BaseModel):
    name: str
    price: float

class ProductCreate(ProductBase):
    pass

class Product(ProductBase):
    id: int
    class Config:
        orm_mode = True

# Schemas สำหรับ Order
class OrderItem(BaseModel):
    product_id: int
    quantity: int

class OrderCreate(BaseModel):
    customer_id: int
    items: List[OrderItem]

class Order(BaseModel):
    id: int
    customer_id: int
    order_date: datetime
    items: List[OrderItem]
    class Config:
        orm_mode = True
