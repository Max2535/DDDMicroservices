package main

import (
	"log"
	"stripe_payment_gateway/handlers"

	"github.com/gofiber/fiber/v2"
	"github.com/joho/godotenv"
)

func main() {
	// โหลด environment variables
	err := godotenv.Load()
	if err != nil {
		log.Fatal("Error loading .env file")
	}

	// ตั้งค่า Fiber
	app := fiber.New()

	// เส้นทางการชำระเงิน
	app.Post("/payment", handlers.ProcessPayment)

	// เส้นทาง Webhook
	app.Post("/webhook", handlers.HandleWebhook)

	// เริ่มเซิร์ฟเวอร์
	log.Fatal(app.Listen(":3000"))
}
