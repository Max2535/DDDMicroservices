from database import Base, engine
from models import Customer, Product, Order, OrderItem

# สร้างตาราง
def init_db():
    Base.metadata.create_all(bind=engine)

if __name__ == "__main__":
    init_db()
