package payment

import "time"

// UserAccount - โครงสร้างสำหรับข้อมูลบัญชีผู้ใช้
type UserAccount struct {
	ID        string    `json:"id"`
	Name      string    `json:"name"`
	Email     string    `json:"email"`
	CreatedAt time.Time `json:"created_at"`
}

// PaymentRequest - โครงสร้างสำหรับข้อมูลคำขอการชำระเงิน
type PaymentRequest struct {
	ID              string    `json:"id"`
	UserID          string    `json:"user_id"`
	Amount          float64   `json:"amount"`
	Currency        string    `json:"currency"`
	PaymentMethodID string    `json:"payment_method_id"`
	Description     string    `json:"description"`
	CreatedAt       time.Time `json:"created_at"`
}

// PaymentResponse - โครงสร้างสำหรับการตอบกลับของการชำระเงิน
type PaymentResponse struct {
	ID           string    `json:"id"`
	Status       string    `json:"status"` // เช่น "completed", "failed"
	Amount       float64   `json:"amount"`
	Currency     string    `json:"currency"`
	CreatedAt    time.Time `json:"created_at"`
	UserID       string    `json:"user_id"`
	ErrorMessage string    `json:"error_message,omitempty"`
}

// Transaction - โครงสร้างสำหรับเก็บข้อมูลการทำธุรกรรม
type Transaction struct {
	ID              string          `json:"id"`
	PaymentRequest  PaymentRequest  `json:"payment_request"`
	PaymentResponse PaymentResponse `json:"payment_response"`
	CreatedAt       time.Time       `json:"created_at"`
}
