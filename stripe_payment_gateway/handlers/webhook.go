package handlers

import (
	"log"
	"net/http"

	"github.com/gofiber/fiber/v2"
	"github.com/stripe/stripe-go/v74/webhook"
)

const WebhookSecret = "whsec_your_secret"

func HandleWebhook(c *fiber.Ctx) error {
	payload := c.Body()
	sigHeader := c.Get("Stripe-Signature")

	// ตรวจสอบความถูกต้องของ Webhook
	event, err := webhook.ConstructEvent(payload, sigHeader, WebhookSecret)
	if err != nil {
		log.Printf("Webhook error: %v", err)
		return c.SendStatus(http.StatusBadRequest)
	}

	// จัดการเหตุการณ์ที่ได้รับ
	switch event.Type {
	case "charge.succeeded":
		log.Println("Charge succeeded!")
		// เพิ่มการจัดการข้อมูลที่ต้องการ
	case "charge.failed":
		log.Println("Charge failed!")
	default:
		log.Printf("Unhandled event type: %s\n", event.Type)
	}

	return c.SendStatus(http.StatusOK)
}
