package config

import (
	"os"

	"github.com/stripe/stripe-go/v74"
)

func InitStripe() {
	stripe.Key = os.Getenv("STRIPE_API_KEY")
}

func GetRedisConfig() (string, string) {
	host := os.Getenv("REDIS_HOST")
	port := os.Getenv("REDIS_PORT")
	return host, port
}
