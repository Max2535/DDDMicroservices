use diesel::prelude::*;

diesel::table! {
    products (id) {
        id -> Int4,
        sku -> Varchar,
        name -> Varchar,
        price -> Numeric,
        created_at -> Timestamp,
    }
}

diesel::table! {
    stock_balance (id) {
        id -> Int4,
        product_id -> Int4,
        quantity -> Int4,
        updated_at -> Timestamp,
    }
}

diesel::table! {
    stock_movements (id) {
        id -> Int4,
        product_id -> Int4,
        movement_type -> Varchar,
        quantity -> Int4,
        created_at -> Timestamp,
    }
}

diesel::joinable!(stock_balance -> products (product_id));
diesel::joinable!(stock_movements -> products (product_id));

diesel::allow_tables_to_appear_in_same_query!(
    products,
    stock_balance,
    stock_movements,
);
