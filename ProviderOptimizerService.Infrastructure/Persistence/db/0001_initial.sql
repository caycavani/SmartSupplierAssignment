-- 0001_initial.sql (PostgreSQL)
-- Crea el esquema mínimo para ProviderOptimizerService
-- Ejecutar dentro de una transacción
BEGIN;

-- PROVEEDORES
CREATE TABLE IF NOT EXISTS providers (
  id                UUID PRIMARY KEY,
  name              VARCHAR(200) NOT NULL,
  is_available      BOOLEAN      NOT NULL,
  rating            DOUBLE PRECISION NOT NULL,
  cost_per_km       DOUBLE PRECISION NOT NULL,
  coverage_radius_km DOUBLE PRECISION NOT NULL,
  lat               DOUBLE PRECISION NOT NULL,
  lng               DOUBLE PRECISION NOT NULL
);

-- Capacidades del proveedor (tabla puente)
CREATE TABLE IF NOT EXISTS provider_capabilities (
  provider_id UUID NOT NULL,
  service_type INT NOT NULL,
  PRIMARY KEY (provider_id, service_type),
  CONSTRAINT fk_provider_capabilities_provider
    FOREIGN KEY (provider_id) REFERENCES providers(id) ON DELETE CASCADE
);

-- SOLICITUDES
CREATE TABLE IF NOT EXISTS assistance_requests (
  id                 UUID PRIMARY KEY,
  assistance_id      VARCHAR(80) NOT NULL UNIQUE,
  service_type       INT NOT NULL,
  -- GeoPoint
  lat                DOUBLE PRECISION NOT NULL,
  lng                DOUBLE PRECISION NOT NULL,
  -- Constraints
  max_eta_minutes    INT NULL,
  preferred_networks TEXT NULL
);

-- OUTBOX (eventos de integración)
CREATE TABLE IF NOT EXISTS outbox_messages (
  id                UUID PRIMARY KEY,
  type              VARCHAR(250) NOT NULL,
  payload           JSONB NOT NULL,
  occurred_on_utc   TIMESTAMP WITHOUT TIME ZONE NOT NULL,
  processed_on_utc  TIMESTAMP WITHOUT TIME ZONE NULL,
  attempts          INT NOT NULL DEFAULT 0,
  correlation_id    VARCHAR(100) NULL,
  trace_id          VARCHAR(100) NULL,
  error             TEXT NULL
);

-- IDEMPOTENCIA (respuestas cacheadas por Idempotency-Key)
CREATE TABLE IF NOT EXISTS idempotency_records (
  key              VARCHAR(120) PRIMARY KEY,
  response         TEXT NOT NULL,
  created_at_utc   TIMESTAMP WITHOUT TIME ZONE NOT NULL,
  expires_at_utc   TIMESTAMP WITHOUT TIME ZONE NOT NULL
);

COMMIT;