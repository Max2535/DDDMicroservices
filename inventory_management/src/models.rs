use diesel::prelude::*;
use serde::{Deserialize, Serialize};
use crate::schema::*;

#[derive(Queryable, Identifiable)]
#[diesel(table_name = products)]
pub struct Product {
    pub id: i32,
    pub sku: String,
    pub name: String,
    pub price: f64,
    pub created_at: chrono::NaiveDateTime,
}

#[derive(Queryable, Identifiable)]
#[diesel(table_name = stock_balance)]
pub struct StockBalance {
    pub id: i32,
    pub product_id: i32,
    pub quantity: i32,
    pub updated_at: chrono::NaiveDateTime,
}

#[derive(Insertable)]
#[diesel(table_name = stock_movements)]
pub struct NewStockMovement {
    pub product_id: i32,
    pub movement_type: String,
    pub quantity: i32,
}
