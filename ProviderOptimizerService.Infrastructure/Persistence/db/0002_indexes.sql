-- 0002_indexes.sql (PostgreSQL)
BEGIN;

-- Búsquedas frecuentes
CREATE INDEX IF NOT EXISTS idx_providers_is_available
  ON providers (is_available);

CREATE INDEX IF NOT EXISTS idx_provider_caps_service_type
  ON provider_capabilities (service_type);

-- Acceso rápido por assistance_id
CREATE UNIQUE INDEX IF NOT EXISTS ux_assistance_requests_assistance_id
  ON assistance_requests (assistance_id);

-- Outbox: despacho por procesados/antigüedad y control de reintentos
CREATE INDEX IF NOT EXISTS idx_outbox_processed
  ON outbox_messages (processed_on_utc);

CREATE INDEX IF NOT EXISTS idx_outbox_attempts
  ON outbox_messages (attempts);

-- Idempotencia: expiración
CREATE INDEX IF NOT EXISTS idx_idem_expires
  ON idempotency_records (expires_at_utc);

COMMIT;