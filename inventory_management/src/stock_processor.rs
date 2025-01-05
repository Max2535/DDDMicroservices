use diesel::prelude::*;
use diesel::ExpressionMethods; // สำหรับ eq()
use diesel::QueryDsl; // สำหรับ filter()
use diesel::RunQueryDsl; // สำหรับ execute()
use crate::schema::stock_balance::dsl as stock_balance_dsl;
use crate::schema::stock_movements::dsl as stock_movements_dsl;
use crate::models::NewStockMovement;

pub fn deduct_stock(
    conn: &mut PgConnection,
    product_id_value: i32,
    quantity_value: i32,
) -> Result<(), diesel::result::Error> {
    use stock_balance_dsl::*;

    // ตรวจสอบจำนวนคงเหลือ
    let current_stock = stock_balance
        .filter(product_id.eq(product_id_value))
        .select(quantity)
        .first::<i32>(conn)?;

    if current_stock < quantity_value {
        return Err(diesel::result::Error::RollbackTransaction);
    }

    // อัปเดตจำนวนคงเหลือ
    diesel::update(stock_balance.filter(product_id.eq(product_id_value)))
        .set(quantity.eq(current_stock - quantity_value))
        .execute(conn)?;

    // บันทึกการเคลื่อนไหว
    let new_movement = NewStockMovement {
        product_id: product_id_value,
        movement_type: "Outbound".to_string(),
        quantity: quantity_value,
    };

    diesel::insert_into(stock_movements_dsl::stock_movements)
        .values(&new_movement)
        .execute(conn)?;

    Ok(())
}
