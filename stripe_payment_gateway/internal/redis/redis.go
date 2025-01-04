package redis

import (
	"context"
	"fmt"
	"stripe_payment_gateway/config"

	"github.com/go-redis/redis/v8"
)

var Rdb *redis.Client

func InitRedis() {
	host, port := config.GetRedisConfig()
	Rdb = redis.NewClient(&redis.Options{
		Addr: fmt.Sprintf("%s:%s", host, port),
	})
}

func SetValue(ctx context.Context, key string, value string) error {
	return Rdb.Set(ctx, key, value, 0).Err()
}

func GetValue(ctx context.Context, key string) (string, error) {
	return Rdb.Get(ctx, key).Result()
}
