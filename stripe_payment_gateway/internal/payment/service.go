package payment

import (
	"context"
	"fmt"
	"stripe_payment_gateway/adapters/stripe"
	"stripe_payment_gateway/internal/redis"
)

func ProcessPayment(ctx context.Context, userID string, amount int64, currency, source string) error {
	// สร้าง Payment Request ผ่าน Stripe
	charge, err := stripe.CreateCharge(amount, currency, source, fmt.Sprintf("Payment by user: %s", userID))
	if err != nil {
		return err
	}

	// บันทึกสถานะการชำระเงินลง Redis
	err = redis.SetValue(ctx, fmt.Sprintf("payment:%s", charge.ID), "completed")
	if err != nil {
		return err
	}

	fmt.Printf("Payment successful: %s\n", charge.ID)
	return nil
}
