#[macro_use]
extern crate diesel;

mod schema;
mod models;
mod stock_processor;

use diesel::prelude::*;
use dotenv::dotenv;
use std::env;
use diesel::pg::PgConnection;


fn main() {
    dotenv().ok();

    let database_url = std::env::var("DATABASE_URL").expect("DATABASE_URL must be set");
    let mut connection = PgConnection::establish(&database_url)
        .expect(&format!("Error connecting to {}", database_url));

    match stock_processor::deduct_stock(&mut connection, 1, 10) {
        Ok(_) => println!("Stock deducted successfully."),
        Err(e) => println!("Failed to deduct stock: {:?}", e),
    }
}
