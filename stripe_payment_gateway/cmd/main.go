package main

import (
	"context"
	"fmt"
	"log"
	"os"
	"stripe_payment_gateway/config"
	"stripe_payment_gateway/internal/payment"
	"stripe_payment_gateway/internal/redis"
	"time"
)

func main() {
	// ตั้งค่า Environment Variables
	os.Setenv("STRIPE_API_KEY", "sk_test_51I1MoJLiNasWpAoWHROPNNHErQwlsXhUyhRjX1MbjxYpWztLXuLMQelzMum6HTOf27iqDLZmYTn2gmqbLfeulGEV00AAsH2yqt")
	os.Setenv("REDIS_HOST", "localhost")
	os.Setenv("REDIS_PORT", "6379")

	// เริ่มต้นระบบ
	config.InitStripe()
	redis.InitRedis()

	// สร้าง Context
	ctx := context.Background()

	// ตัวอย่างการสร้างบัญชีผู้ใช้
	user := payment.UserAccount{
		ID:        "user_12345",
		Name:      "John Doe",
		Email:     "john.doe@example.com",
		CreatedAt: time.Now(),
	}
	fmt.Printf("Created user: %+v\n", user)

	/*


		// ตัวอย่างการสร้างคำขอชำระเงิน
		paymentRequest := payment.PaymentRequest{
			ID:              "req_98765",
			UserID:          user.ID,
			Amount:          100.50,
			Currency:        "USD",
			PaymentMethodID: "pm_card_visa",
			Description:     "Order #001",
			CreatedAt:       time.Now(),
		}
		fmt.Printf("Created payment request: %+v\n", paymentRequest)

		// จำลองการชำระเงินสำเร็จ
		paymentResponse := payment.PaymentResponse{
			ID:          "ch_abc123",
			Status:      "completed",
			Amount:      paymentRequest.Amount,
			Currency:    paymentRequest.Currency,
			CreatedAt:   time.Now(),
			UserID:      user.ID,
		}
		fmt.Printf("Payment response: %+v\n", paymentResponse)

		// สร้างข้อมูลธุรกรรม
		transaction := payment.Transaction{
			ID:              "txn_54321",
			PaymentRequest:  paymentRequest,
			PaymentResponse: paymentResponse,
			CreatedAt:       time.Now(),
		}
		fmt.Printf("Transaction: %+v\n", transaction)

		// บันทึกธุรกรรมลงระบบ (สมมติเป็นการพิมพ์ออกมา)
		log.Println("Transaction completed successfully!")
		log.Printf("Transaction Details: %+v\n", transaction)

	*/
	// จำลองการชำระเงิน
	err := payment.ProcessPayment(ctx, user.Name, 5000, "usd", "tok_visa")
	if err != nil {
		log.Fatalf("Payment failed: %v", err)
	}

	// ตรวจสอบการชำระเงินใน Redis
	value, err := redis.GetValue(ctx, "payment:ch_xxxxxxx")
	if err == nil {
		log.Printf("Payment Status: %s", value)
	} else {
		log.Printf("Error retrieving payment status: %v", err)
	}
}
