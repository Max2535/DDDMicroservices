package handlers

import (
	"log"
	"os"

	"github.com/gofiber/fiber/v2"
	"github.com/redis/go-redis/v9"
	"github.com/stripe/stripe-go/v74"
	"github.com/stripe/stripe-go/v74/charge"
	"golang.org/x/net/context"
)

var ctx = context.Background()

// ตั้งค่า Redis Client
var redisClient = redis.NewClient(&redis.Options{
	Addr: os.Getenv("REDIS_ADDR"), // โหลดที่อยู่ Redis จากไฟล์ .env
})

// ProcessPayment รับคำขอเพื่อชำระเงิน
func ProcessPayment(c *fiber.Ctx) error {

	// อ่านข้อมูลจากคำขอ
	type PaymentRequest struct {
		Amount   int64  `json:"amount"`
		Currency string `json:"currency"`
		Source   string `json:"source"`
	}
	req := new(PaymentRequest)
	if err := c.BodyParser(req); err != nil {
		return c.Status(fiber.StatusBadRequest).JSON(fiber.Map{
			"error": "Invalid request",
		})
	}

	// ตั้งค่า Stripe Key
	stripe.Key = os.Getenv("STRIPE_SECRET_KEY")

	// เรียกใช้ Stripe API
	params := &stripe.ChargeParams{
		Amount:   stripe.Int64(req.Amount),
		Currency: stripe.String(req.Currency),
	}
	params.SetSource(req.Source) // ใช้ SetSource สำหรับการกำหนดค่า Source

	ch, err := charge.New(params)
	if err != nil {
		log.Printf("Stripe error: %v", err)
		return c.Status(fiber.StatusInternalServerError).JSON(fiber.Map{
			"error": "Payment failed",
		})
	}

	err = redisClient.Set(ctx, ch.ID, string(ch.Status), 0).Err() // บันทึก status เป็น string
	if err != nil {
		log.Printf("Redis error: %v", err)
		return c.Status(fiber.StatusInternalServerError).JSON(fiber.Map{
			"error": "Failed to save payment data",
		})
	}

	// ตอบกลับข้อมูลสำเร็จ
	return c.JSON(fiber.Map{
		"status":  "success",
		"charge":  ch.ID,
		"message": "Payment processed successfully",
	})
}
