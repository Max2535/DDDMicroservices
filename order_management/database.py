from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker, declarative_base

# การตั้งค่าการเชื่อมต่อ PostgreSQL
DATABASE_URL = "postgresql://admin:yourpassword@localhost:5432/order_db"

# สร้าง Engine และ Session
engine = create_engine(DATABASE_URL)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

# Base สำหรับการสร้าง Model
Base = declarative_base()
